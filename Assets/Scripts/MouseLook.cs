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
        rotation.x = Mathf.Clamp(rotation.x, -80.0f, 80.0f);

        //Apply smoothing
        //Use smooth damp because it will use the velocity of the mouse
        currentRot.x = Mathf.SmoothDamp(currentRot.x, rotation.x, ref rotationVelocity.x, smoothing * Time.deltaTime);
        currentRot.y = Mathf.SmoothDamp(currentRot.y, rotation.y, ref rotationVelocity.y, smoothing * Time.deltaTime);

        //Apply the rotation
        //This should later be converted to a quaternion
        return new Vector3(currentRot.x, currentRot.y, 0.0f);
    }
}

// --- OLD ---
/*
 * using System.Collections;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Vector2 rotationSensitivity = new Vector2(2f, 2f);
    public float minimumRotationY = -90F;
    public float maximumRotationY = 90F;
    public bool smooth;
    public float smoothTime = 5f;

    public bool hideCursor;

    private Quaternion playerTargetRotation;
    private Quaternion cameraTargetRotation;

    private GameObject player;
    private GameObject camera;


    void Start()
    {
        player = GetComponentInParent<Player>().gameObject;
        camera = GetComponent<Camera>().gameObject;
        playerTargetRotation = player.transform.localRotation;
        cameraTargetRotation = camera.transform.localRotation;

        if (hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
    }


    public void Look()
    {
        playerTargetRotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSensitivity.x, Vector3.up);
        cameraTargetRotation *= Quaternion.AngleAxis((Input.GetAxis("Mouse Y") * rotationSensitivity.y), Vector3.left);

        cameraTargetRotation = ClampRotation(cameraTargetRotation);

        if (smooth)
        {
            player.transform.localRotation = Quaternion.Slerp(player.transform.localRotation, playerTargetRotation, smoothTime * Time.deltaTime);
            camera.transform.localRotation = Quaternion.Slerp(camera.transform.localRotation, cameraTargetRotation, smoothTime * Time.deltaTime);
        }
        else
            player.transform.localRotation = playerTargetRotation; camera.transform.localRotation = cameraTargetRotation;

    }

    //From Unity's MouseLook
    Quaternion ClampRotation(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, minimumRotationY, maximumRotationY);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
*/
