using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour, IDamage<int>
{
    //Configurable Variables
    [SerializeField]
    private float moveSpeed, jumpForce, gravity, maxHealth;

    //Backing Variables
    float _vMovement, _cameraZ, _startingSlideSpeed, _slideDecay, _decayRate, _groundedRayLength, _health, _reloadTimer, _damageEffectTimer;
    int _bulletsLeft, _maxAmmo = 6;
    bool crouched, jumping, grounded, overHead, sliding;

    //Vectors
    Vector2 _moveInput;
    Vector3 _moveDirection, _slideDirection, _leftHandOrigin, _rightHandOrigin;

    //Components
    CharacterController cc;
    GameObject camera;
    Animator rightHandAnimator;
    [SerializeField]
    GameObject idolLiquid;
    [SerializeField]
    GameObject leftHand;
    [SerializeField]
    GameObject rightHand;
    [SerializeField]
    GameObject slideEffect;
    [SerializeField]
    GameObject damageEffect;
    [SerializeField]
    GameObject EnemyPrefab;
    [SerializeField]
    GameObject GunMag;
    [SerializeField]
    PlayerFire PlayerFire;
    [SerializeField]
    Animator muzzleFlash;
    [SerializeField]
    Animator crosshairAnimator;

    //Sounds
    [SerializeField]
    AudioSource gunSource, playerSource, footSource;
    [SerializeField]
    AudioClip reloadSound, damageSound, slideSound, walkSound, jumpSound;
   
    void Start()
    {
        //Initialize Components
        cc = GetComponent<CharacterController>();
        camera = transform.GetChild(0).gameObject;
        _health = maxHealth;
        rightHandAnimator = rightHand.GetComponent<Animator>();

        _rightHandOrigin = rightHand.transform.localPosition;
        _leftHandOrigin = leftHand.transform.localPosition;

        slideEffect.active = false;
        damageEffect.active = false;

        _bulletsLeft = _maxAmmo;
    }

    void Update()
    {
        //Inputs
        _moveInput.x = Input.GetAxis("Horizontal");
        _moveInput.y = Input.GetAxis("Vertical");

        jumping = Input.GetKeyDown(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            sliding = true;
            _slideDecay = 1.5f;
            _decayRate = .005f;
            _slideDirection = transform.forward * moveSpeed;
            _startingSlideSpeed = (_moveInput != Vector2.zero) ? moveSpeed : 0;
            footSource.clip = slideSound;
            footSource.pitch = 1;
            footSource.Play();
        }

        if (Input.GetMouseButtonDown(0) && _bulletsLeft > 0) FirePistol();

        //Step Audio
        if (_moveInput != Vector2.zero && !sliding && grounded) {
            footSource.clip = walkSound;
            if (!footSource.isPlaying) {
                footSource.Play();
            }
        }

        //Determine Controller Height
        cc.height = (sliding) ? .7f : 2;
        _groundedRayLength = (sliding) ? .1f : .9f;

        //Slide Effect
        slideEffect.active = (sliding && _slideDecay > .6f);
         
        //Check Grounded / Above
        checkGrounded();
        checkAbove();
        cameraEffects();

        //Update Health Liquid
        LiquidUpdate();
        _health -= Time.deltaTime;
        _health = Mathf.Clamp(_health, 0, maxHealth);

        //Update Crosshair
        if (_health < 3) {
            if (!crosshairAnimator.GetCurrentAnimatorStateInfo(0).IsName("CountdownTimer"))
                    crosshairAnimator.Play("CountdownTimer", 0, 0);
        } else {
            crosshairAnimator.Play("Default", 0, 0);
        }

        //Configure Movement Vector
        _moveDirection = (_moveInput.x * transform.right + _moveInput.y * transform.forward).normalized;
        _moveDirection *= moveSpeed;

        //Configure Vertical Movement
        if (!grounded) _vMovement -= gravity * Time.deltaTime;
        else _vMovement = 0;

        if (grounded && jumping && !sliding) { _vMovement += jumpForce; playerSource.clip = jumpSound; playerSource.Play(); }
        if (overHead) _vMovement = -1;

        //Slide
        if (sliding) slide();

        //Apply movement
        if (!sliding) cc.Move(_moveDirection * Time.deltaTime);
        cc.Move(new Vector3(0, _vMovement, 0) * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.V)) {
            Instantiate(EnemyPrefab, transform.position + transform.forward * 5, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.R) && _bulletsLeft != _maxAmmo) {
            reload();
        }

        _reloadTimer += Time.deltaTime;
        PlayerFire.canFire = (_reloadTimer > .5f);

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

        Vector3 rightHandDesired;
        rightHandDesired = (sliding) ? (_rightHandOrigin + new Vector3(.05f, .05f, 0)) : (_rightHandOrigin);
        Vector3 leftHandDesired;
        leftHandDesired = (sliding) ? (_leftHandOrigin + new Vector3(.05f, -.01f, 0)) : (_leftHandOrigin);

        leftHand.transform.localPosition = Vector3.Lerp(leftHand.transform.localPosition, leftHandDesired, .1f);
        rightHand.transform.localPosition = Vector3.Lerp(rightHand.transform.localPosition, rightHandDesired, .1f);

        _damageEffectTimer -= Time.deltaTime;
        damageEffect.active = _damageEffectTimer > 0;


    }

    //Slide Player
    void slide() {

        //Slide
        _slideDirection = _slideDirection.normalized * _startingSlideSpeed * _slideDecay;
        cc.Move(_slideDirection * Time.deltaTime);
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            sliding = false;
            cc.Move(new Vector3(0, .9f, 0));
        }

        //Decay Slide
        _slideDecay -= _decayRate;
        _slideDecay = Mathf.Clamp(_slideDecay, 0, 1.5f);
        _decayRate += .000005f;
    }

    void reload() {
        gunSource.clip = reloadSound;
        gunSource.Play();
        rightHandAnimator.Play("gun_reload", -1, 0);
        GunMag.GetComponent<Animator>().Play("Magazine_air", -1, 0);
        _bulletsLeft = _maxAmmo;
        _reloadTimer = 0;
    }

    public void Damage(int damageTaken) {
        _health -= damageTaken;

        float xScale = Random.Range(0, 2) == 0 ? -.099f : .099f;
        float yScale = Random.Range(0, 2) == 0 ? -.099f : .099f;

        damageEffect.transform.localScale = new Vector3(xScale, yScale, .099f);

        _damageEffectTimer = .5f;
        playerSource.clip = damageSound;
        playerSource.pitch = Random.Range(.8f, 1.1f);
        playerSource.Play();
    }

    public void FirePistol() {
        if (_reloadTimer > .5f) {
            rightHandAnimator.Play("RightHandPistolShoot", -1, 0);
            muzzleFlash.Play("muzzle_flash_anim", -1, 0);
            _bulletsLeft--;
        }
    }

    public void LiquidUpdate() {
        float healthProportion = _health / maxHealth;
        idolLiquid.transform.localPosition = new Vector3(.88f, Mathf.Lerp(-.23f, .78f, healthProportion), 0);
    }

    public void Heal() {
        _health += 4;
    }

    public void Respawn() {
        _health = maxHealth;
        _bulletsLeft = _maxAmmo;
    }
}
