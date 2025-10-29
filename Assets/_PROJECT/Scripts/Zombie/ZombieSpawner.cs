using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
   [SerializeField] private ZombieMovement zombiePrefab;
   [SerializeField] private Transform[] spawnPoints;
   [SerializeField] private Transform fireTransform;

   public void SpawnZombie()
   {
      Debug.Log("Spawning Zombie");
      var randomSpawnPoint = Random.Range(0, spawnPoints.Length);
      var tempHolder = ObjectPoolManager.SpawnObject(zombiePrefab,spawnPoints[randomSpawnPoint], Quaternion.identity);
      tempHolder.SetWaypoints(spawnPoints,  randomSpawnPoint, fireTransform);
   }
}
