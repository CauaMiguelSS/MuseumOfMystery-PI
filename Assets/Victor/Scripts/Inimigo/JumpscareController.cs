using UnityEngine;

public class JumpscareController : MonoBehaviour
{
    [Header("Player")]
    public Transform playerCamera;
    public Rigidbody playerRb;
    public MonoBehaviour[] playerScripts;

    [Header("Enemy")]
    public GameObject enemy;
    public MonoBehaviour enemyAI;
    public Transform headTarget;
    public float distanceFromCamera = 1.5f;

    [Header("UI & Audio")]
    public GameObject deathScreen;
    public AudioSource sound;
    public float deathScreenDelay = 0.8f;

    [Header("Camera")]
    public float cameraSnapSpeed = 10f;

    bool triggered;
    Transform lookTarget;

    void Start()
    {
        if (deathScreen)
            deathScreen.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
            TriggerJumpscare();
    }

    void LateUpdate()
    {
        if (!lookTarget) return;

        Vector3 direction = lookTarget.position - playerCamera.position;
        playerCamera.rotation = Quaternion.Slerp(
            playerCamera.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime * cameraSnapSpeed
        );
    }

    public void TriggerJumpscare()
    {
        if (triggered) return;

        triggered = true;

        LockPlayer();
        FreezeEnemy();
        PositionEnemy();

        lookTarget = headTarget;

        if (sound)
            sound.Play();

        Invoke(nameof(ShowDeathScreen), deathScreenDelay);
    }

    void LockPlayer()
    {
        if (playerRb)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerRb.isKinematic = true;
        }

        foreach (var script in playerScripts)
            if (script) script.enabled = false;
    }

    void FreezeEnemy()
    {
        if (enemyAI)
            enemyAI.enabled = false;

        enemy.SendMessage("FreezeEnemy", SendMessageOptions.DontRequireReceiver);
    }

    void PositionEnemy()
    {
        Vector3 position = playerCamera.position +
                           playerCamera.forward * distanceFromCamera;

        position.y = enemy.transform.position.y;
        enemy.transform.position = position;

        Vector3 direction = playerCamera.position - enemy.transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
            enemy.transform.rotation = Quaternion.LookRotation(direction);
    }

    void ShowDeathScreen()
    {
        if (deathScreen)
            deathScreen.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}