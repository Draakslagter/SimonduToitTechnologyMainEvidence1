using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    [SerializeField] private float health, healthMax, damage, moveSpeedMultiplier, jumpHeightMultiplier;
    
    public float Health { get => health; set => health = value; }
    public float HealthMax => healthMax;
    public float Damage => damage;
    public float MoveSpeedMultiplier => moveSpeedMultiplier;
    public float JumpHeightMultiplier => jumpHeightMultiplier;
}
