using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Linq;

public class ObjectSpawner : MonoBehaviour
{
    public enum ObjectType { BigCoin, SmallCoin, Enemy };

    public Tilemap tilemap;
    public GameObject[] objectPrefabs;
    public float bigCoinProbibility = 0.2f;
    public float EnemyProbibility = 0.1f;
    public int maxObjects = 5;
    public float coinLifeTime = 10f;
    public float spawnInterval = 0.5f;

    private List<Vector3> validSpawnPositions = new List<Vector3>();
    private List<GameObject> spawnObjects = new List<GameObject>();
    private bool isSpawning = false;

    void Start()
    {
        GatherValidPositions();
        StartCoroutine(SpawnObjectsIfNeeded());
        GameController.OnReset += LevelChange;
    }

    void Update()
    {
        if (!tilemap.gameObject.activeInHierarchy)
        {
            LevelChange();
        }
        if(!isSpawning && ActiveObjectCount() < maxObjects)
        {
            StartCoroutine(SpawnObjectsIfNeeded());
        }
    }

    private void LevelChange()
    {
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        GatherValidPositions();
        DestroySpawnedObjects();
    }

    private int ActiveObjectCount()
    {
        spawnObjects.RemoveAll(item => item == null);
        return spawnObjects.Count;
    }

    private IEnumerator SpawnObjectsIfNeeded()
    {
        isSpawning = true;

        while (ActiveObjectCount() < maxObjects)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    private bool PositionHasObject(Vector3 positionToCheck)
    {
        return spawnObjects.Any(checkObj =>
            checkObj && Vector3.Distance(checkObj.transform.position, positionToCheck) < 1.0f);
    }

    private ObjectType RandomObjectType()
    {
        float randomChoice = Random.value;

        if (randomChoice <= EnemyProbibility)
        {
            return ObjectType.Enemy;
        }
        else if (randomChoice <= (EnemyProbibility + bigCoinProbibility))
        {
            return ObjectType.BigCoin;
        }
        else
        {
            return ObjectType.SmallCoin;
        }
    }

    private void SpawnObject()
    {
        if (validSpawnPositions.Count == 0) return;

        Vector3 spawnPosition = Vector3.zero;
        bool validPositionFound = false;

        while (!validPositionFound && validSpawnPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, validSpawnPositions.Count);
            Vector3 potentialPosition = validSpawnPositions[randomIndex];

            Vector3 leftPosition = potentialPosition + Vector3.left;
            Vector3 rightPosition = potentialPosition + Vector3.right;

            if (!PositionHasObject(leftPosition) && !PositionHasObject(rightPosition))
            {
                spawnPosition = potentialPosition;
                validPositionFound = true;
            }

            validSpawnPositions.RemoveAt(randomIndex);
        }

        if (validPositionFound)
        {
            ObjectType objectType = RandomObjectType();
            GameObject obj = Instantiate(
                objectPrefabs[(int)objectType],
                spawnPosition,
                Quaternion.identity);

            spawnObjects.Add(obj);

            if (objectType != ObjectType.Enemy)
            {
                StartCoroutine(DestroyObjectAfterTime(obj, coinLifeTime));
            }
        }
    }

    private IEnumerator DestroyObjectAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        if (obj)
        {
            spawnObjects.Remove(obj);
            validSpawnPositions.Add(obj.transform.position);
            Destroy(obj);
        }
    }

    private void DestroySpawnedObjects()
    {
        foreach(GameObject obj in spawnObjects)
        {
            if(obj != null)
            {
                Destroy(obj);
            }
        }
        spawnObjects.Clear();
    }

    private void GatherValidPositions()
    {
        validSpawnPositions.Clear();

        BoundsInt boundsInt = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(boundsInt);
        Vector3 start = tilemap.CellToWorld(new Vector3Int(boundsInt.xMin, boundsInt.yMin, 0));

        for (int x = 0; x < boundsInt.size.x; x++)
        {
            for (int y = 0; y < boundsInt.size.y; y++)
            {
                TileBase tile = allTiles[x + y * boundsInt.size.x];

                if (tile != null)
                {
                    Vector3 place = start + new Vector3(x + 0.5f, y + 1.5f, 0);
                    validSpawnPositions.Add(place);
                }
            }
        }
    }
}