using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MouseItem : MonoBehaviour
{
    [field: SerializeField] public Image ItemSprite { get; private set; }
    [field: SerializeField] public TextMeshProUGUI ItemQuantityText { get; private set; }

    private InventorySlot _assignedInventorySlot = new();
    public InventorySlot AssignedInventorySlot => _assignedInventorySlot;

    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemQuantityText.text = "";
    }

    public void UpdateMouseSlot(InventorySlot slotToDisplay)
    {
        _assignedInventorySlot.AssignItem(slotToDisplay);
        ItemSprite.sprite = slotToDisplay.Item.Icon;

        if(slotToDisplay.ItemQuantity > 1)
            ItemQuantityText.text = slotToDisplay.ItemQuantity.ToString();

        ItemSprite.color = Color.white;
    }

    public void SetNewAssignedSlot(InventorySlot slot)
    {
        _assignedInventorySlot = slot;
        ItemSprite.sprite = slot.Item.Icon;

        if (slot.ItemQuantity > 1)
            ItemQuantityText.text = slot.ItemQuantity.ToString();
        else
            ItemQuantityText.text = "";

        ItemSprite.color = Color.white;

    }

    private void Update()
    {
        if(_assignedInventorySlot.Item != null)
        {
            transform.position = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                ItemFactory.Instance.CreateItemAtPlayerPosition(_assignedInventorySlot.Item, _assignedInventorySlot.ItemQuantity);
                EventBus.Instance.ReleaseItemInHand();
                ClearSlot();
            }
        }
    }

    public void ClearSlot()
    {
        _assignedInventorySlot.ClearSlot();
        ItemQuantityText.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}
