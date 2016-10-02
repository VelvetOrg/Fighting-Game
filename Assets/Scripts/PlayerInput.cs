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
    public Vector2 inputAxis { get; private set; }
    public Vector2 mousePos { get; private set; }

    //Holds fire input state
    public enum FireState { Down, Held, Released, None };
    public FireState fireState { get; private set; }
    
	//Get
	void Awake()
	{
		//Initialze values
        mousePos = Vector3.zero;
		inputAxis = Vector2.zero;
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

        //Get mouse position
        mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //Set the fire state
        //if (Input.GetMouseButtonDown(0))    fireState = FireState.Down;
        //else if (Input.GetMouseButtonUp(0)) fireState = FireState.Released;
        //else if (Input.GetMouseButton(0))   fireState = FireState.Held;
        //else                                fireState = FireState.None;
    }
}
