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
    public float distanceFromCamera = 1.5f;
    public Vector3 faceOffset = new Vector3(0, 1.6f, 0); // altura do rosto

    [Header("UI & Audio")]
    public GameObject deathScreen;
    public AudioSource sound;
    public float deathScreenDelay = 0.8f;

    bool triggered;
    bool lockCamera;

    Transform lookTarget; // alvo fixo (rosto)

    void Start()
    {
        if (deathScreen) deathScreen.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            TriggerJumpscare();
        }
    }

    void LateUpdate()
    {
        // LateUpdate = executa DEPOIS de todos scripts impede bug de c�mera
        if (lockCamera && lookTarget)
        {
            Vector3 dir = (lookTarget.position - playerCamera.position).normalized;
            playerCamera.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void TriggerJumpscare()
    {
        triggered = true;
        lockCamera = true;

        LockPlayer();
        FreezeEnemy();
        PositionEnemyInFront();
        CreateLookTarget();

        if (sound) sound.Play();

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

        foreach (var s in playerScripts)
        {
            if (s) s.enabled = false;
        }
    }

    void FreezeEnemy()
    {
        if (enemyAI) enemyAI.enabled = false;

        enemy.SendMessage("FreezeEnemy", SendMessageOptions.DontRequireReceiver);
    }

    void PositionEnemyInFront()
    {
        // Coloca inimigo direto na frente da c�mera
        Vector3 forward = playerCamera.forward;
        Vector3 pos = playerCamera.position + forward * distanceFromCamera;

        enemy.transform.position = pos;

        // Faz ele olhar direto pro jogador
        enemy.transform.LookAt(playerCamera.position);
    }

    void CreateLookTarget()
    {
        // Cria um ponto no "rosto" do inimigo
        GameObject target = new GameObject("JumpscareTarget");

        target.transform.position = enemy.transform.position + faceOffset;

        lookTarget = target.transform;
    }

    void ShowDeathScreen()
    {
        if (deathScreen) deathScreen.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}