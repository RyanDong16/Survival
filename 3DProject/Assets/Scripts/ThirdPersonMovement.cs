using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThirdPersonMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform Camera;
    private Animator animate;
    public AudioSource runSound;

    private Vector3 moveDir;

    public float speed = 6f;
    public float jumpForce = 5f;
    public float airMult = 0.4f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float horizontal;
    float vertical;

    public bool isGrounded;

    public float playerHeight;
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip jumpSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animate = GetComponent<Animator>();
    }

    private void OnCollisionStay(Collision coll)
    {
        if (coll.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }

    private void OnCollisionExit(Collision coll)
    {
        isGrounded = false;
    }

    private IEnumerator DeathCoroutine()
    {
        SoundManager.Instance.PlaySound(deathSound);
        yield return new WaitForSeconds(deathSound.length);
        // Reload scene or menu here
    }

    void Update()
    {
        // -------------------------
        // INPUT
        // -------------------------
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        bool isMoving = direction.magnitude >= 0.1f;

        // -------------------------
        // ROTATION
        // -------------------------
        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        else
        {
            moveDir = Vector3.zero;   // ← prevents sliding after landing
        }

        // -------------------------
        // MOVEMENT (ground + air)
        // -------------------------
        Vector3 velocity = rb.velocity;
        float control = isGrounded ? 1f : airMult;

        velocity.x = moveDir.x * speed * control;
        velocity.z = moveDir.z * speed * control;

        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        // Extra friction when grounded and not moving
        if (isGrounded && !isMoving)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        // -------------------------
        // ANIMATIONS + FOOTSTEPS
        // -------------------------
        if (isMoving && isGrounded)
        {
            animate.SetBool("IsMoving", true);
            if (!runSound.isPlaying) runSound.Play();
        }
        else
        {
            animate.SetBool("IsMoving", false);
            if (runSound.isPlaying) runSound.Stop();
        }

        // -------------------------
        // JUMP
        // -------------------------
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal);
    }

    public bool CanAttack()
    {
        return horizontal == 0 && isGrounded;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            StartCoroutine(DeathCoroutine());
        }
    }
}
