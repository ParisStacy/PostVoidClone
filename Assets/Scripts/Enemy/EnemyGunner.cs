using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGunner : MonoBehaviour
{

    public Transform lookPlayer;
    public static bool shooting;
    public static bool dashing;
    public static bool moving;
    public static bool dead;

    [Header("Tuning")]
    public float detectionDistance;
    public float moveTime;
    public float dashOffset;
    public float moveOffset;
    public float moveSpeed;
    public float dashSpeed;

    bool active = false;

    float _t;
    int shotsToFire;

    Vector3 navPoint;

    [SerializeField]
    GameObject bulletPrefab, debugTextObject;

    GameObject player;
    TextMesh stateText;
    NavMeshAgent NavAgent;

    //EnumDeclaration
    enum enemyGunnerState { shoot, dash, move, dead }
    enemyGunnerState myState = enemyGunnerState.move;

    void Start()
    {
        player = GameObject.Find("Player");
        NavAgent = GetComponent<NavMeshAgent>();
        stateText = debugTextObject.GetComponent<TextMesh>();
    }

    void Update() {

        stateText.text = ("[" + myState + "]");

        active = (Vector3.Distance(transform.position, player.transform.position) < detectionDistance);

        if (Input.GetKeyDown(KeyCode.V)) ChooseBehavior();

        NavAgent.speed = (dashing) ? dashSpeed : moveSpeed;

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
            }
        }

        var lookDir = lookPlayer.position - transform.position;
        lookDir.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }

    void UpdateShoot()
    {
        _t -= Time.deltaTime;
        Debug.Log("Bang!");
        shooting = true;
//        Instantiate(bulletPrefab, transform.position + Vector3.up, Quaternion.identity);

        shotsToFire--;

        if (_t < 0)
        {
            shooting = false;
            ChooseBehavior();
        }
    }

    void UpdateDash()
    {
        _t -= Time.deltaTime;

        if (_t < 0 || Vector3.Distance(transform.position, navPoint) < .5f)
        {
            dashing = false;
            ChooseBehavior();
        }
    }

    void UpdateMove()
    {
        _t -= Time.deltaTime;

        if (_t < 0 || Vector3.Distance(transform.position, navPoint) < .5f)
        {
            NavAgent.SetDestination(transform.position);
            moving = false;
            ChooseBehavior();
        }
    }

    void ChooseBehavior()
    {

        Debug.Log("Choosing...");

        int ranNum = Random.Range(0, 100);

        if (ranNum < 33)
        {
            myState = enemyGunnerState.move;
            ChooseNavPoint(player, moveOffset);
            NavAgent.SetDestination(navPoint);
            moving = true;
            _t = moveTime;
            Debug.Log("Moving!");
        }
        else if (ranNum > 33 && ranNum < 66)
        {
            myState = enemyGunnerState.dash;
            dashing = true;
            _t = moveTime;
            ChooseNavPoint(gameObject, dashOffset);
            NavAgent.SetDestination(navPoint);
            Debug.Log("Dashing!");
        }
        else
        {
            myState = enemyGunnerState.shoot;
            shooting = true;
            shotsToFire = Random.Range(1, 4);
            _t = .333f * shotsToFire;
            NavAgent.SetDestination(transform.position);
            Debug.Log("Shooting!");
        }
    }

    void ChooseNavPoint(GameObject origin, float range)
    {
        REGEN:
        navPoint = origin.transform.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        if (Vector3.Distance(origin.transform.position, navPoint) < .5f) {
            goto REGEN;
        }
        //TODO: Check if point is valid on navmesh. if not, return to choose behavior?
    }
}
