using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolRespawn : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 2.5f) {
            GameObject.Find("GameManager").GetComponent<GameManager>().Reset();
        }
    }

}
