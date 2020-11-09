using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    GameObject camera;
    public GameObject bulletHolePrefab;

    void Start()
    {
        camera = transform.parent.gameObject;
    }

    void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit)) {
            if (hit.transform.tag == "Enemy") {
                hit.transform.GetComponent<HitboxBehavior>().Damage(1);
            }
        }
    }
}
