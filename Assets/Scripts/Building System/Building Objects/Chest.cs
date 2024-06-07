using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class Chest : ContainerBuilding, IInteractable
{
    [SerializeField] private Sprite _chestClosed;
    [SerializeField] private Sprite _chestOpened;
    [SerializeField] private Color _interactColor;

    private Color _baseColor;
    private bool _isBeingUsed = false;

    private void Start()
    {
        _baseColor = _spriteRenderer.color;
    }

    public void Interact(GameObject initializer)
    {
        if (_isBeingUsed)
        {
            StopInteracting(initializer);
            return;
        }

        if (initializer.GetComponent<IInitializeInteraction>() == null)
            return;

        _isBeingUsed = true;
        _spriteRenderer.sprite = _chestOpened;

        if (initializer.GetComponent<IPlayer>() != null)
        {
            EventBus.Instance.DynamicInventoryDisplayRequested(_containerInventory.PrimaryInventorySystem);
        }
    }

    public void StopInteracting(GameObject initializer)
    {
        _isBeingUsed = false;
        _spriteRenderer.sprite = _chestClosed;
        EventBus.Instance.DynamicInventoryHideRequested();
    }

    public void HighlightInteractableObject()
    {
        _spriteRenderer.color = _interactColor;
    }

    public void DeHighlightInterractableObject()
    {
        _spriteRenderer.color = _baseColor;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IPlayer>() != null)
            DeHighlightInterractableObject();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IPlayer>() != null)
            HighlightInteractableObject();
    }
}