using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform cam;
    public PlayerController playerController;

    [Header("Camera Rotation")]
    public float XSensitivity = 150.0f;
    public float YSensitivity = 150.0f;
    private float XRotation = 0.0f;
    private float YRotation = 0.0f;

    void Start()
    {
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * XSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * YSensitivity;

        XRotation -= mouseY;
        YRotation += mouseX;

        XRotation = Mathf.Clamp(XRotation, -90f, 90f);

        //handle crouching
        transform.position = new Vector3(player.position.x, (player.position.y + (float)0.5) - playerController.crouchOffset, player.position.z);

        //now rotate camera on X axis in order to look up, down
        cam.localRotation = Quaternion.Euler(XRotation, 0, 0);

        //set camera holder same rotation as player
        transform.rotation = player.rotation;

        player.rotation = Quaternion.Euler(0, YRotation * YSensitivity * Time.fixedDeltaTime, 0);
    }
}
