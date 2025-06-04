using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class character_movement : MonoBehaviour
{
	public float speed = 6f;
	public float gravity = -9.81f;
	public float jumpHeight = 1.5f;
	public float groundDistance = 0.6f;
	public LayerMask groundMask;
	public float acceleration = 20f;
	public float deceleration = 25f;

	private Rigidbody rb;
	[SerializeField]
	private bool isGrounded;

	public Transform groundCheck; // Assegna in Inspector


	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
	}

	void Update()
	{
		RaycastHit hitSphere;
		isGrounded = Physics.SphereCast(
			transform.position,
			0.5f, // Raggio della sfera
			Vector3.down,
			out hitSphere,
			groundDistance,
			groundMask
		); 

		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 inputDir = new Vector3(x, 0, z);
		if (inputDir.magnitude > 1f)
			inputDir = inputDir.normalized;

		Vector3 moveDir = transform.right * inputDir.x + transform.forward * inputDir.z;

		Vector3 targetVelocity = moveDir * speed;
		float accelRate = (inputDir.magnitude > 0.1f) ? acceleration : deceleration;

		Vector3 velocity = rb.linearVelocity;
		Vector3 velocityChange = Vector3.MoveTowards(
			new Vector3(velocity.x, 0, velocity.z),
			new Vector3(targetVelocity.x, 0, targetVelocity.z),
			accelRate * Time.deltaTime
		);

		if ((velocityChange - new Vector3(targetVelocity.x, 0, targetVelocity.z)).sqrMagnitude < 0.01f)
		{
			velocityChange = new Vector3(targetVelocity.x, 0, targetVelocity.z);
		}

		rb.linearVelocity = new Vector3(velocityChange.x, velocity.y, velocityChange.z);

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity), rb.linearVelocity.z);
		}
	}
	void FixedUpdate()
	{
		if (!isGrounded)
		{
			rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
		}
	}
}