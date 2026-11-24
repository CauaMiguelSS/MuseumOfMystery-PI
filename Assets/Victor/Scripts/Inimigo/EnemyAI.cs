
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Investigate, Search }
    public State state = State.Patrol;

    [Header("References")]
    public Transform player;
    public Transform[] patrolPoints;
    public AudioSource footstepAudio;
    public JumpscareController jumpscareManager;
    public Animator animator;

    [Header("Vision")]
    public float viewDistance = 12f;
    public float viewAngle = 100f;
    public LayerMask visionMask;

    [Header("Hearing")]
    public float hearingRange = 10f;

    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float stopDistance = 2.2f;

    [Header("Footsteps")]
    public float maxStepDistance = 20f;

    [Header("Timers")]
    public float waitTime = 2f;
    public float searchDuration = 7f;
    public float startDelay = 0f;

    NavMeshAgent agent;
    int patrolIndex;
    float waitTimer;
    float searchTimer;
    float startTimer;

    bool heardNoise;
    bool aiActive;
    bool jumpscareTriggered;

    Vector3 heardPos;
    Vector3 lastSeenPos;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (!animator)
            animator = GetComponentInChildren<Animator>();

        if (footstepAudio)
        {
            footstepAudio.loop = true;
            footstepAudio.playOnAwake = false;
            footstepAudio.spatialBlend = 1f;
            footstepAudio.minDistance = 1f;
            footstepAudio.maxDistance = maxStepDistance;
            footstepAudio.volume = 0f;
        }
    }

    void Update()
    {
        if (!aiActive)
        {
            startTimer += Time.deltaTime;

            if (agent)
                agent.isStopped = true;

            if (startTimer >= startDelay)
            {
                aiActive = true;

                if (agent)
                    agent.isStopped = false;
            }

            return;
        }

        if (jumpscareTriggered)
            return;

        UpdateFootsteps();
        DetectPlayer();

        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Investigate:
                Investigate();
                break;

            case State.Search:
                Search();
                break;
        }

        UpdateAnimation();
    }

    void UpdateFootsteps()
    {
        if (!footstepAudio || !player || !agent)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > maxStepDistance)
        {
            footstepAudio.Stop();
            return;
        }

        bool walking = agent.velocity.magnitude > 0.1f && !agent.isStopped;

        if (walking && !footstepAudio.isPlaying)
            footstepAudio.Play();
        else if (!walking && footstepAudio.isPlaying)
            footstepAudio.Stop();

        footstepAudio.volume = 1f - Mathf.Clamp01(distance / maxStepDistance);
    }

    void UpdateAnimation()
    {
        if (!animator || !agent || !agent.enabled)
            return;

        float speed = 0f;

        if (!agent.isStopped && agent.velocity.magnitude > 0.1f)
            speed = state == State.Chase ? 2f : 1f;

        animator.SetFloat("Speed", speed);
    }

    void DetectPlayer()
    {
        if (!player)
            return;

        Vector3 eye = transform.position + Vector3.up * 1.6f;
        Vector3 direction = player.position - eye;
        float distance = direction.magnitude;

        if (distance > viewDistance)
            return;

        direction.Normalize();

        if (Vector3.Angle(transform.forward, direction) > viewAngle / 2f)
            return;

        if (Physics.Raycast(eye, direction, out RaycastHit hit, viewDistance, visionMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                lastSeenPos = player.position;
                state = State.Chase;
            }
        }
    }

    public void HearNoise(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) <= hearingRange)
        {
            heardNoise = true;
            heardPos = position;
            state = State.Investigate;
        }
    }

    void Patrol()
    {
        agent.speed = patrolSpeed;

        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        if (heardNoise)
        {
            state = State.Investigate;
            return;
        }

        if (agent.remainingDistance < 0.3f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                waitTimer = 0f;
                patrolIndex = Random.Range(0, patrolPoints.Length);
                agent.SetDestination(patrolPoints[patrolIndex].position);
            }
        }
    }

    void Chase()
    {
        agent.speed = chaseSpeed;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= stopDistance)
        {
            TriggerJumpscare();
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);

        lastSeenPos = player.position;

        if (distance > viewDistance * 1.3f)
            state = State.Investigate;
    }

    void Investigate()
    {
        agent.speed = patrolSpeed;
        agent.isStopped = false;

        agent.SetDestination(heardNoise ? heardPos : lastSeenPos);

        if (agent.remainingDistance < 0.4f)
        {
            heardNoise = false;
            searchTimer = 0f;
            state = State.Search;
        }
    }

    void Search()
    {
        agent.speed = patrolSpeed;
        searchTimer += Time.deltaTime;

        if (searchTimer >= searchDuration)
        {
            state = State.Patrol;

            if (patrolPoints != null && patrolPoints.Length > 0)
                agent.SetDestination(patrolPoints[patrolIndex].position);
        }
    }

    void TriggerJumpscare()
    {
        if (jumpscareTriggered)
            return;

        jumpscareTriggered = true;

        if (footstepAudio)
            footstepAudio.Stop();

        if (jumpscareManager)
            jumpscareManager.TriggerJumpscare();
    }

    public void FreezeEnemy()
    {
        jumpscareTriggered = true;

        if (footstepAudio)
            footstepAudio.Stop();

        if (agent && agent.enabled)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
        }

        if (animator)
            animator.SetFloat("Speed", 0f);

        enabled = false;
    }
}
