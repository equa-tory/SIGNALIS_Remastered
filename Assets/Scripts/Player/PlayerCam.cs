//by TheSuspect
//06.04.2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 250f;
    public float sensY = 250f;

    public Transform model;

    private float xRotation;
    private float yRotation;

    public float rotationClamp = 90;

    public bool showCursor;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {

        if (showCursor) return;

        MouseInput();

    }

    void MouseInput()
    {
        //mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -rotationClamp, rotationClamp);

        //rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        model.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
