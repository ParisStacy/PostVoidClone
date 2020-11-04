using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField]
    float sensitivity;

    Vector2 mouseLook;
    PlayerMove controller;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = transform.parent.parent.GetComponent<PlayerMove>();
    }

    void Update()
    {
        //MouseLook
        mouseLook += new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseLook *= sensitivity;

        mouseLook.y = Mathf.Clamp(mouseLook.y, -85, 85);
        transform.localEulerAngles = new Vector3(-mouseLook.y, 0, 0);
        controller.transform.localEulerAngles = new Vector3(0, mouseLook.x, 0);
    }
}
