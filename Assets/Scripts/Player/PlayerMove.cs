﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour, IDamage<int>
{
    //Configurable Variables
    [SerializeField]
    private float moveSpeed, jumpForce, gravity;

    //Backing Variables
    float _vMovement, _cameraZ, _startingSlideSpeed, _slideDecay, _decayRate, _groundedRayLength;
    int health = 100;
    bool crouched, jumping, grounded, overHead, sliding;

    //Vectors
    Vector2 _moveInput;
    Vector3 _moveDirection, _slideDirection;

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

        jumping = Input.GetKeyDown(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            sliding = true;
            _slideDecay = 1.2f;
            _decayRate = .0008f;
            _slideDirection = transform.forward * moveSpeed;
            _startingSlideSpeed = (_moveInput != Vector2.zero) ? moveSpeed : 0;
        }

        //Determine Controller Height
        cc.height = (sliding) ? .7f : 2;
        _groundedRayLength = (sliding) ? .1f : .9f;

        //Check Grounded / Above
        checkGrounded();
        checkAbove();
        cameraEffects();


        //Configure Movement Vector
        _moveDirection = (_moveInput.x * transform.right + _moveInput.y * transform.forward).normalized;
        _moveDirection *= moveSpeed;

        //Configure Vertical Movement
        if (!grounded) _vMovement -= gravity * Time.deltaTime;
        else _vMovement = 0;

        if (grounded && jumping && !sliding) _vMovement += jumpForce;
        if (overHead) _vMovement = -1;

        //Slide
        if (sliding) slide();

        //Apply movement
        if (!sliding) cc.Move(_moveDirection * Time.deltaTime);
        cc.Move(new Vector3(0, _vMovement, 0) * Time.deltaTime);

        
    }

    //Check if the player is on the ground
    void checkGrounded() {
        RaycastHit hit;
        grounded = Physics.SphereCast(transform.position, .2f, Vector3.down, out hit, _groundedRayLength);
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
        _targetZ = (_moveInput.x != 0) ? -2 * _moveInput.x : 0;
        _cameraZ = Mathf.Lerp(_cameraZ, _targetZ, .1f);

        camera.transform.localEulerAngles = new Vector3(0, 0, _cameraZ);


    }

    //Slide Player
    void slide() {

        //Slide
        _slideDirection = _slideDirection.normalized * _startingSlideSpeed * _slideDecay;
        cc.Move(_slideDirection * Time.deltaTime);
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            sliding = false;
            cc.Move(new Vector3(0, .7f, 0));
        }

        //Decay Slide
        _slideDecay -= _decayRate;
        _slideDecay = Mathf.Clamp(_slideDecay, 0, 1.5f);
        _decayRate += .000001f;
    }

    public void Damage(int damageTaken) {
        health -= damageTaken;
        Debug.Log(health);
    }
}