using System;
using UnityEngine;
using Random = System.Random;

public class RoadBrick : MonoBehaviour
{
    public RoadSpawner roadSpawner;
    public GameObject obstaclePrefab;
    public GameObject gemPrefab;
    [SerializeField] private int gemsToSpawn = 1;
    void Start()
    {
        roadSpawner = FindFirstObjectByType<RoadSpawner>();
        SpawnObstacle();
        SpawnGem();
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

    void SpawnGem()
    {
        for (int i = 0; i < gemsToSpawn; i++)
        {
            GameObject temp = Instantiate(gemPrefab, transform);
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());
        }
    }

    Vector3 GetRandomPointInCollider(Collider collider)
    {
        Vector3 point = new Vector3(
            UnityEngine.Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            UnityEngine.Random.Range(collider.bounds.min.y, collider.bounds.max.y),
            UnityEngine.Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );

        if (point != collider.ClosestPoint(point))
        {
            point = GetRandomPointInCollider(collider);
        }

        point.y = 0.25f;
        return point;
    }
}
