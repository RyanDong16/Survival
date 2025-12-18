using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private AudioClip attackSound;
    private Animator anim;
    private ThirdPersonMovement playerMovement;
    private float coolDownTimer = Mathf.Infinity;

    [Header("Hitbox")]
    public GameObject Sword;
    public float hitboxDistance = 1f;
    public float hitboxDuration = 0.2f;

    

    private void Awake()
    {
        anim = GetComponent <Animator>();
        playerMovement = GetComponent<ThirdPersonMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && coolDownTimer > attackCooldown && playerMovement.CanAttack()) {
            StartCoroutine(Attack());
        }

        coolDownTimer += Time.deltaTime;
    }

    private IEnumerator Attack()
    {
        GameObject hitbox = Instantiate(Sword);

        anim.SetTrigger("Attack");
        SoundManager.Instance.PlaySound(attackSound);
        float timer = 0f;
        while (timer < 0.5f)
        {
            hitbox.transform.position = transform.position + transform.forward * 1f;
            hitbox.transform.rotation = transform.rotation;

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(hitbox);
    }


}
