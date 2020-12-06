using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject InstantiatedObjects;
    public GameObject pathMakerPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            Reset();
    }

    void Reset() {

        Pathmaker.tiles.Clear();

        foreach(Transform child in InstantiatedObjects.transform) {
            Destroy(child.gameObject);
        }

        GameObject PrimaryPathMaker = Instantiate(pathMakerPrefab, transform.position, Quaternion.identity);
        PrimaryPathMaker.GetComponent<Pathmaker>().primary = true;
        PrimaryPathMaker.transform.parent = InstantiatedObjects.transform;


    }
}
