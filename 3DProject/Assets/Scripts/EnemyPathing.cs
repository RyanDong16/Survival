using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathing : MonoBehaviour
{

    public NavMeshAgent navMesh;

    //public transform player;

    //public layerMask isGround, isplayer;
    public Vector3 walkPoint;
    bool destination;
    
    public float sightRange;
    public bool playerInRange;
    //Animator animate;

    void Start()
    { 
        //animate = GetComponent<Animator>();
    }

    private void Patorl(){
        //animate.SetBool("IsMoving", true);
    }

    private void ChasePlayer(){
        //animate.SetBool("IsMoving", true);
        ////chase down player 
        //animate.SetBool("IsMoving", false);
        ////call Attack
        
    }
    private void Attack()
    {
        //animate.SetBool("IsAttack", true);
        ////game play code for attacking 
        //animate.SetBool("IsAttack", false);
    }

}

