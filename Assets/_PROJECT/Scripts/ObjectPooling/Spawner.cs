using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _objectToSpawn;

    private void Start()
    {
        ObjectPoolManager.SpawnObject(_objectToSpawn,Vector3.zero, Quaternion.identity);
    }
}
