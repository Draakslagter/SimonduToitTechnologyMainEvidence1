using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGunDisplay : MonoBehaviour
{
  [SerializeField] private List<TMP_Text> weaponUITextList;
  [SerializeField] private GameObject gridParent;

  private void Awake()
  {
    PopulateUITextList();
  }
  private void Start()
  {
    WeaponGenerationManager.Instance.onWeaponFinalised.AddListener(DisplayWeaponData);
  }

  private void OnDisable()
  {
    WeaponGenerationManager.Instance.onWeaponFinalised.RemoveListener(DisplayWeaponData);
  }

  void PopulateUITextList()
  {
    if (weaponUITextList.Count > 0) return;

    foreach (var text in gridParent.GetComponentsInChildren<TMP_Text>())
    {
      weaponUITextList.Add(text);
    }
  }
  
  private void DisplayWeaponData(WeaponData weaponData)
  {
    ClearListTexts();
    
    weaponUITextList[0].text = $"Company Name: {weaponData.gunCompanyName}";
    weaponUITextList[1].text = $"Weapon Type: {weaponData.gunGunBodyType}";
    
  }
  void ClearListTexts()
  {
    foreach(var text in gridParent.GetComponentsInChildren<TMP_Text>())
    {
      text.text = "";
    }
  }
}
