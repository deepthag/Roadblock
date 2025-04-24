using System;
using UnityEngine;

public class RoadBrick : MonoBehaviour
{
    public RoadSpawner roadSpawner;
    void Start()
    {
        roadSpawner = FindFirstObjectByType<RoadSpawner>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        roadSpawner.SpawnTile();
        Destroy(gameObject, 2);
    }
}
