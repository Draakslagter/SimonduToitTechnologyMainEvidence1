using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [SerializeField] private ObjectPoolingExample objectPoolParent;

    public void SetObjectPoolParent(ObjectPoolingExample incomingObjectPool)
    {
        objectPoolParent = incomingObjectPool;
    }
}
