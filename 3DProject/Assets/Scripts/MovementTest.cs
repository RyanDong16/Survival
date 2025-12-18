using System.Collections;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    public Rigidbody rb;
    public Transform Camera;
    private Animator animate;
    public AudioSource runSound;

    private Vector3 moveDir;
    private RaycastHit slopeHit;

    public float speed = 6f;
    public float jumpForce = 5f;
    public float airMult = 0.4f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    float horizontal;
    float vertical;

    public bool isGrounded;
    public float groundCheckRadius = 0.3f;
    public float groundCheckOffset = 0.2f;
    public float maxSlopeAngle = 45f;

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip jumpSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animate = GetComponent<Animator>();
    }

    void Update()
    {
        GroundCheck();

        // Movement input
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        bool isMoving = direction.magnitude >= 0.1f;

        if (isMoving)
        {
            if (isGrounded && !runSound.isPlaying)
                runSound.Play();

            animate.SetBool("IsMoving", true);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            if (OnSlope())
                moveDir = Vector3.ProjectOnPlane(moveDir, slopeHit.normal);

            Vector3 velocity = rb.velocity;
            float control = isGrounded ? 1f : airMult;

            velocity.x = moveDir.x * speed * control;
            velocity.z = moveDir.z * speed * control;

            rb.velocity = velocity;
        }
        else
        {
            if (runSound.isPlaying)
                runSound.Stop();

            animate.SetBool("IsMoving", false);
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jump triggered. isGrounded = " + isGrounded);
            Debug.Log("Y BEFORE jump = " + rb.velocity.y);
            //SoundManager.Instance.PlaySound(jumpSound);

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Y AFTER jump = " + rb.velocity.y);
        }
    }

    // -------------------------
    // Ground Check (CheckSphere)
    // -------------------------
    private void GroundCheck()
    {
        Vector3 spherePos = transform.position + Vector3.down * groundCheckOffset;

        isGrounded = Physics.CheckSphere(
            spherePos,
            groundCheckRadius,
            ~0,
            QueryTriggerInteraction.Ignore
        );

        Debug.DrawRay(transform.position, Vector3.down * groundCheckOffset, Color.green);
    }

    // -------------------------
    // Slope Detection
    // -------------------------
    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 1.5f))
        {
            float angle = Vector3.Angle(slopeHit.normal, Vector3.up);
            return angle > 0 && angle <= maxSlopeAngle;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        SoundManager.Instance.PlaySound(deathSound);
        yield return new WaitForSeconds(deathSound.length);
    }
}
