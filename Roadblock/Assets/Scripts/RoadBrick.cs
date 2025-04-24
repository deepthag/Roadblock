using System;
using UnityEngine;

public class RoadBrick : MonoBehaviour
{
    public RoadSpawner roadSpawner;
    public GameObject obstaclePrefab;
    void Start()
    {
        roadSpawner = FindFirstObjectByType<RoadSpawner>();
        SpawnObstacle();
    }

    
    void Update()
    {
        
    }

    void OnTriggerExit(Collider other)
    {
        roadSpawner.SpawnTile();
        Destroy(gameObject, 2);
    }

    void SpawnObstacle()
    {
        int obstacleIndex = UnityEngine.Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleIndex).transform;
        
        Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
    }
}
