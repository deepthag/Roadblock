using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    public GameObject roadBrick;
    Vector3 nextSpawnPoint;
    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            SpawnTile();
        }
    }
    
    void Update()
    {
        
    }

    public void SpawnTile()
    {
        GameObject temp = Instantiate(roadBrick, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = temp.transform.GetChild(1).position;
    }
}
