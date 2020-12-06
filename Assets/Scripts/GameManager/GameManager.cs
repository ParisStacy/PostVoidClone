using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject InstantiatedObjects;
    public GameObject pathMakerPrefab;
    public GameObject playerPrefab;

    void Start()
    {
        Reset();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            Reset();
    }

    public void Reset() {

        Pathmaker.tiles.Clear();

        foreach(Transform child in InstantiatedObjects.transform) {
            Destroy(child.gameObject);
        }

        GameObject PrimaryPathMaker = Instantiate(pathMakerPrefab, transform.position, Quaternion.identity);
        PrimaryPathMaker.GetComponent<Pathmaker>().primary = true;
        PrimaryPathMaker.transform.parent = InstantiatedObjects.transform;

        Debug.Log("teleported player");
        playerPrefab.transform.position = new Vector3(-4.5f, 17.0f, -4.5f);
        playerPrefab.GetComponent<PlayerMove>().Respawn();

    }
}
