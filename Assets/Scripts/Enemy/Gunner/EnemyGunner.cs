using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGunner : MonoBehaviour, IEnemy
{

    [Header("Tuning")]
    public float detectionDistance;
    public float moveTime;
    public float dashOffset;
    public float moveOffset;
    public float moveSpeed;
    public float dashSpeed;

    bool active = false;

    float _t, health = 9;
    int shotsToFire;

    Vector3 navPoint;

    [Header("Configuration")]
    [SerializeField]
    GameObject debugTextObject;
    [SerializeField]
    Animator gunnerAnimator;
    [SerializeField]
    GameObject healingOrbPrefab;
    [SerializeField]
    GameObject drippingBloodPrefab;
    [SerializeField]
    AudioSource GunnerAudioSource;
    [SerializeField]
    AudioClip deathSound;

    GameObject player;
    TextMesh stateText;
    NavMeshAgent NavAgent;

    //EnumDeclaration
    enum enemyGunnerState { idle, move, dash, shoot, dead }
    enemyGunnerState myState = enemyGunnerState.move;

    void Start()
    {
        player = GameObject.Find("Player");
        NavAgent = GetComponent<NavMeshAgent>();
        stateText = debugTextObject.GetComponent<TextMesh>();
    }

    void Update() {

        //Set Animator and State Text
        gunnerAnimator.SetInteger("gunnerState", (int)myState);
        stateText.text = ("[" + myState + "]");

        //Is Active?
        active = (Vector3.Distance(transform.position, player.transform.position) < detectionDistance);

        //Is Dead?
        if (health <= 0) myState = enemyGunnerState.dead;

        //Determine Speed
        NavAgent.speed = Mathf.Lerp(NavAgent.speed, (myState == enemyGunnerState.dash) ? dashSpeed : moveSpeed, .01f);

        //Determine Update Loop if Active
        if (active)
        {
            switch (myState)
            {
                case (enemyGunnerState.shoot):
                    UpdateShoot();
                    break;
                case (enemyGunnerState.dash):
                    UpdateDash();
                    break;
                case (enemyGunnerState.move):
                    UpdateMove();
                    break;
                case (enemyGunnerState.dead):
                    break;
            }
        }
    }

    void UpdateShoot()
    {
        _t -= Time.deltaTime;

        if (_t < 0)
        {
            ChooseBehavior(9);
        }
    }

    void UpdateDash()
    {
        _t -= Time.deltaTime;

        if (_t < 0 || Vector3.Distance(transform.position, navPoint) < .5f)
        {
            ChooseBehavior(1);
        }
    }

    void UpdateMove()
    {
        _t -= Time.deltaTime;

        if (_t < 0 || Vector3.Distance(transform.position, navPoint) < .5f)
        {
            NavAgent.SetDestination(transform.position);
            ChooseBehavior(9);
        }
    }

    void ChooseBehavior(int numberToSkip)
    {

        int ranNum = ranNum = Random.Range(0, 3);
        while (ranNum == numberToSkip) {
            ranNum = Random.Range(0, 3);
        }

        if (ranNum == 0) {
            myState = enemyGunnerState.move;
            ChooseNavPoint(player, moveOffset, 1);
            NavAgent.SetDestination(navPoint);
            _t = moveTime;
        } else if (ranNum == 1) {
            myState = enemyGunnerState.dash;
            _t = moveTime;
            ChooseNavPoint(gameObject, dashOffset, 3);
            NavAgent.SetDestination(navPoint);
        } else if (ranNum == 2) {
            myState = enemyGunnerState.shoot;
            shotsToFire = Random.Range(1, 2);
            _t = .4f * shotsToFire;
            NavAgent.SetDestination(transform.position);
        }
    }

    void ChooseNavPoint(GameObject origin, float range, float innerRange)
    {
        navPoint = origin.transform.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        while (Vector3.Distance(origin.transform.position, navPoint) < innerRange) {
            navPoint = origin.transform.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        }
    }

    public void takeDamage(float damage) {
        if (myState != enemyGunnerState.dead) {

            gunnerAnimator.Play("gunner_hurt_ani", -1, 0);
            NavAgent.speed = 0;
            health -= damage;

            if (health <= 0) {
                GunnerAudioSource.clip = deathSound;
                GunnerAudioSource.Play();

                myState = enemyGunnerState.dead;
                gunnerAnimator.Play("gunner_death_ani", -1, 0);
                NavAgent.SetDestination(transform.position);
                transform.GetChild(0).gameObject.active = false;
                transform.GetChild(1).gameObject.active = false;
                Instantiate(healingOrbPrefab, transform.position + new Vector3(0, 1.7f, 0), Quaternion.identity);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.up, out hit, 3)) {
                    Instantiate(drippingBloodPrefab, transform.position, transform.rotation);
                }
            }

        }
    }
}
