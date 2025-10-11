using UnityEngine;

public interface IInteractible
{
    public void PreInteract();
    public void ClearPreInteract();
    public void Interact();
    
    public void UIInteract(int index);
}
