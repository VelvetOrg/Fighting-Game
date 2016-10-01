using UnityEngine;
using System.Collections;

//This class allows any controller based functions to get user input
//Because it is a wrapper, the code is versitile
[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour 
{
	//Input axis will be normalized for multi keys
	Vector2 inputAxis; //{ public get; private set; }
	PlayerController attached;

	//Get
	void Awake()
	{
		//Initialze values
		inputAxis = Vector2.zero;
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
	}
}
