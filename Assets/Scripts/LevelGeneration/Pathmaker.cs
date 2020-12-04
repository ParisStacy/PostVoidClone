using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// MAZE PROC GEN LAB
// all students: complete steps 1-6, as listed in this file
// optional: if you're up for a bit of a mind safari, complete the "extra tasks" to do at the very bottom

public class Pathmaker : MonoBehaviour {

    public static int tileTotal = 0;

    bool ending;

    public static List<GameObject> tiles = new List<GameObject>();

    int _tileCount = 0;
    public GameObject floorPrefab;
    public GameObject roofPrefab;
    public GameObject wallPrefab;

    public GameObject enemyPrefab;

    public GameObject pathmakerSpherePrefab;

    int _maxTileCount;
    int _forwardCount;
    float _spawnNewChance;

    private NavMeshBuilder NavBuilder;

    int[] directions = new int[] { 0, 0, 90, -90};

    [Header("Individual Config")]
    public bool primary;
    public int maxTileCount; 

    void Start() {
        _maxTileCount = maxTileCount;
        _spawnNewChance = Random.Range(.95f, 1.0f);
    }

    void Update () {

        Debug.Log(tileTotal);

        if (_tileCount < _maxTileCount && tileTotal < 500) {
            int numGen = Random.Range(0,directions.Length);
            int rotation = directions[numGen];

            if (rotation == 0 && primary) _forwardCount++; 

            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));

            float instantiateGen = Random.Range(0.0f, 1.0f);
            if (instantiateGen < .2f && primary) {
                GameObject newPathMaker = Instantiate(pathmakerSpherePrefab, transform.position, Quaternion.identity);
                newPathMaker.GetComponent<Pathmaker>().maxTileCount = 4;
                newPathMaker.GetComponent<Pathmaker>().primary = false;

                Instantiate(enemyPrefab, transform.position + Vector3.down * 1.5f, Quaternion.identity);
            }

//            else if (numGen > _spawnNewChance) Instantiate(pathmakerSpherePrefab, transform.position, Quaternion.identity);

            transform.position += transform.forward * 3;
            Collider[] hitCollider = Physics.OverlapSphere(transform.position - (Vector3.up * 3), .1f);

            if (hitCollider.Length == 0) {
                Debug.Log("Instantiate");
                GameObject floorTile = Instantiate(floorPrefab, transform.position - (Vector3.up * 3), Quaternion.identity);
                GameObject roofTile = Instantiate(roofPrefab, transform.position - (Vector3.down * 3), Quaternion.identity);

                tiles.Add(floorTile);

                _tileCount++;
                tileTotal++;
            }

        } else {
            if (primary && _forwardCount < 50) {
                _tileCount = 0;
            } else {
                if (primary && !ending) {
                    CreateWalls();
                } else {
                    Destroy(gameObject);
                }
            }
        }
	}

    public void CreateWalls() {
        foreach(GameObject tile in tiles) {
            if (!Physics.CheckSphere(tile.transform.position + Vector3.forward * 3, .5f)) {
                Instantiate(wallPrefab, tile.transform.position + (Vector3.forward * 3) + (Vector3.up * 4.5f), Quaternion.identity);
            }
            if (!Physics.CheckSphere(tile.transform.position + Vector3.back * 3, .5f)) {
                Instantiate(wallPrefab, tile.transform.position + (Vector3.back * 3) + (Vector3.up * 4.5f), Quaternion.identity);
            }
            if (!Physics.CheckSphere(tile.transform.position + Vector3.right * 3, .5f)) {
                Instantiate(wallPrefab, tile.transform.position + (Vector3.right * 3) + (Vector3.up * 4.5f), Quaternion.identity);
            }
            if (!Physics.CheckSphere(tile.transform.position + Vector3.left * 3, .5f)) {
                Instantiate(wallPrefab, tile.transform.position + (Vector3.left * 3) + (Vector3.up * 4.5f), Quaternion.identity);
            }
        }

        NavMeshBuilder builder;
       

        ending = true;
    }

    public void Reset() {
        tileTotal = 0;
        tiles.Clear();
    }

} 

// OPTIONAL EXTRA TASKS TO DO, IF YOU WANT / DARE: ===================================================

// BETTER UI:
// learn how to use UI Sliders (https://unity3d.com/learn/tutorials/topics/user-interface-ui/ui-slider) 
// let us tweak various parameters and settings of our tech demo
// let us click a UI Button to reload the scene, so we don't even need the keyboard anymore.  Throw that thing out!

// WALL GENERATION
// add a "wall pass" to your proc gen after it generates all the floors
// 1. raycast downwards around each floor tile (that'd be 8 raycasts per floor tile, in a square "ring" around each tile?)
// 2. if the raycast "fails" that means there's empty void there, so then instantiate a Wall tile prefab
// 3. ... repeat until walls surround your entire floorplan
// (technically, you will end up raycasting the same spot over and over... but the "proper" way to do this would involve keeping more lists and arrays to track all this data)
