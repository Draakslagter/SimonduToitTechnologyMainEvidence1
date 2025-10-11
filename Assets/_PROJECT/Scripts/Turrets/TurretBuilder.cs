using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum TurretActionState {Idle, Reloading, Firing}
public class TurretBuilder : MonoBehaviour
{
    private static readonly int IsVisible = Shader.PropertyToID("_isVisible");
    private static readonly int FresnelActive = Shader.PropertyToID("_fresnelActive");
    
    private TurretActionState _turretActionState = TurretActionState.Idle;
    
    [SerializeField] private Renderer[] materialRenderers;
    
    public void ActivateTurret(int visible)
    {
        foreach (var r in materialRenderers)
        {
            r.material.SetInt(IsVisible, visible);
        }
    }
    public void BuildTurret()
    {
        foreach (var r in materialRenderers)
        {
            r.material.SetInt(IsVisible, 1);
            r.material.SetInt( FresnelActive, 0);
        }
    }

    
}
