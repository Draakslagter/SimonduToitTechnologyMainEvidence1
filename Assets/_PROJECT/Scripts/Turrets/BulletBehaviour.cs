using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class BulletBehaviour : MonoBehaviour
{
    private Transform _bulletTransform;
    [SerializeField] private float launchHeight;
    [SerializeField] private Ease bulletEase;
    [SerializeField] private LayerMask enemyLayer;
    private float _explosionRadius;

    private void Awake()
    {
        if (_bulletTransform == null)
        {
            _bulletTransform = GetComponent<Transform>();
        }
    }
    
    public void LaunchProjectile(Transform targetTransform, float travelDuration, float explosionRadius, float damage)
    {
        _explosionRadius = explosionRadius;
        var hitLocation = new Vector3(targetTransform.position.x, 0, targetTransform.position.z);
        _bulletTransform.DOJump(hitLocation, launchHeight, 1, travelDuration).SetEase(bulletEase).OnComplete(() =>
        {
            var hitGroup = Physics.OverlapSphere(_bulletTransform.position, explosionRadius, enemyLayer);
            foreach (var hit in hitGroup)
            {
                hit.TryGetComponent(out IDamageable damageable);
                damageable.TakeDamage(damage);
            }
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        });
        _bulletTransform.DOLookAt(hitLocation, travelDuration).SetEase(bulletEase);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_bulletTransform.position, _explosionRadius);
    }
}
