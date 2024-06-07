using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DynamicInventoryDisplay : InventoryDisplay, IEventListener
{
    [SerializeField] protected InventorySlotUI slotPrefab;

    public override void AssignSlots(InventorySystem inventoryToDisplay)
    {
        _slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        if (inventoryToDisplay == null)
            return;

        for (int i = 0; i < inventoryToDisplay.InventorySize; i++)
        {
            var uiSlot = Instantiate(slotPrefab, transform); //Object pulling
            _slotDictionary.Add(uiSlot, inventoryToDisplay.InventorySlots[i]);

            uiSlot.InitializeSlot(inventoryToDisplay.InventorySlots[i]);
            uiSlot.UpdateUISlot();
        }
    }

    public void RefreshDynamicInventory(InventorySystem inventoryToDisplay)
    {
        ClearSlots();
        _inventorySystem = inventoryToDisplay;

        if (_inventorySystem != null)
            SubscribeToEvents();

        AssignSlots(inventoryToDisplay);
    }

    public void SubscribeToEvents()
    {
        EventBus.Instance.OnInventorySlotChanged += UpdateSlot;
    }

    public void UnsubscribeFromEvents()
    {
        EventBus.Instance.OnInventorySlotChanged -= UpdateSlot;
    }

    private void ClearSlots()
    {
        foreach (var child in transform.Cast<Transform>())
        {
            Destroy(child.gameObject); //Object pulling
        }

        if (_slotDictionary != null)
            _slotDictionary.Clear();
    }

    private void OnDestroy()
    {
        if (_inventorySystem != null)
            UnsubscribeFromEvents();
    }
}
