using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
   [SerializeField] private ZombieMovement zombiePrefab;
   [SerializeField] private Transform[] spawnPoints;
   [SerializeField] private float zombieSpawnRate;
   [SerializeField] private Transform fireTransform;

   public void SpawnZombie()
   {
      Debug.Log("Spawning Zombie");
      var tempHolder = ObjectPoolManager.SpawnObject(zombiePrefab,spawnPoints[Random.Range(0, spawnPoints.Length)], Quaternion.identity);
      tempHolder.SetTargets(fireTransform);
   }
}
