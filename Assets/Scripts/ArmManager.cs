/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
*/

using UnityEngine;
using System.Collections;

//This class handles postion of the arm so that it will always point to where the user want to fire
//It also handles equipping of weapons
//Not finished yet
public class ArmManager : MonoBehaviour
{
    //Publics
    public float armFollowSpeed = 10; //Speed at which the arm lerps to the mosue
    public Vector3 armOffset; //Where on the screen is the arm
    public Transform hand; //The transform that attaches to the gun

    [Range(0, 1)]
    public float bobEffect = 0.5f; //How much should the gug bob
    
    //Privates
    Transform arm; //Stores the parent of the hand

    float targetVerticalRotation;
    float targetHorizontalRotation;

    //Will set privates
    void Start()
    {
        if (hand != null)
        {
            //Assumes the tree structure of the heirachy is:
            /* Arm
             * | -- Mesh
             * | -- Hand 
             * |     | -- Weapon
             */

            arm = hand.parent;
        }
    }

    //Will give the player a weapon
    public Weapon EquiptWeapon(Weapon w)
    {
        //Just make sure that the publics have been set
        if (hand != null)
        {
            //There is already a gun, remove it
            //Check that gun isnt already added
            //Remove
            if (hand.childCount > 0) { Destroy(hand.GetChild(0).gameObject); Debug.LogWarning("Removed weapon"); }

            //Add the gun prefab
            GameObject instanciatedWeapon = Instantiate(w.gameObject, hand, false) as GameObject;
            instanciatedWeapon.name = "Weapon";

            //Set
            return instanciatedWeapon.GetComponent<Weapon>();
        }
        else { Debug.LogWarning("No hand on the character"); return null; }
    }

    //Will update the position of the arm
    public void UpdateArm(MouseLook mouseLook)
    {
        //Update the arms position
        if (arm != null)
        {
            //Position
            Vector3 targetPos = mouseLook.transform.position;

            //Remove head bob
            //Slow for now
            HeadBob bob = mouseLook.gameObject.GetComponent<HeadBob>();
            if (bob != null) targetPos -= bob.getBobAmount() * bobEffect;

            //Since position is dependant on rotation
            arm.transform.position = targetPos + (Quaternion.Euler(0, targetVerticalRotation, 0) * armOffset);

            //Lerp the rotation
            targetHorizontalRotation = mouseLook.getTargetXRotation();
            targetVerticalRotation = mouseLook.getTargetYRotation();

            //targetHorizontalRotation = Mathf.Lerp(targetHorizontalRotation, mouseLook.getTargetXRotation(), Time.deltaTime * armFollowSpeed);
            //targetVerticalRotation = Mathf.Lerp(targetVerticalRotation, mouseLook.getTargetYRotation(), Time.deltaTime * armFollowSpeed);

            arm.transform.rotation = Quaternion.Lerp(arm.transform.rotation, Quaternion.Euler(targetHorizontalRotation, targetVerticalRotation, 0), Time.deltaTime * armFollowSpeed);
        }
    }
}