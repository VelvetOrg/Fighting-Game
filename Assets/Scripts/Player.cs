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
public class Player : Entity
{
    //Temporary
    public Weapon startingWeapon; //Temporary, these will later be picked up
    float walkingSpeed;
    float normalSpeed;

    //Needs to communicate with the following scripts
    PlayerController playerController;
    PlayerInput playerInput;
    GunManager gunManager;
    MouseLook mouseLook;
    HeadBob headBobber;
    
    //Get components
    protected override void Start()
    {
        base.Start();

        //These will always work
        playerController = GetComponent<PlayerController>();
        playerInput      = GetComponent<PlayerInput>();

        //May possibly evaluate to null
        gunManager       = GetComponent<GunManager>();
        mouseLook        = GetComponentInChildren<MouseLook>();
        headBobber          = GetComponentInChildren<HeadBob>();

        //Attach the gun to the hand
        if(gunManager != null) gunManager.EquipWeapon(startingWeapon);

        normalSpeed = playerController.moveSpeed;
        walkingSpeed = normalSpeed * 0.1f;
    }

    //Take values from input class and turn them into calls
    void Update()
    {
        bool isShiftHeld = (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift));
        if (isShiftHeld) { playerController.moveSpeed = walkingSpeed; }
        if (playerController.moveSpeed != normalSpeed && !isShiftHeld) { playerController.moveSpeed = normalSpeed; }

        //Force player to move via the axis
        playerController.Move(playerInput.inputAxis);

        //Apply if availible
        if (headBobber != null) headBobber.Bob(playerController);
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
        if (gunManager.currentlyEquipt != null)
        {
            if (Input.GetMouseButton(0)) gunManager.currentlyEquipt.AttackHeld();
            if (Input.GetMouseButtonUp(0)) gunManager.currentlyEquipt.AttackReleased();
        }
        else { Debug.LogWarning("No gun is equipt to the player"); }
    }
}
