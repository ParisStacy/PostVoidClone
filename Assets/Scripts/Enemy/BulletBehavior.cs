﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float bulletSpeed;

    void Start()
    {
        transform.LookAt(GameObject.Find("Camera").transform.position);
    }

    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision impactObject) {
        if (impactObject.gameObject == gameObject.CompareTag("Player")) {
            impactObject.gameObject.GetComponent<IDamage<int>>().Damage(5);
        }

        Destroy(gameObject);
    }
}
