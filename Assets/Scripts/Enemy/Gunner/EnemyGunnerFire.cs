using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunnerFire : MonoBehaviour
{
    public GameObject BulletPrefab;

    void Fire() {
        Instantiate(BulletPrefab, transform.position + new Vector3(0,1.7f,0), Quaternion.identity);
    }
}
