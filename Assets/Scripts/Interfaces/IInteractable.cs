using UnityEngine;

public interface IInteractable
{
    public void Interact(GameObject initializer);
    public void StopInteracting(GameObject initializer);
    public void HighlightInteractableObject();
    public void DeHighlightInterractableObject();
}
