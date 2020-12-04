using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MAZE PROC GEN LAB
// all students: complete steps 1-6, as listed in this file
// optional: if you're up for a bit of a mind safari, complete the "extra tasks" to do at the very bottom

public class Pathmaker : MonoBehaviour {

    public static int tileTotal = 0;

    public static List<GameObject> tiles = new List<GameObject>();

    int _tileCount = 0;
    public Transform floorPrefab;
    public Transform pathmakerSpherePrefab;
    public GameObject camera;

    int _maxTileCount;
    float _spawnNewChance;

    public Sprite[] tileTypes;

    Vector3 avgPosition;

    void Start() {
        _maxTileCount = Random.Range(40, 60);
        _spawnNewChance = Random.Range(.95f, 1.0f);
    }

    void Update () {

        Debug.Log(tileTotal);

        if (_tileCount < _maxTileCount && tileTotal < 500) {
            float numGen = Random.Range(0.0f, 1.0f);
            if (numGen < 0.25f) transform.Rotate(0, 90, 0);
            else if (numGen > .25f && numGen < 0.5f) transform.Rotate(0, -90, 0);
            else if (numGen > _spawnNewChance) Instantiate(pathmakerSpherePrefab, transform.position, Quaternion.identity);

            transform.position += transform.forward * 5;
            Collider[] hitCollider = Physics.OverlapSphere(transform.position, .1f);

            if (hitCollider.Length == 0) {
                Debug.Log("Instantiate");
                GameObject newTile = Instantiate(floorPrefab, transform.position, Quaternion.identity).gameObject;

                int tilePick;

                if (numGen < .1f) tilePick = 1;
                else if (numGen > .9f) tilePick = 2;
                else tilePick = 0;

                newTile.GetComponentInChildren<SpriteRenderer>().sprite = tileTypes[tilePick];

                tiles.Add(newTile);

                _tileCount++;
                tileTotal++;
            }
        } else {
            if (tileTotal > 201) {
                Destroy(gameObject);
            } else {
                _tileCount = 0;
            }
        }
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
