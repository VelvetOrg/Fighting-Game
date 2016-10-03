/* 
 * Owned by Velvet-Org. Copyright 2016 - 
 * This code is licenced under: Apache 2.0
 * Cameron Bell, Ruchir Bapat 
*/

using System.Collections;
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
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }


    public void ApplyRotation()
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
