using UnityEngine;

public class JumpscareController : MonoBehaviour
{
    [Header("Player")]
    public Transform playerCamera;
    public Rigidbody playerRb;
    public MonoBehaviour[] playerScripts;

    [Header("Enemy")]
    public GameObject normalEnemy;
    public MonoBehaviour enemyAI;
    public GameObject jumpscareEnemy;
    public Transform lookTarget;
    public float minDistance = 2f;

    [Header("UI & Audio")]
    public GameObject deathScreen;
    public AudioSource sound;
    public float deathScreenDelay = 0.8f;

    bool triggered;

    void Start()
    {
        SetActive(deathScreen, false);
        SetActive(jumpscareEnemy, false);
    }

    public void TriggerJumpscare()
    {
        if (triggered) return;
        triggered = true;

        LockPlayer();
        HandleEnemy();
        PlayEffects();

        Invoke(nameof(ShowDeathScreen), deathScreenDelay);
    }

    void LockPlayer()
    {
        PlayerLock.IsLocked = true;

        if (playerRb)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerRb.isKinematic = true;
        }

        foreach (var s in playerScripts)
            if (s) s.enabled = false;

        if (lookTarget)
        {
            Vector3 dir = (lookTarget.position - playerCamera.position).normalized;
            playerCamera.rotation = Quaternion.LookRotation(dir);
        }
    }

    void HandleEnemy()
    {
        if (!normalEnemy) return;

        normalEnemy.SendMessage("FreezeEnemy", SendMessageOptions.DontRequireReceiver);

        if (enemyAI) enemyAI.enabled = false;

        normalEnemy.transform.LookAt(playerCamera.position);
        KeepDistance(normalEnemy.transform);
    }

    void KeepDistance(Transform enemy)
    {
        Vector3 dir = (enemy.position - playerCamera.position).normalized;
        float dist = Vector3.Distance(enemy.position, playerCamera.position);

        if (dist < minDistance)
            enemy.position = playerCamera.position + dir * minDistance;
    }

    void PlayEffects()
    {
        SetActive(jumpscareEnemy, true);

        if (sound) sound.Play();
    }

    void ShowDeathScreen()
    {
        SetActive(deathScreen, true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void SetActive(GameObject obj, bool state)
    {
        if (obj) obj.SetActive(state);
    }
}