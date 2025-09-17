using UnityEngine;

public class Tree : MonoBehaviour,  IInteractible
{
    private static readonly int DissolveAmount = Shader.PropertyToID("_dissolveAmount");
    private Renderer materialRenderer;
    private int chopCount;
    private bool empty;

    private void Start()
    {
        materialRenderer = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        if (!empty) return;
        var tempFloat = materialRenderer.material.GetFloat(DissolveAmount);
        tempFloat += 0.1f * Time.deltaTime;
        materialRenderer.material.SetFloat(DissolveAmount, tempFloat);
    }
    
    public void Interact()
    {
        Debug.Log("Chopping Trees");
        if (chopCount >= 3)
        {
            empty = true;
            return;
        }
        chopCount++;
        PlayerInventory.Instance.AddWood(1);
    }
}
