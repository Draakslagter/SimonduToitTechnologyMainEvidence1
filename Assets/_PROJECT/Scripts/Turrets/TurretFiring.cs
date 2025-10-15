using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public enum TurretMovementState
{
    Idle,
    Targeting,
}

public enum TurretFiringState
{
    Firing,
    Reloading
}

public class TurretFiring : MonoBehaviour
{
    private TurretMovementState _turretMovementState = TurretMovementState.Idle;
    private TurretFiringState _turretFiringState = TurretFiringState.Firing;

    [Header("Turret Variables")] 
    [SerializeField] private Transform turretTransform;

    [FormerlySerializedAs("turretStats")] [SerializeField] protected TurretDataObject turretDataObject;

    [Header("Target Variables")] [SerializeField]
    private LayerMask enemyLayer;

    private Collider _currentTarget;
    private Transform _targetTransform;

    private void Awake()
    {
        if (turretTransform == null)
        {
            turretTransform = GetComponent<Transform>();
        }
    }
    
    private void FixedUpdate()
    {
        switch (_turretMovementState)
        {
            case TurretMovementState.Idle when _turretFiringState != TurretFiringState.Reloading:
                TurretIdle();
                break;
            case TurretMovementState.Targeting when _turretFiringState == TurretFiringState.Firing:
                SetCurrentTarget();
                AimTurret();
                break;
            default:
                ReloadTurret();
                break;
        }
    }
    
    private void TurretIdle()
    {
        _currentTarget = null;
        var targetsArray = Physics.OverlapSphere(turretTransform.position, turretDataObject.TargetingRadius, enemyLayer);
        if (targetsArray.Length != 0)
        {
            DOTween.Kill(turretTransform);
            _turretMovementState = TurretMovementState.Targeting;
            return;
        }
        
        if (DOTween.IsTweening(turretTransform)) return;
        if (turretTransform.forward != new Vector3(0, 0, 1))
        {
            turretTransform.DORotate(Vector3.zero, turretDataObject.TurnTime).SetEase(turretDataObject.TurretMoveEase);
        }
        else
        {
            _turretMovementState = TurretMovementState.Idle;
            var leftDirection = Quaternion.AngleAxis(-45, transform.up);
            var rightDirection = Quaternion.AngleAxis(+45, transform.up);
            turretTransform.DORotate(leftDirection.eulerAngles, turretDataObject.TurnTime).SetEase(turretDataObject.TurretMoveEase)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    turretTransform.DORotate(rightDirection.eulerAngles, turretDataObject.TurnTime)
                        .SetEase(turretDataObject.TurretMoveEase).SetLoops(2, LoopType.Yoyo);
                });
        }
    }

    private void SetCurrentTarget()
    {
        var targetsArray = Physics.OverlapSphere(turretTransform.position, turretDataObject.TargetingRadius, enemyLayer);
       
        if (targetsArray.Length == 0)
        {
            DOTween.Kill(turretTransform);
            _turretMovementState = TurretMovementState.Idle;
            return;
        }
        
        var targetDistances = new List<float>();
        foreach (var target in targetsArray)
        {
            target.TryGetComponent<Transform>(out var targetTransform);
            var targetDistance = Vector3.Distance(turretTransform.position, targetTransform.position);
            targetDistances.Add(targetDistance);
        }
        
        _currentTarget = targetsArray[targetDistances.IndexOf(targetDistances.Min())];
        _currentTarget.TryGetComponent(out _targetTransform);
    }

    private void AimTurret()
    {
        if (!DOTween.IsTweening(turretTransform))
        {
            turretTransform.DOLookAt(_targetTransform.position, turretDataObject.TurnTime);
        }
        var inRangeArray = Physics.OverlapSphere(turretTransform.position, turretDataObject.TargetingRadius, enemyLayer);
        
        if (!inRangeArray.Contains(_currentTarget) || !TargetInCone()) return;
        ShootTurret(_currentTarget, _targetTransform);
    }
    
    private bool TargetInCone()
    {
        var targetDirection = (_targetTransform.position - turretTransform.position).normalized;
        var leftDirection =
            (Quaternion.AngleAxis(-45, transform.up) * turretTransform.forward * turretDataObject.FiringRadius).normalized;
        var rightDirection =
            (Quaternion.AngleAxis(+45, transform.up) * turretTransform.forward * turretDataObject.FiringRadius).normalized;

        var angleToLeft = Vector3.SignedAngle(leftDirection, targetDirection, Vector3.up);
        var angleBetweenBounds = Vector3.SignedAngle(leftDirection, rightDirection, Vector3.up);

        var isBetween = false;

        if (angleBetweenBounds >= 0)
        {
            if (angleToLeft >= 0 && angleToLeft <= angleBetweenBounds)
            {
                isBetween = true;
            }
        }
        else
        {
            if (angleToLeft <= 0 && angleToLeft >= angleBetweenBounds)
            {
                isBetween = true;
            }
        }

        return isBetween;
    }

    private void ReloadTurret()
    {
        if (DOTween.IsTweening(turretTransform)) return;
        var upRotation = new Vector3(-45, turretTransform.localEulerAngles.y, turretTransform.localEulerAngles.z);
        turretTransform.DORotate(upRotation, turretDataObject.TurnTime/2).SetEase(turretDataObject.TurretMoveEase).SetLoops(2, LoopType.Yoyo).OnComplete(() => { _turretFiringState = TurretFiringState.Firing;});
    }

    protected virtual void ShootTurret(Collider target, Transform targetTransform = null)
    {
        _turretFiringState = TurretFiringState.Reloading;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(turretTransform.position, turretDataObject.TargetingRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(turretTransform.position, turretDataObject.FiringRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(turretTransform.position, turretTransform.forward);
        Gizmos.color = Color.green;
        var leftDirection = Quaternion.AngleAxis(-45, transform.up);
        var rightDirection = Quaternion.AngleAxis(+45, transform.up);
        
        Gizmos.DrawRay(turretTransform.position, leftDirection * turretTransform.forward * turretDataObject.FiringRadius);
        Gizmos.DrawRay(turretTransform.position, rightDirection * turretTransform.forward * turretDataObject.FiringRadius);
    
    }
}