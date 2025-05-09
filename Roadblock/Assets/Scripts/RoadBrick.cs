using System;
using UnityEngine;
using Random = System.Random;
using System.Collections;

public class RoadBrick : MonoBehaviour
{
    public RoadSpawner roadSpawner;
    public GameObject obstaclePrefab;
    public GameObject tallObstaclePrefab;
    public float tallObstacleChance = 0.3f;
    public GameObject gemPrefab;
    [SerializeField] private int gemsToSpawn = 1;
    
    public static int roadBricksSpawned = 0;
    void Start()
    {
        roadSpawner = FindFirstObjectByType<RoadSpawner>();
        
        roadBricksSpawned++; 

        if (roadBricksSpawned > 3)
        {
            StartCoroutine(DelayedObstacleSpawn()); // Start spawning obstacles only after 3 tiles
        }
        
        SpawnGem();
    }

    
    void Update()
    {
        
    }

    void OnTriggerExit(Collider other)
    {
        roadSpawner.SpawnTile();
        Destroy(gameObject, 4);
    }

    void SpawnObstacle()
    {
        GameObject obstacleToSpawn = obstaclePrefab;
        float rand = UnityEngine.Random.Range(0f, 1f);
        if (rand < tallObstacleChance)
        {
            obstacleToSpawn = tallObstaclePrefab;
        }
        
        int obstacleIndex = UnityEngine.Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleIndex).transform;

        Vector3 offset = new Vector3(0, 0, 40f);
        Vector3 spawnPosition = spawnPoint.position + offset;

        GameObject obstacle = Instantiate(obstacleToSpawn, spawnPosition, Quaternion.identity, transform);
        
        StartCoroutine(FadeInObstacle(obstacle));
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
    
    public IEnumerator DelayedObstacleSpawn()
    {
        yield return new WaitForSeconds(3f); 
        SpawnObstacle();
    }
    
    IEnumerator FadeInObstacle(GameObject obstacle)
    {
        Renderer renderer = obstacle.GetComponent<Renderer>();

        if (renderer == null)
        {
            renderer = obstacle.GetComponentInChildren<Renderer>(); // fallback if Renderer is on child
        }

        if (renderer != null)
        {
            Material material = renderer.material;
            Color color = material.color;
            color.a = 0f;
            material.color = color;

            float duration = 1f; // How long the fade takes
            float elapsed = 0f;

            while (elapsed < duration)
            {
                color.a = Mathf.Lerp(0f, 1f, elapsed / duration);
                material.color = color;
                elapsed += Time.deltaTime;
                yield return null;
            }

            color.a = 1f;
            material.color = color;
        }
    }

}
