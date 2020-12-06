using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{

    public Sprite[] props;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = props[Random.Range(0, props.Length)];

        if (transform.rotation.x != 0) {
            transform.Rotate(0, 0, 180);
        }
    }

    void Update()
    {
        
    }
}
