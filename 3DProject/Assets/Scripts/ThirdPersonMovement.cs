using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody rb;
    public Transform Camera;
    public Vector3 jump;

    public float speed = 6f;
    public float jumpForce = 5f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public bool isGrounded;
    //not sure about this 
    int runningForwardHash;
    int combatHash;
    int deathHash;
    int takeDamageHash;
    int attackHash;

    Animator animate;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0f, 2f, 0f);
        animate = GetComponent<Animator>();
        runningForwardHash = Animator.StringToHash("runningForward");
        combatHash=Animator.StringToHash("combat");
        deathHash=Animator.StringToHash("death");
        takeDamageHash=Animator.StringToHash("takeDamage");
        attackHash=Animator.StringToHash("attack");

    }

    private void OnCollisionStay()
    {
        isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        bool runningForward = animate.GetBool(runningForwardHash);
        //bool isJumping = animate.GetBool(name, true);
        bool combat = animate.GetBool(combatHash);
        bool death = animate.GetBool(deathHash);
        bool takeDamage = animate.GetBool(takeDamageHash);
        bool attack = animate.GetBool(attackHash);
        

        //get input from player 
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {

            //rotates the players body
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //moves the player 
            Vector3 Velocity = moveDir.normalized * speed;


            Velocity.y = rb.velocity.y;

            rb.velocity = Velocity;

            if (!runningForward)
            {

               animate.SetBool("runningForward", true);
            }

        }
        else {
            //checks when to siwtch between the states 
            if (runningForward)
            {
                animate.SetBool("runningForward", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

       

    }

    void Jump()
    {
        rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        //set to false after jumping 
        //jumpTriggered = false;
        //animation should be in this method 
    }
}