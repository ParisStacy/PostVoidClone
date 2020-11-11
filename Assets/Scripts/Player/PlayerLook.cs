using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField]
    float sensitivity;

    float recoil;

    Vector2 mouseLook;
    PlayerMove controller;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = transform.parent.parent.GetComponent<PlayerMove>();
    }

    void Update()
    {
        recoil -= .05f;
        recoil = Mathf.Clamp(recoil, 0, 9);

        //MouseLook
        mouseLook += new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseLook *= sensitivity;

        mouseLook.y = Mathf.Clamp(mouseLook.y, -85, 85);
        transform.localEulerAngles = new Vector3(-mouseLook.y - recoil, 0, 0);
        controller.transform.localEulerAngles = new Vector3(0, mouseLook.x, 0);
    }

    public void Recoil(float degree) {
        recoil += degree;
    }
}
