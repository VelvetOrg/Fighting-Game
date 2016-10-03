using System.Collections;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;
    public bool clampVerticalRotation = true;
    public float minimumRotationY = -90F;
    public float maximumRotationY = 90F;
    public bool smooth;
    public float smoothTime = 5f;

    public bool lockAndHideCursor;

    private Quaternion playerTargetRotation;
    private Quaternion cameraTargetRot;

    private Transform playerTransform;
    private Transform cameraTransform;


    void Start()
    {
        playerTransform = GetComponentInParent<Player>().transform;
        cameraTransform = GetComponent<Camera>().transform;
        playerTargetRotation = playerTransform.localRotation;
        cameraTargetRot = cameraTransform.localRotation;
        if (lockAndHideCursor)
        {
             Cursor.lockState = CursorLockMode.None;
             Cursor.visible = true;
        }
    }


    public void Rotate()
    {
        float yRot = Input.GetAxis("Mouse X") * sensitivityX;
        float xRot = Input.GetAxis("Mouse Y") * sensitivityY;

        playerTargetRotation *= Quaternion.Euler(0f, yRot, 0f);
        cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

        
        cameraTargetRot = Adjust(cameraTargetRot);

        playerTransform.localRotation = Quaternion.Slerp(playerTransform.localRotation, playerTargetRotation, smoothTime * Time.deltaTime);
        cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, cameraTargetRot, smoothTime * Time.deltaTime);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }


    Quaternion Adjust(Quaternion q)
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