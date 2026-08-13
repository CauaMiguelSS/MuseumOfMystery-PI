using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip movementSound;
    public AudioClip fSound;
    public AudioClip shiftSound;

    void Update()
    {
        bool moving =
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow);

        bool shift =
            Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift);

        // SHIFT + movimento
        if (shift && moving)
        {
            PlayLoop(shiftSound);
        }
        // Movimento normal
        else if (moving)
        {
            PlayLoop(movementSound);
        }
        // Parado
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.clip = null;
        }

        // Som do F
        if (Input.GetKeyDown(KeyCode.F))
        {
            audioSource.PlayOneShot(fSound);
        }
    }

    void PlayLoop(AudioClip sound)
    {
        if (sound == null) return;

        if (audioSource.clip != sound || !audioSource.isPlaying)
        {
            audioSource.Stop();

            audioSource.clip = sound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}