using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{

    [SerializeField] private bool _addToDontDestroyOnLoad;

    private GameObject _emptyHolder;

    private static GameObject _gameObjectsEmpty;

    private static Dictionary<GameObject, ObjectPool<GameObject>> _objectsPools;
    private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;

    public enum PoolType
    {
        GameObjects
    }
    public static PoolType PoolingType;

    private void Awake()
    {
        _objectsPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        _cloneToPrefabMap = new Dictionary<GameObject, GameObject>();
        
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _emptyHolder = new GameObject("Object Pools");

        _gameObjectsEmpty = new GameObject("GameObjects");
        _gameObjectsEmpty.transform.SetParent(_emptyHolder.transform);

        if (_addToDontDestroyOnLoad)
        {
            //add what not to destroy if ticked
        }
    }

    private static void CreatePool(GameObject prefab, Transform parent, Quaternion rotation,
        PoolType poolType = PoolType.GameObjects)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, parent, rotation, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );
        
        _objectsPools.Add(prefab, pool);
    }
    private static void CreatePool(GameObject prefab, Vector3 position, Quaternion rotation,
        PoolType poolType = PoolType.GameObjects)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, position, rotation, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );
        
        _objectsPools.Add(prefab, pool);
    }

    private static GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion rotation,
        PoolType poolType = PoolType.GameObjects)
    {
        prefab.SetActive(false);
        GameObject obj = Instantiate(prefab, position, rotation);
        
        prefab.SetActive(true);

        GameObject parentObject = SetParentObject(poolType);
        obj.transform.SetParent(parentObject.transform);

        return obj;
    }
    private static GameObject CreateObject(GameObject prefab, Transform parent, Quaternion rotation,
        PoolType poolType = PoolType.GameObjects)
    {
        prefab.SetActive(false);
        GameObject obj = Instantiate(prefab, parent);
        
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = rotation;
        obj.transform.localScale = Vector3.one;
        
        prefab.SetActive(true);

        return obj;
    }

    private static void OnGetObject(GameObject gameObject)
    {
        //optional logic
    }

    private static void OnReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private static void OnDestroyObject(GameObject gameObject)
    {
        if (_cloneToPrefabMap.ContainsKey(gameObject))
        {
            _cloneToPrefabMap.Remove(gameObject);
        }
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.GameObjects:
                return _gameObjectsEmpty;
            default:
                return null;
        }
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation,
        PoolType poolType = PoolType.GameObjects) where T : Object
    {
        if (!_objectsPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, spawnPosition, spawnRotation, poolType);
        }

        GameObject obj = _objectsPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }
            obj.transform.position = spawnPosition;
            obj.transform.rotation = spawnRotation;
            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }
            
            T component  = obj.GetComponent<T>();
            if (component != null)
            {
                Debug.LogError($"Object {objectToSpawn.name} doesn't have a component of type {typeof(T)}");
                return null;
            }
            
            return component;
        }
        return null;
    }

    public static T SpawnObject<T>(T typePrefab, Vector3 spawnPosition, Quaternion spawnRotation,
        PoolType poolType = PoolType.GameObjects) where T : Component
    {
        return SpawnObject<T>(typePrefab, spawnPosition, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation,
        PoolType poolType = PoolType.GameObjects)
    {
        return SpawnObject<GameObject>(objectToSpawn, spawnPosition, spawnRotation, poolType);
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Transform parent, Quaternion spawnRotation,
        PoolType poolType = PoolType.GameObjects) where T : Object
    {
        if (!_objectsPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, parent, spawnRotation, poolType);
        }

        GameObject obj = _objectsPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }
            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = spawnRotation;
            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }
            
            T component  = obj.GetComponent<T>();
            if (component != null)
            {
                Debug.LogError($"Object {objectToSpawn.name} doesn't have a component of type {typeof(T)}");
                return null;
            }
            
            return component;
        }
        return null;
    }
    public static T SpawnObject<T>(T typePrefab, Transform parent, Quaternion spawnRotation,
        PoolType poolType = PoolType.GameObjects) where T : Component
    {
        return SpawnObject<T>(typePrefab, parent, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parent, Quaternion spawnRotation,
        PoolType poolType = PoolType.GameObjects)
    {
        return SpawnObject<GameObject>(objectToSpawn, parent, spawnRotation, poolType);
    }

    
    public static void ReturnObjectToPool(GameObject gameObject, PoolType poolType = PoolType.GameObjects)
    {
        if (_cloneToPrefabMap.TryGetValue(gameObject, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(poolType);
            if (gameObject.transform.parent != parentObject.transform)
            {
                gameObject.transform.SetParent(parentObject.transform);
            }

            if (_objectsPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Trying to return object that was not in the pool: " + gameObject.name );
        }
    }
}
