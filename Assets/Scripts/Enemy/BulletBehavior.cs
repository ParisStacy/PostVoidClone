using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float bulletSpeed;

    float _t;

    void Start()
    {
        transform.LookAt(GameObject.Find("Camera").transform.position);
    }

    void Update()
    {
//          _t += Time.deltaTime;
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
//        transform.position += new Vector3(0, Mathf.Sin(_t), 0);
    }

    void OnCollisionEnter(Collision impactObject) {

        if (impactObject.gameObject.tag == ("Player")) {
            impactObject.gameObject.GetComponent<PlayerMove>().Damage(2);
        }

        Destroy(gameObject);
    }
}
