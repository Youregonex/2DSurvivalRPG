using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private MouseItem _mouseInventoryItem;

    protected InventorySystem _inventorySystem;
    protected Dictionary<InventorySlotUI, InventorySlot> _slotDictionary;
    protected InventoryUIController _inventoryUIController;

    public InventorySystem InventorySystem => _inventorySystem;
    public Dictionary<InventorySlotUI, InventorySlot> SlotDictionary => _slotDictionary;

    protected virtual void Awake()
    {
        _inventoryUIController = UIComponentProvider.Instance.InventoryUIController;
    }

    public abstract void AssignSlots(InventorySystem inventoryToDisplay);

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if(slot.Value == updatedSlot)
            {
                slot.Key.UpdateUISlot(updatedSlot);
                return;
            }
        }
    }

    public void SlotClicked(InventorySlotUI clickedUISlot)
    {
        bool isCtrlKeyPressed = Keyboard.current.leftCtrlKey.isPressed;
        bool isShiftKeyPressed = Keyboard.current.leftShiftKey.isPressed;

        if(clickedUISlot.AssignedInventorySlot.Item != null && _mouseInventoryItem.AssignedInventorySlot.Item == null) // Not holding an item and click on slot with item
        {
            if(isShiftKeyPressed)
            {
                List<InventorySystem> openedInventories = _inventoryUIController.GetOpenedInventories();

                foreach (InventorySystem inventory in openedInventories) // If holding shift -> add click item to other inventory if any opened
                {
                    if (inventory == _inventorySystem)
                        continue;

                    InventorySlot slot = clickedUISlot.AssignedInventorySlot;

                    int inventoryCantFit = inventory.AddItemToInventory(slot.Item, slot.ItemQuantity);

                    if (inventoryCantFit == slot.ItemQuantity) // Inventory is full
                        continue;

                    if(inventoryCantFit == 0)
                    {
                        clickedUISlot.ClearSlot();

                        EventBus.Instance.InventorySlotChanged(clickedUISlot.AssignedInventorySlot);
                        EventBus.Instance.InventoryUIItemClick();

                        return;
                    }
                    else
                    {
                        InventorySlot newSlot = new(clickedUISlot.AssignedInventorySlot.Item, inventoryCantFit);

                        clickedUISlot.AssignedInventorySlot.UpdateInventorySlot(newSlot.Item, newSlot.ItemQuantity);
                        clickedUISlot.UpdateUISlot();

                        EventBus.Instance.InventorySlotChanged(clickedUISlot.AssignedInventorySlot);
                        EventBus.Instance.InventoryUIItemClick();

                        return;
                    }
                }
            }

            if(isCtrlKeyPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot)) // If ctrl is pressed -> split stack. Hold half-stack item
            {
                _mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();

                EventBus.Instance.InventorySlotChanged(clickedUISlot.AssignedInventorySlot);
                EventBus.Instance.InventoryUIItemClick();
                EventBus.Instance.PickItemInHand();

                return;
            }
            else
            {
                _mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot); // Take item from slot to hand
                clickedUISlot.ClearSlot();

                EventBus.Instance.InventorySlotChanged(clickedUISlot.AssignedInventorySlot);
                EventBus.Instance.InventoryUIItemClick();
                EventBus.Instance.PickItemInHand();

                return;
            }
        }

        if(clickedUISlot.AssignedInventorySlot.Item == null && _mouseInventoryItem.AssignedInventorySlot.Item != null) // Click empty slot with item in hands
        {
            if (isCtrlKeyPressed) // If ctrl pressed -> add 1 of item stack to the slot
            {
                InventorySlot newSlotItem = new(_mouseInventoryItem.AssignedInventorySlot.Item, 1);
                clickedUISlot.AssignedInventorySlot.AssignItem(newSlotItem);
                clickedUISlot.UpdateUISlot();

                InventorySlot newMouseItem = new(_mouseInventoryItem.AssignedInventorySlot.Item, _mouseInventoryItem.AssignedInventorySlot.ItemQuantity - 1);
                _mouseInventoryItem.ClearSlot();
                _mouseInventoryItem.UpdateMouseSlot(newMouseItem);

                if (_mouseInventoryItem.AssignedInventorySlot.ItemQuantity == 0)
                {
                    _mouseInventoryItem.ClearSlot();
                    EventBus.Instance.ReleaseItemInHand();
                }

                EventBus.Instance.InventorySlotChanged(clickedUISlot.AssignedInventorySlot);
                EventBus.Instance.InventoryUIItemClick();

                return;
            }
            else
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();

                _mouseInventoryItem.ClearSlot();

                EventBus.Instance.InventorySlotChanged(clickedUISlot.AssignedInventorySlot);
                EventBus.Instance.InventoryUIItemClick();
                EventBus.Instance.ReleaseItemInHand();

                return;
            }
        }

        if (clickedUISlot.AssignedInventorySlot.Item != null && _mouseInventoryItem.AssignedInventorySlot.Item != null) // Click not empty slot with item in hands
        {
            bool isSameItem = clickedUISlot.AssignedInventorySlot.Item == _mouseInventoryItem.AssignedInventorySlot.Item;

            InventorySlot clickedSlotData = clickedUISlot.AssignedInventorySlot;

            bool slotIsFull = clickedSlotData.SlotIsFull();

            if(isCtrlKeyPressed && !slotIsFull && isSameItem) // If ctrl pressed add 1 item to stack
            {
                clickedUISlot.AssignedInventorySlot.AddToStack(1);
                clickedUISlot.UpdateUISlot();

                InventorySlot newMouseSlot = new(_mouseInventoryItem.AssignedInventorySlot.Item, _mouseInventoryItem.AssignedInventorySlot.ItemQuantity - 1);
                _mouseInventoryItem.SetNewAssignedSlot(newMouseSlot);

                if (_mouseInventoryItem.AssignedInventorySlot.ItemQuantity == 0)
                    _mouseInventoryItem.ClearSlot();

                EventBus.Instance.InventorySlotChanged(clickedUISlot.AssignedInventorySlot);
                EventBus.Instance.InventoryUIItemClick();

                return;
            }

            if (isSameItem && clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(_mouseInventoryItem.AssignedInventorySlot.ItemQuantity)) // Item in slot and in hand are same and we can add whole hand item -> add to slot
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();

                _mouseInventoryItem.ClearSlot();

                EventBus.Instance.InventorySlotChanged(clickedUISlot.AssignedInventorySlot);
                EventBus.Instance.InventoryUIItemClick();
                EventBus.Instance.ReleaseItemInHand();

                return;
            }
            else if(isSameItem &&
                    !clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(_mouseInventoryItem.AssignedInventorySlot.ItemQuantity, out int stackCanFit)) // Item in slot and in hand are same but we can't add whole hand item
            {
                if (stackCanFit < 1) // Slot full
                {
                    if (isCtrlKeyPressed)
                        return;

                    SwapSlots(clickedUISlot);

                    EventBus.Instance.InventorySlotChanged(clickedUISlot.AssignedInventorySlot);
                    EventBus.Instance.InventoryUIItemClick();

                    return;
                }
                else
                {
                    int remainingInMouseSlot = _mouseInventoryItem.AssignedInventorySlot.ItemQuantity - stackCanFit;
                    clickedUISlot.AssignedInventorySlot.AddToStack(stackCanFit);
                    clickedUISlot.UpdateUISlot();

                    InventorySlot newItem = new(_mouseInventoryItem.AssignedInventorySlot.Item, remainingInMouseSlot);
                    _mouseInventoryItem.ClearSlot();
                    _mouseInventoryItem.UpdateMouseSlot(newItem);

                    EventBus.Instance.InventorySlotChanged(clickedUISlot.AssignedInventorySlot);
                    EventBus.Instance.InventoryUIItemClick();

                    return;
                }
            }
            else if(!isSameItem)
            {
                SwapSlots(clickedUISlot);

                EventBus.Instance.InventorySlotChanged(clickedUISlot.AssignedInventorySlot);
                EventBus.Instance.InventoryUIItemClick();

                return;
            }
        }
    }

    private void SwapSlots(InventorySlotUI clickedUISlot)
    {
        InventorySlot clonedSlot = new(_mouseInventoryItem.AssignedInventorySlot.Item, _mouseInventoryItem.AssignedInventorySlot.ItemQuantity);
        _mouseInventoryItem.ClearSlot();

        _mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);

        clickedUISlot.ClearSlot();

        clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}