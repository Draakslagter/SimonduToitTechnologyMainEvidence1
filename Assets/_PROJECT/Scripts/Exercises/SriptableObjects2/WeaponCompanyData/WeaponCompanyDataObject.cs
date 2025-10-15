using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WeaponCompanyDataObject", menuName = "Scriptable Objects 2/WeaponCompanyDataObject")]
public class WeaponCompanyDataObject : ScriptableObject
{
    [Header("WeaponData")]
    public GunCompanyName gunCompanyName;
    public ElementalDamageType preferredElementalDamageType;

    public float gunDamage;
    public float gunAccuracy;
    public float gunBarrelAccuracy;
    public float gunReloadSpeed;
    public float gunStability;
    public int gunMagazineSize;
    public int gunAmmoCapacity;
    public float gunScopeZoom;
    public float gunFiringSpeed;
    public float gunElementalDamage;

    [Header("Modifiers")]
    public float gunPositiveModifier;
    public float gunNegativeModifier;
    
    [Header("Misc")]
    public float gunModifierThreshold;
}

public enum ElementalDamageType
{
    Fire, Ice, Acid, Electric, Explosion, Slag
}

public enum GunCompanyName
{
    Jakobs, Maliwan, Hyperion, Bandit, Torgue, Tediore, Dahl, Vladof, Atlas
}