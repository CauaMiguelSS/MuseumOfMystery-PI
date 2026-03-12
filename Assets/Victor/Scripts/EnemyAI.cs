using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Investigate, Search }
    public EnemyState currentState = EnemyState.Patrol;

    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;
    public Transform[] patrolPoints;
    public JumpscareController jumpscareManager;

    [Header("Vision Settings")]
    public float viewDistance = 12f;
    public float viewAngle = 100f;
    public LayerMask visionMask;
    public LayerMask obstacleMask;

    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    [Header("Patrol Settings")]
    public float waitTimeAtPoints = 2f;
    private float waitTimer = 0f;
    private int patrolIndex = 0;

    [Header("Hearing Settings")]
    public float hearingRange = 10f;
    private Vector3 heardNoisePos;
    private bool heardNoise = false;

    [Header("Search Settings")]
    public float searchDuration = 7f;
    private float searchTimer = 0f;
    private Vector3 lastSeenPos;

    private bool jumpscareTriggered = false;

    private EnemyFootsteps footsteps;

    [Header("Start Delay")]
    public float startDelay = 5f;
    private float startTimer = 0f;
    private bool aiActive = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        footsteps = GetComponent<EnemyFootsteps>();

        if (footsteps != null)
        {
            footsteps.player = player;
        }
    }

    void Update()
    {
        if (!aiActive)
        {
            startTimer += Time.deltaTime;

            agent.isStopped = true;

            if (startTimer >= startDelay)
            {
                aiActive = true;
                agent.isStopped = false;
            }

            return;
        }

        if (jumpscareTriggered) return;

        DetectPlayer();

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Investigate:
                Investigate();
                break;

            case EnemyState.Search:
                Search();
                break;
        }
    }

    void DetectPlayer()
    {
        if (PlayerHiddenState.isHidden)
            return;

        Vector3 eyePos = transform.position + Vector3.up * 1.6f;
        Vector3 dir = (player.position - eyePos).normalized;
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > viewDistance) return;

        float angle = Vector3.Angle(transform.forward, dir);
        if (angle > viewAngle / 2f) return;

        if (Physics.Raycast(eyePos, dir, out RaycastHit hit, viewDistance, visionMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (!Physics.Raycast(eyePos, dir, dist, obstacleMask))
                {
                    lastSeenPos = player.position;
                    currentState = EnemyState.Chase;
                }
            }
        }
    }

    public void HearNoise(Vector3 noisePos)
    {
        if (Vector3.Distance(transform.position, noisePos) <= hearingRange)
        {
            heardNoise = true;
            heardNoisePos = noisePos;
            currentState = EnemyState.Investigate;
        }
    }

    void Patrol()
    {
        agent.speed = patrolSpeed;

        if (patrolPoints.Length == 0) return;

        if (heardNoise)
        {
            currentState = EnemyState.Investigate;
            return;
        }

        if (agent.remainingDistance < 0.3f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTimeAtPoints)
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

        agent.SetDestination(player.position);
        lastSeenPos = player.position;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < 1.5f && !jumpscareTriggered)
        {
            jumpscareTriggered = true;
            jumpscareManager.TriggerJumpscare();
            return;
        }

        if (dist > viewDistance * 1.3f)
        {
            currentState = EnemyState.Investigate;
        }
    }

    void Investigate()
    {
        agent.speed = patrolSpeed;

        Vector3 target = heardNoise ? heardNoisePos : lastSeenPos;
        agent.SetDestination(target);

        if (agent.remainingDistance < 0.4f)
        {
            heardNoise = false;
            currentState = EnemyState.Search;
            searchTimer = 0f;
        }
    }

    void Search()
    {
        agent.speed = patrolSpeed;
        searchTimer += Time.deltaTime;

        if (searchTimer >= searchDuration)
        {
            currentState = EnemyState.Patrol;
            agent.SetDestination(patrolPoints[patrolIndex].position);
        }
    }

    public void FreezeEnemy()
    {
        agent.isStopped = true;
        agent.ResetPath();
        this.enabled = false;
    }
}