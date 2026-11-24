
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
    public Animator enemyAnimator;
    public float distanceFromCamera = 2.5f;

    [Header("Animation")]
    public string jumpscareTrigger = "Jumpscare";
    public string jumpscareStateName = "Jumpscare";
    public float animationWaitLimit = 10f;

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

        if (!enemyAnimator && enemy)
            enemyAnimator = enemy.GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
            TriggerJumpscare();
    }

    void LateUpdate()
    {
        if (!lookTarget || !playerCamera)
            return;

        Vector3 direction = lookTarget.position - playerCamera.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            playerCamera.rotation = Quaternion.Slerp(
                playerCamera.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * cameraSnapSpeed
            );
        }
    }

    public void TriggerJumpscare()
    {
        if (triggered)
            return;

        triggered = true;

        LockPlayer();
        FreezeEnemy();
        PositionEnemy();

        lookTarget = headTarget;

        if (sound)
            sound.Play();

        StartCoroutine(PlayJumpscare());
    }

    void LockPlayer()
    {
        if (playerRb)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerRb.isKinematic = true;
        }

        foreach (MonoBehaviour script in playerScripts)
        {
            if (script)
                script.enabled = false;
        }
    }

    void FreezeEnemy()
    {
        if (enemyAI)
        {
            enemyAI.SendMessage("FreezeEnemy", SendMessageOptions.DontRequireReceiver);
        }
        else if (enemy)
        {
            EnemyAI ai = enemy.GetComponent<EnemyAI>();

            if (ai)
                ai.FreezeEnemy();
        }

        Transform root = enemyAI ? enemyAI.transform : enemy.transform;

        NavMeshAgent agent = root.GetComponent<NavMeshAgent>();

        if (agent && agent.enabled)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
        }
    }

    void PositionEnemy()
    {
        Transform root = enemyAI ? enemyAI.transform : enemy.transform;

        Vector3 forward = playerCamera.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude < 0.001f)
            forward = playerCamera.forward;

        forward.Normalize();

        Vector3 position = playerCamera.position + forward * distanceFromCamera;

        position.y = root.position.y;

        root.position = position;

        Vector3 direction = playerCamera.position - root.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
            root.rotation = Quaternion.LookRotation(direction);
    }

    IEnumerator PlayJumpscare()
    {
        if (!enemyAnimator)
        {
            ShowDeathScreen();
            yield break;
        }

        enemyAnimator.applyRootMotion = false;
        enemyAnimator.ResetTrigger(jumpscareTrigger);
        enemyAnimator.SetTrigger(jumpscareTrigger);

        float timer = 0f;
        bool animationStarted = false;

        while (timer < animationWaitLimit)
        {
            AnimatorStateInfo state = enemyAnimator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName(jumpscareStateName))
            {
                animationStarted = true;
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (!animationStarted)
        {
            ShowDeathScreen();
            yield break;
        }

        while (true)
        {
            AnimatorStateInfo state = enemyAnimator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName(jumpscareStateName) && state.normalizedTime >= 1f)
                break;

            timer += Time.deltaTime;

            if (timer >= animationWaitLimit)
                break;

            yield return null;
        }

        yield return new WaitForSeconds(deathScreenDelay);

        ShowDeathScreen();
    }

    void ShowDeathScreen()
    {
        if (deathScreen)
            deathScreen.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
