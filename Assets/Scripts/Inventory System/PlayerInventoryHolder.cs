using UnityEngine;

public class PlayerInventoryHolder : InventoryHolder, IEventListener
{
    [SerializeField] protected int _secondaryInventorySize;
    [SerializeField] protected InventorySystem _secondaryInventorySystem;

    public InventorySystem SecondaryInventorySystem => _secondaryInventorySystem;

    protected override void Awake()
    {
        base.Awake();

        _secondaryInventorySystem = new InventorySystem(_secondaryInventorySize);
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    public void RequestPlayerInventoryUI()
    {
        EventBus.Instance.PlayerInventoryDisplayRequested();
    }

    public int AddToInventory(ItemDataSO item, int quantity)
    {
        int quantityRemaining = _primaryInventorySystem.AddItemToInventory(item, quantity);

        if (quantityRemaining == 0)
        {
            EventBus.Instance.InventoryChanged();
            return 0;
        }
        else
        {
            quantityRemaining = _secondaryInventorySystem.AddItemToInventory(item, quantityRemaining);

            if(quantityRemaining == 0)
            {
                EventBus.Instance.InventoryChanged();
                return 0;
            }
        }

        if(quantityRemaining != quantity)
            EventBus.Instance.InventoryChanged();

        return quantityRemaining;
    }

    public bool RemoveFromInventory(ItemDataSO item, int quantity)
    {
        InventorySystem combinedPlayerInventories = GetCombinedInventory();

        if (!combinedPlayerInventories.RemoveItem(item, quantity))
            return false;

        SplitCombinedInventory(combinedPlayerInventories);

        EventBus.Instance.InventoryChanged();

        return true;
    }

    public void DecreaseSlotQuantity(InventorySlot slotToModify, int amount = 1)
    {
        InventorySystem combinedPlayerInventories = GetCombinedInventory();

        combinedPlayerInventories.DecreaseSlotQuantity(slotToModify, amount);

        SplitCombinedInventory(combinedPlayerInventories);

        EventBus.Instance.InventoryChanged();        
    }

    public bool ContainsCraftComponent(CraftingComponent component)
    {
        InventorySystem combinedPlayerInventories = GetCombinedInventory();

        bool containsItem = combinedPlayerInventories.ContainsItemWithQuantity(component.Item, component.quantityRequired);

        return containsItem;
    }

    private InventorySystem GetCombinedInventory() // Get PlayerInventory + Hotbar
    {
        InventorySystem combinedPlayerInventories = new InventorySystem(_primaryInventorySize + _secondaryInventorySize);

        for (int i = 0; i < combinedPlayerInventories.InventorySize; i++)
        {
            if (i < _primaryInventorySize)
            {
                combinedPlayerInventories.InventorySlots[i] = _primaryInventorySystem.InventorySlots[i];
            }
            else
            {
                combinedPlayerInventories.InventorySlots[i] = _secondaryInventorySystem.InventorySlots[i - _primaryInventorySize];
            }
        }

        return combinedPlayerInventories;
    }

    private void SplitCombinedInventory(InventorySystem combinedPlayerInventories) // Updates Player Inventory + Hotbar data with combined inventory data
    {
        bool slotDidntChange;

        for (int i = 0; i < combinedPlayerInventories.InventorySize; i++)
        {
            if (i < _primaryInventorySize)
            {
                slotDidntChange = _primaryInventorySystem.InventorySlots[i] == combinedPlayerInventories.InventorySlots[i];

                if (slotDidntChange)
                    continue;

                _primaryInventorySystem.InventorySlots[i].UpdateInventorySlot(combinedPlayerInventories.InventorySlots[i].Item,
                                                                              combinedPlayerInventories.InventorySlots[i].ItemQuantity);
            }
            else
            {
                slotDidntChange = _secondaryInventorySystem.InventorySlots[i - _primaryInventorySize] == combinedPlayerInventories.InventorySlots[i];

                if (slotDidntChange)
                    continue;

                _secondaryInventorySystem.InventorySlots[i - _primaryInventorySize].UpdateInventorySlot(combinedPlayerInventories.InventorySlots[i].Item,
                                                                                                        combinedPlayerInventories.InventorySlots[i].ItemQuantity);
            }
        }
    }

    public void SubscribeToEvents()
    {
        EventBus.Instance.OnInventoryKeyPressed += RequestPlayerInventoryUI;
    }

    public void UnsubscribeFromEvents()
    {
        EventBus.Instance.OnInventoryKeyPressed -= RequestPlayerInventoryUI;
    }
}