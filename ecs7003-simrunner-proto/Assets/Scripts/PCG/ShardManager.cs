﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardManager : MonoBehaviour
{

    public GameObject BoundaryTile;
    public GameObject[] tiles; // Array used for different type of tiles (if available)

    public float fwdSpawn;
    public float tileLen;
    public Transform playerTransform;
    
    public int noOfAliveBounds;
    public int noOfTilesPerBound;

    public float spawnOffset;
    public Vector3 spawnTileValues;
    public Vector3 rotationValues;

    public int movingTiles;
    public int rotatingTiles;

    //public Vector3 randomMovementValues;
    public Vector3 randomRotationValues;
    public float tileMovementSpeed;
    public float tileRotationSpeed;

    private List<GameObject> aliveBounds = new List<GameObject>();
    private List<GameObject> aliveTiles = new List<GameObject>();
    private int[] tilesToMove;
    private int directionX = 1;
    private int directionY = 1;
    private int directionZ = 1;
    private float rotX = 1f;
    private float rotY = 1f;
    private float rotZ = 1f;

    /**
     * Starts the game with a single game boundary. The rest is built as player moves.
     */
    void Start()
    {
        // Start with a game boundary:
        for (int i = 0; i < noOfAliveBounds; i++)
        {
            //SpawnGameBoundary();
            SpawnTiles();

            tilesToMove = new int[movingTiles];
            for (int arrayIndex = 0; arrayIndex < tilesToMove.Length; arrayIndex++)
            {
                tilesToMove[arrayIndex] = Random.Range(0, aliveTiles.Count);
            }
        }
    }

    /**
     * As player moves spawn new game boundary and kill the old ones.
     */
    void Update()
    {
        if (playerTransform.position.z - spawnOffset > (fwdSpawn - noOfAliveBounds * tileLen))
        {
            SpawnGameBoundary();
            SpawnTiles();

            DestroyBoundary();
            DestroyTiles();
        }

        UpdateTilePositionRotation();

    }

    /**
     * Spawns a tile and adds its to the set of alive tiles.
     */
    public void SpawnGameBoundary()
    {
        GameObject boundary = Instantiate(BoundaryTile, transform.forward * fwdSpawn, transform.rotation);
        aliveBounds.Add(boundary);
        fwdSpawn += tileLen;
    }

/**
 * Randomly spawns tiles in the bounded area.
 * 
 */
    public void SpawnTiles()
    {
        for (int i = 0; i < noOfTilesPerBound; i++)
        {

            int tiletype = Random.Range(0, tiles.Length);

            Vector3 tilePos = new Vector3(Random.Range(-spawnTileValues.x, spawnTileValues.x),
                Random.Range(-spawnTileValues.y, spawnTileValues.y),
                Random.Range(-spawnTileValues.z, spawnTileValues.z));

            if (tiletype != 1) {
                // Rotation can be added as well: - Not to horizontal tile which is set to 1 
                GameObject tile = Instantiate(tiles[tiletype], tilePos + transform.forward * (fwdSpawn - tileLen),
                    Quaternion.Euler(new Vector3(Random.Range(-rotationValues.x, rotationValues.x), Random.Range(-rotationValues.y, rotationValues.y), Random.Range(-rotationValues.z, rotationValues.z))));
                aliveTiles.Add(tile);
            }else
            {
                GameObject tile = Instantiate(tiles[tiletype], tilePos + transform.forward * (fwdSpawn - tileLen),
                    Quaternion.Euler(new Vector3(0,0,0)));
                aliveTiles.Add(tile);
            }

        }
    }

    /**
     * Randomly changes the position and rotation of some tiles for increased complexity.
     * Flat tiles are not moved or rotated. -- BUGGY
     */
    private void UpdateTilePositionRotation()
    {
        for (int i = 0; i < tilesToMove.Length; i++)
        {
            if (aliveTiles[tilesToMove[i]].transform.rotation.eulerAngles.x > randomRotationValues.x)
            {
                rotX = -1f;
            }
            else if (aliveTiles[tilesToMove[i]].transform.rotation.eulerAngles.x < randomRotationValues.x)
            {
                rotX = 1f;
            }

            if (aliveTiles[tilesToMove[i]].transform.rotation.eulerAngles.y > randomRotationValues.y)
            {
                rotY = -1f;
            }
            else if (aliveTiles[tilesToMove[i]].transform.rotation.eulerAngles.y < randomRotationValues.y)
            {
                rotY = 1f;
            }

            if (aliveTiles[tilesToMove[i]].transform.rotation.eulerAngles.z > randomRotationValues.z)
            {
                rotZ = -1f;
            }
            else if (aliveTiles[tilesToMove[i]].transform.rotation.eulerAngles.z < randomRotationValues.z)
            {
                rotZ = 1f;
            }

            Vector3 rotation = new Vector3(rotY * tileRotationSpeed, rotY * tileRotationSpeed * Time.deltaTime, rotZ * tileRotationSpeed * Time.deltaTime);
            aliveTiles[tilesToMove[i]].transform.Rotate(rotation);
        }
 
            // tile movement:
            for (int i = 0; i < tilesToMove.Length; i++)
        {

            if (aliveTiles[tilesToMove[i]].transform.position.x > 15)
            {
                directionX = -1;
            }
            else if (aliveTiles[tilesToMove[i]].transform.position.x < -15)
            {
                directionX = 1;
            }

            if (aliveTiles[tilesToMove[i]].transform.position.y > 15)
            {
                directionY = -1;
            }
            else if (aliveTiles[tilesToMove[i]].transform.position.y < -15)
            {
                directionY = 1;
            }

            // Dont play in Z due to platform construction for now:s
            if (aliveTiles[tilesToMove[i]].transform.position.z > 10)
            {
                directionZ = -1;
            }
            else if (aliveTiles[tilesToMove[i]].transform.position.z < -10)
            {
                directionZ = 1;
            }

            Vector3 movement = Vector3.right * directionX * tileMovementSpeed * Time.deltaTime +
                Vector3.up * directionY * tileMovementSpeed * Time.deltaTime +
                Vector3.forward * directionZ * tileMovementSpeed * Time.deltaTime;
            aliveTiles[tilesToMove[i]].transform.Translate(movement);
        }

    }

    /**
     * Destroys the boundary from the starting position to keep memory in check.
     */
    private void DestroyBoundary()
    {
        Destroy(aliveBounds[0]);
        aliveBounds.RemoveAt(0);
    }

    /**
     * Removes first N tiles we left behind.
     * 
     */
    public void DestroyTiles()
    {
        for (int i = 0; i < noOfTilesPerBound; i++)
        {
            Destroy(aliveTiles[0]);
            aliveTiles.RemoveAt(0);
        }
    }
}
