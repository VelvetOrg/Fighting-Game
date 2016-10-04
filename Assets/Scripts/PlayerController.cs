/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
 */

using UnityEngine;
using System.Collections;

//Works on a physics based system
[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour
{
    //Since character controller do not have physics:
    [Header("Physics")]
    public float pushStrength = 3.0f;
    public float gravity = 20.0f;

    //Collision proerties
    [Header("Collision")]
    public float groundOffset = 0.3f; //Stay off the ground slightly
    public float antiJitter = 3.0f; //Fix collider rotating randomly

    //Movement properties
    [Header("Movement")]
    public float accelerateSpeed = 10.0f; //Lerping amount
    public float slideSpeed = 12.0f; //Move down a slope
    public float airControl = 0.6f; //Limit movement in the air
    public float moveSpeed = 8.0f; //Units to move in one second
    public float jump = 10.0f; //Jump force - in units

    //Holds the fake collider
    Rigidbody childCol;

	//Privates
	CharacterController body; //Holds information on events
    Vector3 direction; //Move dir
    Vector3 contact; //When checking for collision
    RaycastHit hit; //Generaic value
    Vector2 axis; //Smoothed input axis

    float rayDistance; //Height of collider
    float slideLimit; //Angle to slide from
    bool grounded; //Collider

    //For other classes
    public bool isGrounded() { return grounded; } 

    //Set variables
    void Awake ()
    {
        //Get controller
        body = GetComponent<CharacterController> ();

        //Ray distance should be how far down is the bottom of the collider
        //Assumes transform is in the center
        //Will be used later
        rayDistance = (body.height / 2) + body.radius + groundOffset;

        //Find character controller slope angle
        slideLimit = body.slopeLimit - 0.1f;

        //Find
        if(childCol == null) { childCol = GetComponentInChildren<Rigidbody>(); }
    }

    //Called by the player input class
    public void Move(Vector2 a) 
	{
        //Lerp the input axis
        axis = Vector2.Lerp(axis, a, Time.deltaTime * accelerateSpeed); 

		//Move when grounded
        if(grounded)
        {
            //Guess that player is not sliding
            bool sliding = false;

            //Check if the player should be sliding
            //Raycast down from CharacterControllerHit point
            Physics.Raycast(contact + new Vector3(0, 1, 0), -Vector3.up, out hit);

            //Cheak if "hit" is at the right angle to slide down
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            if (angle > slideLimit && angle < 85) { sliding = true; }

            //Only when not sliding does player have control
            if (!sliding)
            {
                //Direction holds the force to apply to the controller
                //Convert to local space - so it will move relative to the mouse look script (once done)
                direction = transform.TransformDirection(new Vector3(axis.x, -antiJitter, axis.y)) * moveSpeed;
            }
            else
            {
                //Move at 90 degrees to the slope angle
                Vector3 norm = hit.normal;
                direction = new Vector3(norm.x, -norm.y, norm.z);
                Vector3.OrthoNormalize(ref norm, ref direction);

                //Scalar
                direction *= slideSpeed;
            }

            //Jump - apply firce to the direction vector
            if (Input.GetButton("Jump")) { direction.y = jump; }
        }
        else
        {
            //Still let player move but limit it
            //Because they are in the air
            //Alos convert to local space
            direction = transform.TransformDirection(new Vector3(axis.x * moveSpeed * airControl, direction.y, axis.y * moveSpeed * airControl));
        }

        //Apply gavity
        direction.y -= gravity * Time.deltaTime;

        //Move
        //Body returns the collision types
        //Convert that to a grounded stat
        //https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
        CollisionFlags g = body.Move(direction * Time.deltaTime);
        grounded = (g & CollisionFlags.Below) != 0;

        //Fake physics
        if(childCol != null) childCol.velocity = direction * Time.deltaTime;
	}

    //Has a collision hit occured
    void OnControllerColliderHit(ControllerColliderHit h) { contact = h.point; }
}