/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
*/

using UnityEngine;
using System.Collections;

//Needs the following
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    //Temporary
    public Weapon startingWeapon; //Temporary, these will later be picked up
    
    //Needs to communicate with the following scripts
    PlayerController playerController;
    PlayerInput playerInput;
    ArmManager armManager;
    MouseLook mouseLook;
    HeadBob headBob;

    //Holds the currently attached weapon 
    Weapon equipt;

    //Get components
    void Start()
    {
        //These will always work
        playerController = GetComponent<PlayerController>();
        playerInput      = GetComponent<PlayerInput>();

        //May possibly evaluate to null
        armManager       = GetComponent<ArmManager>();
        mouseLook        = GetComponentInChildren<MouseLook>();
        headBob          = GetComponentInChildren<HeadBob>();
        
        //Attach the gun to the hand
        if(armManager != null) equipt = armManager.EquiptWeapon(startingWeapon);
    }
    
    //Take values from input class and turn them into calls
   	void Update ()
    {
        //Force player to move via the axis
        playerController.Move(playerInput.inputAxis);

        //Apply if availible
        if (headBob != null) headBob.Bob(playerController);
        else { Debug.LogWarning("No head bob script on camera"); }

        //Apply mouse look
        if (mouseLook != null)
        {
            Vector3 rotation = mouseLook.look(playerInput.mousePos);

            //Actually rotate the player and camera
            mouseLook.transform.rotation = Quaternion.Euler(new Vector3(rotation.x, rotation.y, mouseLook.transform.rotation.eulerAngles.z));
            playerController.transform.rotation = Quaternion.Euler(new Vector3(playerController.transform.rotation.x, rotation.y, playerController.transform.rotation.eulerAngles.z));
        }
        else { Debug.LogWarning("No mouselook script found"); }

        //This is handled by player input
        if (equipt != null)
        {
            if (Input.GetMouseButton(0)) equipt.AttackHeld();
            if (Input.GetMouseButtonUp(0)) equipt.AttackReleased();
        }
        else { Debug.LogWarning("No gun is equipt to the player"); }
    }
}
