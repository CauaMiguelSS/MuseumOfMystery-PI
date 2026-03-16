using UnityEngine;
using UnityEngine.LowLevel;

public class JumpscareController : MonoBehaviour
{
    [Header("Player")]
    public GameObject playerCamera;
    public Rigidbody playerRigidbody;
    public MonoBehaviour[] playerMovementScripts;

    [Header("Enemies")]
    public GameObject normalEnemy;
    public MonoBehaviour enemyAIScript;
    public GameObject jumpscareEnemy;
    public Transform lookAtPoint;
    public float minDistanceFromPlayer = 2f;

    [Header("UI & Audio")]
    public GameObject deathScreen;
    public AudioSource jumpscareSound;

    private bool triggered = false;

    void Start()
    {
        deathScreen.SetActive(false);
        if (jumpscareEnemy != null)
            jumpscareEnemy.SetActive(false);
    }

    public void TriggerJumpscare()
    {
        if (triggered) return;
        triggered = true;

        PlayerLock.IsLocked = true;

        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.isKinematic = true;
        }

        foreach (var script in playerMovementScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        if (lookAtPoint != null)
        {
            Vector3 direction = (lookAtPoint.position - playerCamera.transform.position).normalized;
            playerCamera.transform.rotation = Quaternion.LookRotation(direction);
        }

        if (normalEnemy != null)
        {
            normalEnemy.SendMessage("FreezeEnemy", SendMessageOptions.DontRequireReceiver);

            normalEnemy.transform.LookAt(playerCamera.transform.position);

            if (enemyAIScript != null)
                enemyAIScript.enabled = false;

            MaintainDistance(normalEnemy.transform, playerCamera.transform, minDistanceFromPlayer);
        }

        if (jumpscareEnemy != null)
            jumpscareEnemy.SetActive(true);

        if (jumpscareSound != null)
            jumpscareSound.Play();

        Invoke(nameof(ShowDeathScreen), 0.8f);
    }

    private void MaintainDistance(Transform enemy, Transform player, float minDistance)
    {
        Vector3 offset = enemy.position - player.position;
        float currentDistance = offset.magnitude;

        if (currentDistance < minDistance)
        {
            Vector3 direction = offset.normalized;
            enemy.position = player.position + direction * minDistance;
        }
    }

    void ShowDeathScreen()
    {
        deathScreen.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}