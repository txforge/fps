using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class character_movement : MonoBehaviour
{
	public float speed = 6f;
	public float gravity = -9.81f;
	public float groundDistance = 0.6f;
	public LayerMask groundMask;
	public float acceleration = 20f;
	public float deceleration = 25f;

	private Rigidbody rb;
	[SerializeField]
	private bool isGrounded;

	public Transform groundCheck; // Assegna in Inspector

    [Header("JUMP")]
	public float jumpHeight = 1.5f;
	public int maxJumpCount; // Numero di salti consentiti
	private int jumpCount = 0; // Contatore dei salti effettuati
	private float jumpBufferTime = 0.2f; // Tempo di buffer per il salto
	public Timer jumpBufferTimer;

	private bool wasGrounded = false; // aggiungi questa variabile


	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;

		jumpBufferTimer.duration = jumpBufferTime;
	}

	void Update()
	{
		// Transizione aria→terra: resetta i salti
		if (isGrounded && !wasGrounded)
			jumpCount = 0;

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

		// Input salto: attiva buffer
		if (Input.GetButtonDown("Jump"))
			jumpBufferTimer.StartTimer();

		// Salto da buffer SOLO se atterri mentre il buffer è attivo
		if (isGrounded && jumpBufferTimer.IsRunning())
		{
			if (jumpCount < maxJumpCount)
			{
				Jump();
				jumpBufferTimer.StopTimer();
			}
		}
		// Doppio/triplo salto in aria SOLO se premi il tasto
		else if (!isGrounded && Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
		{
			Jump();
		}

		wasGrounded = isGrounded;
	}
	void Jump()
	{
		rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity), rb.linearVelocity.z);
		jumpCount++;
	}
	void FixedUpdate()
	{
		if (!isGrounded)
		{
			rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
		}
	}

	private void OnCollisionEnter(Collision collision)
{
    if ((groundMask.value & (1 << collision.gameObject.layer)) != 0)
    {
        isGrounded = true;
    }
}

private void OnCollisionExit(Collision collision)
{
    if ((groundMask.value & (1 << collision.gameObject.layer)) != 0)
    {
        isGrounded = false;
    }
}
}