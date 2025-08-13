using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingExample : MonoBehaviour
{
    private List<GameObject> _objectPool = new List<GameObject>();
    
    [SerializeField] private GameObject pooledObjectPrefab;
    
    [SerializeField] private float objectPoolSize;

    private void Start()
    {
        for (var i = 0; i < objectPoolSize; i++)
        {
            var temporaryGameObject = Instantiate(pooledObjectPrefab, transform);
            if (temporaryGameObject.TryGetComponent<PooledObject>(out var foundObject))
            {
                foundObject.SetObjectPoolParent(this);
            }
            temporaryGameObject.SetActive(false);
            _objectPool.Add(temporaryGameObject);
        }
    }

    public void EnableObject()
    {
        var tempObject = GetPooledObject();
        if (tempObject != null)
        {
            GetPooledObject().SetActive(true);
        }
    }
    public GameObject GetPooledObject()
    {
        foreach (var pooledItem in _objectPool)
        {
            if (!pooledItem.activeInHierarchy)
            {
                return pooledItem;
            }
        }

        return null;
    }
}
