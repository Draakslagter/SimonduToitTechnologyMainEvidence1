using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WeaponBodyDataObject", menuName = "Scriptable Objects 2/WeaponBodyDataObject")]
public class WeaponBodyDataObject : ScriptableObject
{
    public GunBodyType weaponGunBodyType;
    public float fireRate;
    public float bodyAccuracy;
    public int magazineSize;
    
}

public enum GunBodyType
{
    Pistol, Shotgun, SMG, Assault_Rifle, Sniper_Rifle, RPG 
}