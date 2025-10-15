using UnityEngine;

public class MortarFiring : TurretFiring
{
    [SerializeField] private BulletBehaviour bulletPrefab;
    [SerializeField] private Transform shootTransform;
    protected override void ShootTurret(Collider target, Transform targetTransform = null)
    {
        base.ShootTurret(target, targetTransform);
        var tempHolder = ObjectPoolManager.SpawnObject(bulletPrefab, shootTransform.position, shootTransform.rotation);
        tempHolder.LaunchProjectile(targetTransform, turretDataObject.ReloadTime/2, turretDataObject.TargetingRadius/4, turretDataObject.Damage);
        //Add VFX anim
        //Add Sound
    }
  
}
