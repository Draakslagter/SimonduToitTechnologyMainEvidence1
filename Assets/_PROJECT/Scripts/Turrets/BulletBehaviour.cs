using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class BulletBehaviour : MonoBehaviour
{
    private Transform _bulletTransform;
    [SerializeField] private float launchHeight;
    [SerializeField] private Ease bulletEase;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private VisualEffect bulletEffect;
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
            bulletEffect.SendEvent("OnExplode");
            var hitGroup = Physics.OverlapSphere(_bulletTransform.position, explosionRadius, enemyLayer);
            foreach (var hit in hitGroup)
            {
                hit.TryGetComponent(out IDamageable damageable);
                damageable.TakeDamage(damage);
            }

            StartCoroutine(WaitBeforePool());
        });
        _bulletTransform.DOLookAt(hitLocation, travelDuration/2).SetEase(bulletEase);
    }

    private IEnumerator WaitBeforePool()
    {
        yield return new WaitForSeconds(0.5f);
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_bulletTransform.position, _explosionRadius);
    }
}
