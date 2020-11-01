using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGunner : MonoBehaviour
{
    [Header("Tuning")]
    public float detectionDistance;
    public float moveTime;
    public float dashOffset;
    public float moveOffset;

    bool active = false;
    float _t;
    int shotsToFire;

    Vector3 navPoint;

    [SerializeField]
    GameObject bulletPrefab;

    GameObject player;
    NavMeshAgent NavAgent;

    //EnumDeclaration
    enum enemyGunnerState { shoot, dash, move, dead }
    enemyGunnerState myState = enemyGunnerState.move;

    void Start()
    {
        player = GameObject.Find("Player");
        NavAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        active = (Vector3.Distance(transform.position, player.transform.position) < detectionDistance);

        if (Input.GetKeyDown(KeyCode.V)) ChooseBehavior();

        if (active) {
            switch (myState) {
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
    }

    void UpdateShoot() {
        Debug.Log("Bang!");
        Instantiate(bulletPrefab, transform.position + Vector3.up, Quaternion.identity);
        shotsToFire--;

        if (shotsToFire == 0) {
            ChooseBehavior();
        }
    }

    void UpdateDash() {
        if (NavAgent.remainingDistance < .05f) {
            ChooseBehavior();
        }
    }

    void UpdateMove() {
        _t -= Time.deltaTime;

        if (_t < 0) {
            NavAgent.SetDestination(transform.position);
            ChooseBehavior();
        }
    }

    void ChooseBehavior() {

        Debug.Log("Choosing...");

        int ranNum = Random.Range(0, 100);

        if (ranNum < 33) {
            myState = enemyGunnerState.move;
            ChooseNavPoint(player, moveOffset);
            NavAgent.SetDestination(navPoint);
            _t = moveTime;
            Debug.Log("Moving!");
        } else if (ranNum > 33 && ranNum < 66) {
            myState = enemyGunnerState.dash;
            ChooseNavPoint(gameObject, dashOffset);
            Debug.Log("Dashing!");
        } else {
            myState = enemyGunnerState.shoot;
            shotsToFire = Random.Range(1, 4);
            Debug.Log("Shooting!");
        }
    }

    void ChooseNavPoint(GameObject origin, float range) {
        navPoint = origin.transform.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        //TODO: Check if point is valid on navmesh. if not, return to choose behavior?
    }
}
