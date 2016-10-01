using UnityEngine;
using System.Collections;

//Works on a physics based system
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
public class PlayerController : MonoBehaviour {

	//Movement properties
	public float moveSpeed = 10.0f;

	//Needs to affect the rigidbody
	Rigidbody body;

	void Awake () { body = GetComponent<Rigidbody> (); }

	//Called by the player input class
	public void Move(Vector2 axis) 
	{
		//Move smoothly
		Vector2 moveAmount = axis * moveSpeed;

		//Apply to the body
		body.velocity = new Vector3(moveAmount.x, body.velocity.y, moveAmount.y);
	}
}