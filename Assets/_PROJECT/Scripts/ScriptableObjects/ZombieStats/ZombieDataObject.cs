using UnityEngine;

[CreateAssetMenu(fileName = "ZombieDataObject", menuName = "Scriptable Objects/ZombieDataObject")]
public class ZombieDataObject : ScriptableObject
{
    [SerializeField] private float healthMax, damage, stoppingDistance, moveSpeed;
    
    public float HealthMax => healthMax;
    public float Damage => damage;
    public float StoppingDistance => stoppingDistance;
    public float MoveSpeed => moveSpeed;
    public float TurnSpeed => moveSpeed * 35;
    public float Acceleration => moveSpeed * 2.5f;
  
}
