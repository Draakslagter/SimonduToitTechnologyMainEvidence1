using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
   [SerializeField] private GameObject zombiePrefab;
   [SerializeField] private Transform[] spawnPoints;
   [SerializeField] private float zombieSpawnRate;
   [SerializeField] private Transform playerTransform;

   public void SpawnZombie()
   {
      ObjectPoolManager.SpawnObject(zombiePrefab,spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity);
   }
}
