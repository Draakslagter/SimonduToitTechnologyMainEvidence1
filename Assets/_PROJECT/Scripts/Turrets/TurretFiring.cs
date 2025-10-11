using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public enum TurretActionState
{
    Idle,
    Targeting,
    Firing
}

public class TurretFiring : MonoBehaviour
{
    private TurretActionState _turretActionState = TurretActionState.Idle;

    [SerializeField] private Transform _turretTransform;
    [SerializeField] private TurretStats turretStats;

    [SerializeField] private LayerMask enemyLayer;
    private Collider _currentTarget;
    private Transform _targetTransform;

    private void Awake()
    {
        if (_turretTransform == null)
        {
            _turretTransform = GetComponent<Transform>();
        }
    }

    private void Update()
    {
        var targetsArray = Physics.OverlapSphere(_turretTransform.position, turretStats.TargetingRadius, enemyLayer);
        var inRangeArray = Physics.OverlapSphere(_turretTransform.position, turretStats.TargetingRadius, enemyLayer);
        
        if (targetsArray.Length == 0)
        {
            _currentTarget = null;
            TurretIdle();
        }
        // else if (inRangeArray.Contains(_currentTarget))
        // {
        //     ShootTarget();
        // }
        else
        {
            SetCurrentTarget(targetsArray);
            if (!DOTween.IsTweening(_turretTransform.gameObject))
            {
                _turretTransform.DOLookAt(_targetTransform.position, turretStats.TurnTime);
            }
            ShootTarget();
        }
    }

    private void TurretIdle()
    {
        if (DOTween.IsTweening(_turretTransform)) return;
        if (_turretTransform.forward != new Vector3(0, 0, 1))
        {
            _turretTransform.DORotate(Vector3.zero, turretStats.TurnTime);
        }
        else
        {
            _turretActionState = TurretActionState.Idle;
            var leftDirection = Quaternion.AngleAxis(-45, transform.up);
            var rightDirection = Quaternion.AngleAxis(+45, transform.up);
            _turretTransform.DORotate(leftDirection.eulerAngles, turretStats.TurnTime).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    _turretTransform.DORotate(rightDirection.eulerAngles, turretStats.TurnTime).SetLoops(2, LoopType.Yoyo);
                });
        }
       
    }

    private void SetCurrentTarget(Collider[] targets)
    {
        _turretActionState = TurretActionState.Targeting;
        var targetDistances = new List<float>();
        foreach (var target in targets)
        {
            target.TryGetComponent<Transform>(out var targetTransform);
            var targetDistance = Vector3.Distance(_turretTransform.position, targetTransform.position);
            targetDistances.Add(targetDistance);
        }
        _currentTarget = targets[targetDistances.IndexOf(targetDistances.Min())];
        _currentTarget.TryGetComponent(out _targetTransform);
    }

    private void ShootTarget()
    {
        Debug.Log(TargetInCone());
    }

    private bool TargetInCone()
    {
        var targetDirection = (_targetTransform.position - _turretTransform.position).normalized;
        var leftDirection = (Quaternion.AngleAxis(-45, transform.up)*_turretTransform.forward * turretStats.FiringRadius).normalized;
        var rightDirection = (Quaternion.AngleAxis(+45, transform.up)*_turretTransform.forward * turretStats.FiringRadius).normalized;
       
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
    private void FiringDirection()
    {
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(_turretTransform.position, turretStats.TargetingRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_turretTransform.position, turretStats.FiringRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_turretTransform.position, _turretTransform.forward);
        Gizmos.color = Color.green;
        var leftDirection = Quaternion.AngleAxis(-45, transform.up);
        var rightDirection = Quaternion.AngleAxis(+45, transform.up);
        Gizmos.DrawRay(_turretTransform.position, leftDirection * _turretTransform.forward * turretStats.FiringRadius);
        Gizmos.DrawRay(_turretTransform.position, rightDirection * _turretTransform.forward * turretStats.FiringRadius);
    }
}