using System;
using TMPro;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI preInteractText;
    private void Awake()
    {
        TreeBehaviour.TriggerPreInteract += UpdatePreInteractText;
        TurretBehaviour.TriggerPreInteract += UpdatePreInteractText;
        PlayerMovementAndControlSetup.TriggerClearPreInteract += ClearPreInteractText;
    }

    private void OnDisable()
    {
        TreeBehaviour.TriggerPreInteract -= UpdatePreInteractText;
        TurretBehaviour.TriggerPreInteract -= UpdatePreInteractText;
        PlayerMovementAndControlSetup.TriggerClearPreInteract -= ClearPreInteractText;
    }

    private void UpdatePreInteractText(string text)
    {
        preInteractText.text = text;
    }

    private void ClearPreInteractText()
    {
        preInteractText.text = "";
    }
}
