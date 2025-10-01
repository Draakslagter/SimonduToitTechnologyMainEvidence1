using UnityEngine;

[CreateAssetMenu(fileName = "characterStats", menuName = "Custom/characterStats")]
public class CharacterStats : ScriptableObject
{
    [SerializeField] private float health, healthMax, damage, speedMultiplier, jumpMultiplier;
    
    public float Health { get => health; set => health = value; }
    public float HealthMax => healthMax;
    public float Damage => damage;
    public float SpeedMultiplier => speedMultiplier;
    public float JumpMultiplier => jumpMultiplier;
}
