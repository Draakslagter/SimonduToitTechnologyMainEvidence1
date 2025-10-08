using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum TurretBuildState {Inactive, Active, Built}
public enum TurretActionState {Idle, Reloading, Firing}
public class TurretBehaviour : MonoBehaviour, IInteractible
{
    private static readonly int IsVisible = Shader.PropertyToID("_isVisible");
    private static readonly int FresnelActive = Shader.PropertyToID("_fresnelActive");
    
    [SerializeField] private TurretBuildState _turretBuildState = TurretBuildState.Inactive;
    private TurretActionState _turretActionState = TurretActionState.Idle;
    
    [SerializeField] private Renderer[] materialRenderers;
    
    public static Action<string> TriggerPreInteract;

    private void Awake()
    {
        PlayerMovementAndControlSetup.TriggerClearPreInteract += ClearPreInteract;
    }

    private void OnDisable()
    {
        PlayerMovementAndControlSetup.TriggerClearPreInteract -= ClearPreInteract;
    }
    
    public void ChangeBuildState(TurretBuildState newState)
    {
        _turretBuildState = newState;
        
        gameObject.layer = newState switch
        {
            TurretBuildState.Active => 6,
            TurretBuildState.Built => 0,
            _ => gameObject.layer
        };
    }
    private void ActivateTurret(int visible)
    {
        foreach (var r in materialRenderers)
        {
            r.material.SetInt(IsVisible, visible);
        }
    }
    private void BuildTurret()
    {
        ChangeBuildState(TurretBuildState.Built);
        foreach (var r in materialRenderers)
        {
            r.material.SetInt(IsVisible, 1);
            r.material.SetInt( FresnelActive, 0);
        }
    }

    public void PreInteract()
    {
        if (_turretBuildState != TurretBuildState.Active) return;
        TriggerPreInteract.Invoke($"E - Build Turret");
        ActivateTurret(1);
    }

    private void ClearPreInteract()
    {
        if (_turretBuildState != TurretBuildState.Active) return;
            ActivateTurret(0);
    }

    public void Interact()
    {
        if (_turretBuildState != TurretBuildState.Active) return;
        BuildTurret();
    }
}
