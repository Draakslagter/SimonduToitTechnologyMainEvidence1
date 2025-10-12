using System;
using UnityEngine;
using UnityEngine.Events;

public enum TurretBuildState {Inactive, Active, Built}
public class TurretSelector : MonoBehaviour, IInteractible
{
    private TurretBuildState _turretBuildState = TurretBuildState.Inactive;
    
    [SerializeField] private TurretBuilder[] turrets;
    private int _currentTurretIndex;
    
    public static Action<string> TriggerPreInteract;
    public UnityEvent<TurretBuildState> triggerBuild;
    
    private void Awake()
    {
        PlayerMovementAndControlSetup.TriggerClearPreInteract += ClearPreInteract;
    }

    private void Start()
    {
        foreach (var turretBuilder in turrets)
        {
            turretBuilder.ActivateTurret(0);
        }
    }
    private void OnDisable()
    {
        PlayerMovementAndControlSetup.TriggerClearPreInteract -= ClearPreInteract;
    }
    
    private void ChangeTurretIndex(int newIndex)
    {
        _currentTurretIndex += newIndex;
        Debug.Log(_currentTurretIndex);
        if (_currentTurretIndex >= turrets.Length)
        {
            Debug.Log("Turret Index out of range: Positive");
            _currentTurretIndex = 0;
        }
        if (_currentTurretIndex >= 0) return;
        Debug.Log("Turret Index out of range: Negative");
        _currentTurretIndex = turrets.Length - 1;
    }

    public void ChangeBuildState(TurretBuildState newState)
    {
        _turretBuildState = newState;
        triggerBuild.Invoke(_turretBuildState);
        
        gameObject.layer = newState switch
        {
            TurretBuildState.Active => 6,
            TurretBuildState.Built => 0,
            _ => gameObject.layer
        };
    }
    
    public void PreInteract()
    {
        if (_turretBuildState != TurretBuildState.Active) return;
        TriggerPreInteract.Invoke($"E - Build Turret\nF - Previous  // R - Next");
        turrets[_currentTurretIndex].ActivateTurret(1);
    }

    public void ClearPreInteract()
    {
        if (_turretBuildState != TurretBuildState.Active) return;
        turrets[_currentTurretIndex].ActivateTurret(0);
    }

    public void Interact()
    {
        if (_turretBuildState != TurretBuildState.Active) return;
        turrets[_currentTurretIndex].BuildTurret();
        ChangeBuildState(TurretBuildState.Built);
    }

    public void UIInteract(int index)
    {
        ChangeTurretIndex(index);
        foreach (var turret in turrets)
        {
            turret.ActivateTurret(0);
        }
        PreInteract();
    }
}
