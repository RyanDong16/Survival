using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Patrol,     // Moves between pointA ↔ pointB
    Chaser      // Tiger that chases the player
}

public class TestEnemy : MonoBehaviour
{
    [Header("Enemy Type")]
    public EnemyType enemyType = EnemyType.Patrol;

    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;
    private Transform target;

    [Header("Chase Settings")]
    public float detectionRange = 8f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private bool coolDown = false;

    [Header("Health")]
    public bool alive = true;

    [Header("Return Home Settings")]
    public float maxDistanceFromHome = 15f;
    private Vector3 homePosition;


    private NavMeshAgent agent;
    private Transform player;
    private Animator anim;
    private bool chasing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Start patrol at pointB
        target = pointB;
        agent.SetDestination(target.position);

        homePosition = transform.position;
    }

    void Update()
    {
        if (!alive) return;

        switch (enemyType)
        {
            case EnemyType.Chaser:
                ChaserBehavior();
                break;

            case EnemyType.Patrol:
                PatrolBehavior();
                break;
        }

        if (alive)
        CheckReturnHome();
    }

    void PatrolBehavior()
    {
        // If returning home, wait until we arrive
        if (Vector3.Distance(transform.position, homePosition) > 1f &&
            agent.destination == homePosition)
            return;

        // Normal patrol
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            target = target == pointA ? pointB : pointA;
            agent.SetDestination(target.position);
        }
    }


    void ChaserBehavior()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            chasing = true;

            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
        }
        else
        {
            chasing = false;
            PatrolBehavior();
        }
    }

    void CheckReturnHome()
    {
        float distFromHome = Vector3.Distance(transform.position, homePosition);

        if (distFromHome > maxDistanceFromHome)
        {
            chasing = false;
            agent.isStopped = false;
            agent.SetDestination(homePosition);
        }
    }

    void Attack()
    {
        if (!coolDown)
        {
            coolDown = true;
            agent.isStopped = true;

            // Face the player
            Vector3 lookDir = (player.position - transform.position).normalized;
            lookDir.y = 0;
            transform.forward = lookDir;

            anim.SetTrigger("Attack");
            StartCoroutine(ResetAttackCooldown());
        }
    }

    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        coolDown = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword") && alive)
        {
            alive = false;
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        agent.isStopped = true;
        anim.SetTrigger("Die"); // Animation

        yield return new WaitForSeconds(2f); // match animation length

        Destroy(gameObject);
    }

}
