using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private ItemDataSO _item;
    [SerializeField] private int _itemQuantity;

    public ItemDataSO Item => _item;
    public int ItemQuantity => _itemQuantity;

    public InventorySlot(ItemDataSO item, int quantity)
    {
        _item = item;
        _itemQuantity = quantity;
    }

    public InventorySlot()
    {
        ClearSlot();
    }

    public void ClearSlot()
    {
        _item = null;
        _itemQuantity = -1;
    }

    public void AssignItem(InventorySlot slotToAssign)
    {
        if (_item == slotToAssign.Item)
            AddToStack(slotToAssign.ItemQuantity);
        else
        {
            _item = slotToAssign.Item;
            _itemQuantity = 0;
            AddToStack(slotToAssign.ItemQuantity);
        }
    }

    public bool EnoughRoomLeftInStack(int amountToAdd, out int slotCanAdd)
    {
        slotCanAdd = _item.MaxStackSize - _itemQuantity;

        return EnoughRoomLeftInStack(amountToAdd);
    }

    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        if (_item == null || _item != null && _itemQuantity + amountToAdd <= _item.MaxStackSize)
            return true;

        return false;
    }

    public void UpdateInventorySlot(ItemDataSO item, int quantity)
    {
        _item = item;
        _itemQuantity = quantity;
    }

    public void AddToStack(int amount)
    {
        _itemQuantity += amount;
    }

    public void RemoveFromStack(int amount)
    {
        _itemQuantity -= amount;
    }

    public bool SlotIsFull() => _itemQuantity == _item.MaxStackSize;

    public bool SplitStack(out InventorySlot splitStack)
    {
        if (_itemQuantity <= 1)
        {
            splitStack = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(_itemQuantity / 2);
        RemoveFromStack(halfStack);

        splitStack = new InventorySlot(_item, halfStack);

        return true;
    }
}