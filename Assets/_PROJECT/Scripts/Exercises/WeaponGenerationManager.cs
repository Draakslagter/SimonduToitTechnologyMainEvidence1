using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;
using Random = UnityEngine.Random;

public class WeaponGenerationManager : MonoBehaviour
{
    private static WeaponGenerationManager _instance;
    public static WeaponGenerationManager Instance => _instance;
    [Header ("Lists of Data Objects")]
    [SerializeField] private List<WeaponCompanyDataObject> listOfCompanies;
    [SerializeField] private List<WeaponBodyDataObject> listOfWeaponBodies;
    
    [Header ("Selected Data Objects")]
    private WeaponCompanyDataObject _selectedCompany;
    private WeaponBodyDataObject _selectedWeaponObject;

    [Header("Random Value Holders")] 
    private int _randomCompanyNumber;
    private int _randomWeaponNumber;
    private int _gripNumber;
    
    public WeaponData finalWeapon;
    public UnityEvent<WeaponData> onWeaponFinalised;

    private void Awake()
    {
        _instance = this;
    }
    public void GenerateWeapon()
    {
        _randomCompanyNumber = Random.Range(0, listOfCompanies.Count);
        _selectedCompany = listOfCompanies[_randomCompanyNumber];
        GenerateWeaponData();
        onWeaponFinalised.Invoke(finalWeapon);
    }

    private void GenerateWeaponData()
    {
        finalWeapon = new WeaponData();
        
        _randomWeaponNumber = Random.Range(0, listOfWeaponBodies.Count);
        _selectedWeaponObject = listOfWeaponBodies[_randomWeaponNumber];

        finalWeapon.gunGunBodyType = (GunBodyType)_randomWeaponNumber;
        _gripNumber = Random.Range(0, 6);

        if (_gripNumber == 0)
        {
            finalWeapon.gunGunBodyType = GunBodyType.Pistol;
        }

        finalWeapon.gunCompanyName = _selectedCompany.gunCompanyName;
        finalWeapon.gunDamage = _selectedCompany.gunDamage;
        finalWeapon.gunAccuracy = _selectedCompany.gunAccuracy;
        finalWeapon.gunStability = _selectedCompany.gunStability;
        finalWeapon.gunAmmoCapacity = _selectedCompany.gunAmmoCapacity;
        finalWeapon.gunMagazineSize = _selectedCompany.gunMagazineSize;
        finalWeapon.gunBarrelAccuracy = _selectedCompany.gunBarrelAccuracy;
        finalWeapon.gunReloadSpeed = _selectedCompany.gunReloadSpeed;
        finalWeapon.gunScopeZoom = _selectedCompany.gunScopeZoom;
        finalWeapon.gunFiringSpeed = _selectedCompany.gunFiringSpeed;

        switch (finalWeapon.gunCompanyName)
        {
            case GunCompanyName.Bandit:
                finalWeapon.gunMagazineSize *= (int)_selectedCompany.gunPositiveModifier;
                break;
            case GunCompanyName.Jakobs:
                
                break;
            default:
                finalWeapon.gunCriticalChance = 0;
                finalWeapon.gunCriticalDamageModifier = 0;
                finalWeapon.isCriticalWeapon = false;
                break;
        }

        CalculateElementalDamage();
        ApplySmallModifiers();
    }

    private void CalculateElementalDamage()
    {
        var percentageCheck = Random.value;
      ElementalDamageType elementalDamageTypeHolder;

      switch (finalWeapon.gunCompanyName)
      {
          case GunCompanyName.Jakobs:
              finalWeapon.isElementalWeapon = false;
              finalWeapon.gunElementalDamage = 0;
              return;
          case GunCompanyName.Maliwan:
              percentageCheck -= 0.2f;
              break;
          default:
              break;
      }

      if (percentageCheck < 0)
      {
          percentageCheck = 0;
      }

      if (percentageCheck >= finalWeapon.gunElementalChance)
      {
          finalWeapon.isElementalWeapon = true;
          finalWeapon.gunElementalDamage = _selectedCompany.gunElementalDamage;

          var randomElementSelector = Random.Range(0, Enum.GetValues(typeof(ElementalDamageType)).Length);
          var preferredElementalReRoll = Random.value;

          if (preferredElementalReRoll > 0.5f)
          {
              elementalDamageTypeHolder = _selectedCompany.preferredElementalDamageType;
          }
          else
          {
              elementalDamageTypeHolder = (ElementalDamageType)randomElementSelector;
          }
          
          finalWeapon.gunElementalDamageType = elementalDamageTypeHolder;
      }
    }
    private void ApplySmallModifiers()
    {
        if (_selectedCompany.gunMagazineSize > _selectedCompany.gunModifierThreshold)
        {
            finalWeapon.gunDamage -= (_selectedCompany.gunNegativeModifier * _selectedCompany.gunDamage);
        }

        if (_selectedCompany.gunStability > _selectedCompany.gunModifierThreshold)
        {
            finalWeapon.gunReloadSpeed += (_selectedCompany.gunPositiveModifier * _selectedCompany.gunReloadSpeed);
        }

        if (_selectedCompany.gunAccuracy + _selectedCompany.gunBarrelAccuracy > _selectedCompany.gunModifierThreshold)
        {
            finalWeapon.gunDamage += (_selectedCompany.gunPositiveModifier * _selectedCompany.gunDamage);
        }

        if (finalWeapon.gunGunBodyType == GunBodyType.Shotgun)
        {
            finalWeapon.gunDamage -= (finalWeapon.gunDamage / finalWeapon.gunBulletCount);
        }
    }
}



[System.Serializable]
public class WeaponData
{
    public GunCompanyName gunCompanyName;
    public GunBodyType gunGunBodyType;
    public ElementalDamageType gunElementalDamageType;
    
    public float gunDamage, gunAccuracy, gunBarrelAccuracy, gunReloadSpeed;
    public float gunStability;
    public float gunScopeZoom;
    public float gunFiringSpeed;
    public float gunElementalChance;
    public float gunElementalDamage;
    public float gunCriticalChance;
    public float gunCriticalDamageModifier;
    
    public int gunBulletCount;
    public int gunMagazineSize;
    public int gunAmmoCapacity;
    
    public bool isCriticalWeapon;
    public bool isElementalWeapon;
}
