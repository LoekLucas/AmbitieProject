using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform player; // Asking for object to take position, rotation and scale from
    private float xMouse;
    private float yMouse;
    private float xRotation;
    public float speed = 100f;



    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks mouse position when playing
    }

    // Update is called once per frame
    void Update()
    {
        // Records mouse movement and makes camera rotation linear and not dependant on frames
        xMouse = Input.GetAxis("Mouse X") * speed * Time.deltaTime;
        yMouse = Input.GetAxis("Mouse Y") * speed * Time.deltaTime;
        xRotation -= yMouse; // Translates mouse movement to camera rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ?
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // ?
        player.Rotate(Vector3.up * xMouse); // Rotates player with mouse movement
    }
}
