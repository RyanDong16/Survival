using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform Camera;
    private Animator animate;

    private Vector3 slopeSlideVelocity;
    private Vector3 moveDir;

    public float speed = 6f;
    public float jumpForce = 5f;
    public float airMult = 0.4f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float horizontal;
    float vertical;

    public bool isGrounded;
    public bool isSliding;

    public float playerHeight;

    public float maxSlopeAngle;
    private RaycastHit slopeHit;
  

    //not sure about this 
    //int runningForwardHash;
    //int combatHash;
    //int deathHash;
    //int takeDamageHash;
    //int attackHash;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        animate = GetComponent<Animator>();
        //runningForwardHash = Animator.StringToHash("runningForward");
        //combatHash=Animator.StringToHash("combat");
        //deathHash=Animator.StringToHash("death");
        //takeDamageHash=Animator.StringToHash("takeDamage");
        //attackHash=Animator.StringToHash("attack");


    }

    //updates isGrounded
    private void OnCollisionStay(Collision coll)
    {
        if(coll.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            animate.SetBool("IsJumping", false);
        }
    }

    //updates isGrounded
    private void OnCollisionExit(Collision coll)
    {
        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        //bool runningForward = animate.GetBool(runningForwardHash);
        ////bool isJumping = animate.GetBool(name, true);
        //bool combat = animate.GetBool(combatHash);
        //bool death = animate.GetBool(deathHash);
        //bool takeDamage = animate.GetBool(takeDamageHash);
        //bool attack = animate.GetBool(attackHash);
        

        //get input from player 
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //player is moving
        if (direction.magnitude >= 0.1f) 
        {
            //set running animation to true
            animate.SetBool("IsMoving", true);
            

            //rotates the players body
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //check if player is on slope and ground
            if(OnSlope() && isGrounded)
            {
                moveDir = Vector3.ProjectOnPlane(moveDir, slopeHit.normal);
            }

            //moves the player 
            //Vector3 Velocity = moveDir.normalized * speed;
            Vector3 velocity = rb.velocity;

            //air control
            float control = isGrounded ? 1f : airMult;

            //velocity based on whether player is on ground or air 
            velocity.x = moveDir.x * speed * control;
            velocity.z = moveDir.z * speed * control;

            rb.velocity = velocity;

        //    if (!runningForward)
        //    {

        //       animate.SetBool("runningForward", true);
        //    }

        //}
        //else {
        //    //checks when to siwtch between the states 
        //    if (runningForward)
        //    {
        //        animate.SetBool("runningForward", false);
        //    }

        }

        //else not running update parameter to false
        else
        {
            animate.SetBool("IsMoving", false);
          
        }

        //if player hits jump button 
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if(isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //reset y
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                animate.SetBool("IsJumping", true);
            }
        }

    }

    private void SetSlopeSlideVelocity()
    {
        //use physics ray cast to check the ground under the player, only proceed if theres been a hit
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, 5))
        {
            //get slope angle 
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

            //check if its greater or equal to slope limit 
            //if(angle >= rb.slopeLimit)
        }
    }

    public bool OnSlope()
    {
        //checks the ground beneath the player 
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            //get how steep slope angle is
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            //return true if angle is less than max slope angle
            return angle < maxSlopeAngle && angle != 0;
           // {
               /// float ySpeed = rb.velocity.y;
                //if on slope set slope slide velocity
               // slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, ySpeed, 0), slopeHit.normal);
                //return;
            //}
           //return angle < maxSlopeAngle && angle != 0;
        }
        return false;

        //if character is on any ground steep enough to need a slide set slide velocity to 0 
        //slopeSlideVelocity = Vector3.zero;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal);
    }

    public bool CanAttack()
    {
        return horizontal == 0 && isGrounded;
    }
}