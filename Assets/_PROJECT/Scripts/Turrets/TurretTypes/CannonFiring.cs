using UnityEngine;

public class CannonFiring : TurretFiring
{
    protected override void ShootTurret(Collider target, Transform targetTransform = null)
    {
        base.ShootTurret(target, targetTransform);
        target.TryGetComponent(out IDamageable damageable);
        damageable.TakeDamage(turretDataObject.Damage);
        //Add VFX anim
        //Add Sound
        
    }
}
