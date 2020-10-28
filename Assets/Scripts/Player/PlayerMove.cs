using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed, crouchSpeed;

    float _currentSpeed;
    bool crouched;

    Vector2 _moveInput;
    Vector3 _moveDirection;

    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Inputs
        _moveInput.x = Input.GetAxis("Horizontal");
        _moveInput.y = Input.GetAxis("Vertical");

        _currentSpeed = (crouched) ? crouchSpeed : moveSpeed;

    }

    void FixedUpdate() {
        //Apply Movement
        _moveDirection = (_moveInput.x * transform.right + _moveInput.y * transform.forward).normalized;
        _moveDirection *= _currentSpeed;

        cc.Move(_moveDirection * Time.fixedDeltaTime);
    }
}
