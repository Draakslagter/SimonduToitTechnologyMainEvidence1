using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [SerializeField] private ObjectPoolManager objectPoolParent;

    public void SetObjectPoolParent(ObjectPoolManager incomingObjectPool)
    {
        objectPoolParent = incomingObjectPool;
    }
}
