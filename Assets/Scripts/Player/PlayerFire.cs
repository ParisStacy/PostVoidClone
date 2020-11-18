using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    GameObject camera;
    PlayerLook playerLookScript;
    public GameObject bulletHolePrefab;
    public bool canFire;


    void Start()
    {
        camera = transform.parent.gameObject;
        playerLookScript = camera.GetComponent<PlayerLook>();
    }

    void Fire() {
        if (canFire) {
            RaycastHit hit;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit)) {
                if (hit.transform.tag == "Enemy") {
                    hit.transform.GetComponent<HitboxBehavior>().Damage(1);
                }
            }
            playerLookScript.Recoil(3);
        }
    }
}
