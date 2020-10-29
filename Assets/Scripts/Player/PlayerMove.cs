using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //Configurable Variables
    [SerializeField]
    private float moveSpeed, crouchSpeed, jumpForce, gravity;

    //Backing Variables
    float _currentSpeed, _vMovement, _cameraZ;
    bool crouched, jumping, grounded, overHead;

    //Vectors
    Vector2 _moveInput;
    Vector3 _moveDirection;

    //Components
    CharacterController cc;
    GameObject camera;

    void Start()
    {
        //Initialize Components
        cc = GetComponent<CharacterController>();
        camera = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        //Inputs
        _moveInput.x = Input.GetAxis("Horizontal");
        _moveInput.y = Input.GetAxis("Vertical");

        _currentSpeed = (crouched) ? crouchSpeed : moveSpeed;

        jumping = Input.GetKeyDown(KeyCode.Space);

        //Check Grounded / Above
        checkGrounded();
        checkAbove();
        cameraEffects();

        //Configure Movement Vector
        _moveDirection = (_moveInput.x * transform.right + _moveInput.y * transform.forward).normalized;
        _moveDirection *= _currentSpeed;

        //Configure Vertical Movement
        if (!grounded) _vMovement -= gravity * Time.deltaTime;
        else _vMovement = 0;

        if (grounded && jumping) _vMovement += jumpForce;
        if (overHead) _vMovement = -1;

        //Apply movement
        cc.Move(_moveDirection * Time.deltaTime);
        cc.Move(new Vector3(0, _vMovement, 0) * Time.deltaTime);

    }

    //Check if the player is on the ground
    void checkGrounded() {
        RaycastHit hit;
        grounded = Physics.SphereCast(transform.position, .2f, Vector3.down, out hit, .9f);
    }

    //Check if there is anything above the player
    void checkAbove() {
        RaycastHit hit;
        overHead = Physics.SphereCast(transform.position, .3f, Vector3.up, out hit, .8f);
    }

    //Apply camera effects
    void cameraEffects() {

        //Camera Lean on Move
        float _targetZ;
        _targetZ = (_moveInput.x != 0) ? -3 * _moveInput.x : 0;
        _cameraZ = Mathf.Lerp(_cameraZ, _targetZ, .05f);

        camera.transform.localEulerAngles = new Vector3(camera.transform.rotation.x, 0, _cameraZ);


    }
}
