using System;
using UnityEngine;
using UnityEngine.Events;

public enum TreeState {Standing, Chopped}

public class TreeBehaviour : MonoBehaviour,  IInteractible
{
    private static readonly int DissolveAmount = Shader.PropertyToID("_dissolveAmount");
    private Renderer _materialRenderer;
    private int _chopCount;
    private TreeState _treeState = TreeState.Standing;

    public UnityEvent<TurretBuildState> triggerTreeChopped;
    public static Action<string> TriggerPreInteract;

    private void Start()
    {
        _materialRenderer = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        if (_treeState == TreeState.Standing) return;
        var tempFloat = _materialRenderer.material.GetFloat(DissolveAmount);
        tempFloat += 0.1f * Time.deltaTime;
        Debug.Log(tempFloat);
        _materialRenderer.material.SetFloat(DissolveAmount, tempFloat);
        if (!(tempFloat >= 0.5)) return;
        triggerTreeChopped.Invoke(TurretBuildState.Active);
        gameObject.SetActive(false);
    }

    public void PreInteract()
    {
        if (_treeState == TreeState.Chopped) return;
        TriggerPreInteract.Invoke($"E - CHOP");
    }

    public void Interact()
    {
        if (_chopCount >= 3)
        {
            _treeState = TreeState.Chopped;
            return;
        }
        _chopCount++;
        PlayerInventory.Instance.AddWood(1);
    }
}
