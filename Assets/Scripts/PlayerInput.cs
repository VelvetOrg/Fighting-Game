/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
 */

using UnityEngine;
using System.Collections;

//This class allows any controller based functions to get user input
//Because it is a wrapper, the code is versitile
[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour 
{
    //Input
    Vector2 inputAxis;
    Vector2 mousePos;

    //Objects
    PlayerController attached;
    MouseLook cam;
    HeadBob bob;

	//Get
	void Awake()
	{
		//Initialze values
        mousePos = Vector3.zero;
		inputAxis = Vector2.zero;

        bob = GetComponentInChildren<HeadBob>();
        cam = GetComponentInChildren<MouseLook>();
		attached = GetComponent<PlayerController>();
	}
	
	//Later events may be used
	void Update() 
	{
		//Get the basic unity input axis
		int horizontal = (int)Input.GetAxisRaw("Horizontal");
		int vertical = (int)Input.GetAxisRaw("Vertical");

		//What should the multi key affect be
		float mul = 1.0f;

        //Are two keys down
        // 1.0 / (sqrt(2.0))
        if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) == 2) { mul = 0.707106f; }

		//Pack into a vec 2
		inputAxis = new Vector2(horizontal, vertical) * mul;

		//Force player to move via the axis
		attached.Move(inputAxis);

        //Actually use some head bobbing
        if(bob != null) bob.Bob(attached);

        //Get mouse position
        mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 rotation = cam.look(mousePos);

        //Actually roate the player and camera
        cam.transform.rotation =      Quaternion.Euler(new Vector3(rotation.x,                    rotation.y, cam.transform.rotation.eulerAngles.z));
        attached.transform.rotation = Quaternion.Euler(new Vector3(attached.transform.rotation.x, rotation.y, attached.transform.rotation.eulerAngles.z));
    }
}
