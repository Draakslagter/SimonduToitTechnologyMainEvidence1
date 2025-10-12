using UnityEngine;

public class MortarFiring : TurretFiring
{
    [SerializeField] private BulletBehaviour bulletPrefab;
    [SerializeField] private Transform shootTransform;
    protected override void ShootTurret(Collider target, Transform targetTransform = null)
    {
        base.ShootTurret(target, targetTransform);
        var tempHolder = ObjectPoolManager.SpawnObject(bulletPrefab, shootTransform.position, shootTransform.rotation);
        tempHolder.LaunchProjectile(targetTransform, turretStats.ReloadTime/2, turretStats.TargetingRadius/4, turretStats.Damage);
        //Add VFX anim
        //Add Sound
    }
  
}
