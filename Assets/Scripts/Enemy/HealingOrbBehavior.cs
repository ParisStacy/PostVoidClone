using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingOrbBehavior : MonoBehaviour
{
    [SerializeField]
    float speed;
    GameObject target, player;

    void Start()
    {
        target = GameObject.Find("LeftHand");
        player = GameObject.Find("Player");
    }

    void Update() {
        transform.LookAt(target.transform.position);

        transform.position += transform.forward * speed * Time.deltaTime;


        if (Vector3.Distance(transform.position, target.transform.position) < .5f) {
            player.GetComponent<PlayerMove>().Heal();
            Destroy(gameObject);
        }
    }
}
