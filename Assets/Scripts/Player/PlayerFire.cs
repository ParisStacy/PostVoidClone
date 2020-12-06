using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    GameObject camera;
    PlayerLook playerLookScript;
    public GameObject bulletHolePrefab;
    public GameObject instantiatedObjects;
    public bool canFire;
    public AudioClip fireSound;
    public AudioSource gunSource;

    void Start()
    {
        camera = transform.parent.gameObject;
        playerLookScript = camera.GetComponent<PlayerLook>();
    }

    void Fire() {
        if (canFire) {
            gunSource.clip = fireSound;
            gunSource.Play();
            RaycastHit hit;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit)) {
                if (hit.transform.tag == "Enemy") {
                    if (hit.transform.GetComponent<HitboxBehavior>() != null) {
                        hit.transform.GetComponent<HitboxBehavior>().Damage(1);
                    }
                }
                if (hit.transform.tag == "Untagged") {
                    GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point + (hit.normal * .05f), Quaternion.FromToRotation(Vector3.forward, hit.normal));
                    bulletHole.transform.parent = instantiatedObjects.transform;
                }
            }
            playerLookScript.Recoil(3);
        }
    }
}
