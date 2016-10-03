using UnityEngine;
using System.Collections;

//Needs the following
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    //Needs to communicate with the following scripts
    PlayerController playerController;
    PlayerInput playerInput;
    MouseLook mouseLook;
    HeadBob headBob;

    public Weapon equipt;

    //Get components
    void Start()
    {
        //These will always work
        playerController = GetComponent<PlayerController>();
        playerInput      = GetComponent<PlayerInput>();

        //May possibly evaluate to null
        mouseLook        = GetComponentInChildren<MouseLook>();
       
        headBob          = GetComponentInChildren<HeadBob>();

        //Temp
        if (equipt != null) equipt = GetComponentInChildren<Weapon>();
    }
    
    //Take values from input class and turn them into calls
   	void Update ()
    {
        //Force player to move via the axis
        playerController.Move(playerInput.inputAxis);
        mouseLook.Rotate();   
        //Apply if availible
        if (headBob != null) headBob.Bob(playerController);
      
        //This is handled by player input
        if(equipt != null)
        {
            if (Input.GetMouseButton(0))   equipt.AttackHeld();
            if (Input.GetMouseButtonUp(0)) equipt.AttackReleased();
        }
    }
}
