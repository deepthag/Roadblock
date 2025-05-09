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
    public GameObject wideObstaclePrefab;
    public float wideObstacleChance = 0.1f;
    
    
    public GameObject gemPrefab;
    [SerializeField] private float gemsToSpawn = 0.5f;
    
    public GameObject slowPowerupPrefab;
    [Range(0f, 1f)]
    public float slowPowerupChance = 0.2f;
    
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
        SpawnPowerup();
    }

    
    void Update()
    {
        
    }
    
    void OnTriggerExit(Collider other)
    {
        if (GameBehavior.Instance.State != Utilities.GameplayState.Play)
            return;

        roadSpawner.SpawnTile();
        StartCoroutine(DestroyIfStillPlaying());
    }

    IEnumerator DestroyIfStillPlaying()
    {
        yield return new WaitForSeconds(4f);

        if (GameBehavior.Instance.State == Utilities.GameplayState.Play)
        {
            Destroy(gameObject);
        }
    }


    void SpawnObstacle()
    {
        float rand = UnityEngine.Random.Range(0f, 1f);
        GameObject obstacleToSpawn;

        if (rand < wideObstacleChance)
        {
            obstacleToSpawn = wideObstaclePrefab;
            Vector3 centerOffset = new Vector3(0, 0, 40f);
            Vector3 centerPosition = transform.position + centerOffset;

            GameObject wideObstacle = Instantiate(obstacleToSpawn, centerPosition, Quaternion.identity, transform);
            StartCoroutine(FadeInObject(wideObstacle));
        }
        else if (rand < wideObstacleChance + tallObstacleChance)
        {
            obstacleToSpawn = tallObstaclePrefab;

            int obstacleIndex = UnityEngine.Random.Range(2, 5);
            Transform spawnPoint = transform.GetChild(obstacleIndex).transform;
            Vector3 offset = new Vector3(0, 0, 40f);
            Vector3 spawnPosition = spawnPoint.position + offset;

            GameObject obstacle = Instantiate(obstacleToSpawn, spawnPosition, Quaternion.identity, transform);
            StartCoroutine(FadeInObject(obstacle));
        }
        else
        {
            obstacleToSpawn = obstaclePrefab;

            int obstacleIndex = UnityEngine.Random.Range(2, 5);
            Transform spawnPoint = transform.GetChild(obstacleIndex).transform;
            Vector3 offset = new Vector3(0, 0, 40f);
            Vector3 spawnPosition = spawnPoint.position + offset;

            GameObject obstacle = Instantiate(obstacleToSpawn, spawnPosition, Quaternion.identity, transform);
            StartCoroutine(FadeInObject(obstacle));
        }
    }

    void SpawnGem()
    {
        for (int i = 0; i < gemsToSpawn; i++)
        {
            GameObject temp = Instantiate(gemPrefab, transform);
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());
            StartCoroutine(FadeInObject(temp));
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
    
    void SpawnPowerup()
    {
        if (slowPowerupPrefab == null) return;

        float rand = UnityEngine.Random.Range(0f, 1f);
        if (rand > slowPowerupChance) return;

        Vector3 spawnPos = GetRandomPointInCollider(GetComponent<Collider>());
        GameObject powerup = Instantiate(slowPowerupPrefab, spawnPos, Quaternion.identity, transform);
        StartCoroutine(FadeInObject(powerup));
    }

    
    public IEnumerator DelayedObstacleSpawn()
    {
        yield return new WaitForSeconds(3f);
        if (GameBehavior.Instance.State == Utilities.GameplayState.Play)
        {
            SpawnObstacle();
        }
    }

    
    
    IEnumerator FadeInObject(GameObject obstacle)
    {
        Renderer renderer = obstacle.GetComponent<Renderer>();

        if (renderer == null)
        {
            renderer = obstacle.GetComponentInChildren<Renderer>(); 
        }

        if (renderer != null)
        {
            Material material = renderer.material;
            
            material.SetFloat("_Mode", 2); 
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;

            Color color = material.color;
            color.a = 0f;
            material.color = color;

            float duration = 1f;
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
