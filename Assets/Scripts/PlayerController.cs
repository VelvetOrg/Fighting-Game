using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

public class PlayerController : MonoBehaviour {

	public float movementFactor = 10.0f;
	public float maximumVelocity = 10.0f;
	public bool canJump = true;
	public float jumpHeight = 2.0f;
	private bool isGrounded = true;
	Rigidbody playerRigidbody;

	void Awake () {
		playerRigidbody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		if (isGrounded) {
			Vector3 finalVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			finalVelocity = transform.TransformDirection(finalVelocity);
			finalVelocity *= movementFactor;

			Vector3 currentVelocity = GetComponent<Rigidbody>().velocity;
			Vector3 velocityChange = (finalVelocity - currentVelocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maximumVelocity, maximumVelocity);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maximumVelocity, maximumVelocity);
			velocityChange.y = 0;
			playerRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
			//playerRigidbody.MovePosition(velocityChange + transform.position * movementFactor);
			// Jump
			if (canJump && Input.GetKeyDown(KeyCode.Space)) {
				playerRigidbody.velocity = new Vector3(currentVelocity.x, Mathf.Sqrt(2 * jumpHeight * 10), currentVelocity.z);
			}
		}

		// We apply gravity manually for more tuning control
		playerRigidbody.AddForce(new Vector3 (0, -10 * GetComponent<Rigidbody>().mass, 0));
		//playerRigidbody.MovePosition(transform.position + new Vector3 (0, -10 * playerRigidbody.mass, 0) * movementFactor);

		isGrounded = false;
	}

	void OnCollisionStay () {
		isGrounded = true;    
	}
}