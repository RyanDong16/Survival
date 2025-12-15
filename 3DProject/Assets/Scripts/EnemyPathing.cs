using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathing : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float detectionRange = 8f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public bool coolDown = false;

    private NavMeshAgent agent;
    private Transform player;
    private Transform target;
    private bool chasing = false;

    public bool health;

    private Animator anim;

    public bool Tigger;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = pointB;
        agent.SetDestination(target.position);
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange){
            // Chase Player
            chasing = true;

            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else
            {
                // Chase player
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
        }
        else
        {
            // Resume patrol
            if (chasing)
            {
                chasing = false;
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                target = target == pointA ? pointB : pointA;
                agent.SetDestination(target.position);
            }
        }
    }

    void Attack(){

        if (!coolDown)
        {
            coolDown = true;
            // Trigger animation or damage logic here
            // Stop moving
            agent.isStopped = true;

            // Face the player
            Vector3 lookDir = (player.position - transform.position).normalized;
            lookDir.y = 0; // keep rotation flat
            transform.forward = lookDir;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            health = false;
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        agent.isStopped = true;                 // stop movement
        anim.SetTrigger("Die");                 // play animation
        yield return new WaitForSeconds(2f);    // wait for animation length
        Destroy(gameObject);                    // remove enemy
    }


}
