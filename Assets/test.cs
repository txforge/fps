using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class test : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float slopeLimit = 45f; // In gradi
    public float slideForce = 10f;
    public Transform groundCheck;
    public float groundCheckDistance = 0.5f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 groundNormal;

    [Header("Slope Handling")]
    public float slopeLimitAngle = 45f; // Angolo massimo per considerare una superficie inclinata
    private RaycastHit slopeHit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Non si ribalta
    }

    void FixedUpdate()
    {
        // --- Controllo se è a terra
        RaycastHit hit;
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, out hit, groundCheckDistance, groundLayer);

        if (isGrounded)
        {
            groundNormal = hit.normal;
        }
        else
        {
            groundNormal = Vector3.up;
        }

        // --- Movimento base
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(x, 0, z).normalized;
        Vector3 moveDirection = Quaternion.FromToRotation(Vector3.up, groundNormal) * inputDir;
        moveDirection *= moveSpeed;

        // --- Movimento applicato
        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveDirection.x;
        velocity.z = moveDirection.z;
        rb.linearVelocity = velocity;

        // --- Scivolamento se la pendenza è troppo alta
        float slopeAngle = Vector3.Angle(groundNormal, Vector3.up);

        if (isGrounded && slopeAngle > slopeLimit)
        {
            Vector3 slideDir = new Vector3(groundNormal.x, -groundNormal.y, groundNormal.z);
            rb.AddForce(slideDir * slideForce, ForceMode.Acceleration);
        }
    }
}