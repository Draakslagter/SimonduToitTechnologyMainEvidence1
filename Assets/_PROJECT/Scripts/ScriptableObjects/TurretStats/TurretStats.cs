using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretStats", menuName = "Scriptable Objects/TurretStats")]
public class TurretStats : ScriptableObject
{
    [SerializeField] private float cost, health, healthMax, targetingRadius, turnTime, firingRadius, damage,reloadTime;
    [SerializeField] private Ease turretMoveEase;
    
    public float Cost => cost;
    public float Health { get => health; set => health = value; }
    public float HealthMax => healthMax;
    public float TargetingRadius => targetingRadius;
    public float TurnTime => turnTime;
    public float FiringRadius => firingRadius;
    public float Damage => damage;
    public float ReloadTime => reloadTime;
    public Ease TurretMoveEase => turretMoveEase;
}
