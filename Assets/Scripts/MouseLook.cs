/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
*/

using UnityEngine;
using System.Collections;

//Smooths movement from the mouse
[RequireComponent(typeof(Camera))]
public class MouseLook : MonoBehaviour
{
    //Public values
    public float sensitivity = 3.0f; //Multiplier
    public float smoothing = 2.0f; //Lerping

    //Locking of the cursor
    public CursorLockMode state;

    //Needed for calcualting the rotation
    Vector2 rotation;
    Vector2 currentRot;
    Vector2 rotationVelocity;

    //Sometimes the user want the cursor to lock to the center of the screen
    void Start() { Cursor.lockState = state; }

    //Used by the player script when calcullating gun rotation
    public float getTargetXRotation() { return rotation.x; }
    public float getTargetYRotation() { return rotation.y; }

    //Called by the player input class
    public Vector3 look(Vector2 mouse)
    {
        //Apply the rotation
        rotation.x -= mouse.y * sensitivity;
        rotation.y += mouse.x * sensitivity;

        //Limit looking upwards
        //Hard code the values
        rotation.x = Mathf.Clamp(rotation.x, -85.0f, 85.0f);

        //Apply smoothing
        //Use smooth damp because it will use the velocity of the mouse
        currentRot.x = Mathf.SmoothDamp(currentRot.x, rotation.x, ref rotationVelocity.x, smoothing * Time.deltaTime);
        currentRot.y = Mathf.SmoothDamp(currentRot.y, rotation.y, ref rotationVelocity.y, smoothing * Time.deltaTime);

        //Apply the rotation
        //This should later be converted to a quaternion
        return new Vector3(currentRot.x, currentRot.y, 0.0f);
    }
}