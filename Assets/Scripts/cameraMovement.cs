using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public Vector2 cameraRotation;
    public float sensitivity = 0.5f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        cameraRotation.x += Input.GetAxis("Mouse X")*sensitivity;
        cameraRotation.y += Input.GetAxis("Mouse Y")*sensitivity;
        transform.localRotation = Quaternion.Euler(-cameraRotation.y, -cameraRotation.x, 0);
    }
}
