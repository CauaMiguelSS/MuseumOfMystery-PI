using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip movementSound;
    public AudioClip fSound;

    void Update()
    {
        // Som ao andar com WASD ou setas
        bool moving =
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow);

        if (moving)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = movementSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.clip == movementSound && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        // Som diferente ao apertar F
        if (Input.GetKeyDown(KeyCode.F))
        {
            audioSource.loop = false;
            audioSource.PlayOneShot(fSound);
        }
    }
}