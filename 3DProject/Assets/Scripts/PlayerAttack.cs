using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private AudioClip attackSound;
    private Animator anim;
    private ThirdPersonMovement playerMovement;
    private float coolDownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent <Animator>();
        playerMovement = GetComponent<ThirdPersonMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && coolDownTimer > attackCooldown && playerMovement.CanAttack()) {
            Attack();
        }

        coolDownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        SoundManager.Instance.PlaySound(attackSound);
        anim.SetTrigger("Attack");
        coolDownTimer = 0; 
    }

}
