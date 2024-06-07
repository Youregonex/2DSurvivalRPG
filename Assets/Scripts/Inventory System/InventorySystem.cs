using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> _inventorySlots;

    public List<InventorySlot> InventorySlots => _inventorySlots;

    public int InventorySize => _inventorySlots.Count;

    public InventorySystem(int size)
    {
        _inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            _inventorySlots.Add(new InventorySlot());
        }
    }

    public int AddItemToInventory(ItemDataSO newItem, int quantity)
    {
        if(ContainsItem(newItem, out List<InventorySlot> sameItemSlots)) // If inventory cointains same item -> get List of slots, containing this item
        {
            foreach (InventorySlot slot in sameItemSlots)
            {
                if(slot.EnoughRoomLeftInStack(quantity)) // If we can add whole item quantity to already existing stack -> update slot item quantity
                {
                    slot.AddToStack(quantity);

                    EventBus.Instance.InventorySlotChanged(slot);

                    return 0;
                }
                else if(!slot.EnoughRoomLeftInStack(quantity, out int slotCanAdd)) // If we can't add whole item quantity -> add what we can, reduce quantity
                {
                    if (slotCanAdd == 0)
                        continue;

                    slot.AddToStack(slotCanAdd);
                    quantity -= slotCanAdd;

                    EventBus.Instance.InventorySlotChanged(slot);
                }
            }
        }

        while (quantity > 0 && HasFreeSlot(out InventorySlot freeSlot)) // If we didnt find slots with same item/all slots with same item are full -> add item to free slot
        {
            if (newItem.MaxStackSize >= quantity) // If we can add whole item quantity to one slot -> update slot
            {
                freeSlot.UpdateInventorySlot(newItem, quantity);

                EventBus.Instance.InventorySlotChanged(freeSlot);

                return 0;
            }
            else
            {
                freeSlot.UpdateInventorySlot(newItem, newItem.MaxStackSize); // Add amount that slot can hold
                quantity -= newItem.MaxStackSize;

                EventBus.Instance.InventorySlotChanged(freeSlot);
            }
        }

        return quantity;
    }

    public void DecreaseSlotQuantity(InventorySlot slotToModify, int amount = 1)
    {
        foreach (InventorySlot slot in _inventorySlots)
        {
            if (slot == slotToModify)
            {
                slot.UpdateInventorySlot(slot.Item, slot.ItemQuantity - amount);

                if (slot.ItemQuantity <= 0)
                    slot.ClearSlot();

                EventBus.Instance.InventorySlotChanged(slot);

                return;
            }

        }
       
    }

    public bool ContainsItemWithQuantity(ItemDataSO item, int quantityNeeded)
    {
        if (ContainsItem(item, out List<InventorySlot> inventorySlots))
        {
            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.ItemQuantity >= quantityNeeded)
                {
                    return true;
                }
                else
                {
                    quantityNeeded -= slot.ItemQuantity;
                }
            }
        }

        return false;
    }

    public bool RemoveItem(ItemDataSO item, int quantity)
    {
        if(ContainsItem(item, out List<InventorySlot> inventorySlots))
        {
            int inventoryHasItemAmount = 0;

            foreach (InventorySlot slot in inventorySlots)
            {
                inventoryHasItemAmount += slot.ItemQuantity;
            }

            if (inventoryHasItemAmount < quantity)
                return false;

            foreach (InventorySlot slot in inventorySlots)
            {
                if(slot.ItemQuantity >= quantity)
                {
                    slot.RemoveFromStack(quantity);

                    if (slot.ItemQuantity == 0)
                        slot.ClearSlot();

                    EventBus.Instance.InventorySlotChanged(slot);

                    return true;
                }
                else
                {
                    quantity -= slot.ItemQuantity;
                    slot.ClearSlot();

                    EventBus.Instance.InventorySlotChanged(slot);
                }
            }
        }

        return false;
    }

    public bool ContainsItem(ItemDataSO item, out List<InventorySlot> inventorySlots)
    {
        inventorySlots = _inventorySlots.Where(i => i.Item == item).ToList();

        return inventorySlots == null ? false : true;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = _inventorySlots.FirstOrDefault(i => i.Item == null);

        return freeSlot == null ? false : true;
    }
}