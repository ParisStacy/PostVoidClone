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
                    if (hit.transform.GetComponent<HitboxBehavior>() != null) {
                        hit.transform.GetComponent<HitboxBehavior>().Damage(1);
                    }
                }
                if (hit.transform.tag == "Untagged") {
                    Instantiate(bulletHolePrefab, hit.point + (hit.normal * .05f), Quaternion.FromToRotation(Vector3.forward, hit.normal));
                }
            }
            playerLookScript.Recoil(3);
        }
    }
}
