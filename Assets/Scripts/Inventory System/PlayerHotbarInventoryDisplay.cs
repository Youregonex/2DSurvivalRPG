using UnityEngine;

public class PlayerHotbarInventoryDisplay : InventoryDisplay, IEventListener
{
    [SerializeField] private InventoryHolder _inventoryHolder;
    [SerializeField] private PlayerHotbarSlotUI[] _hotbarUISlots;

    private PlayerHotbarSlotUI _currentlySelectedUISlot;

    protected override void Awake()
    {
        base.Awake();
        SubscribeToEvents();
    }

    private void Start()
    {
        if(_inventoryHolder != null)
        {
            _inventorySystem = _inventoryHolder.PrimaryInventorySystem;
        }
        else
        {
            Debug.LogWarning($"No inventory assigned to {gameObject}");
        }

        AssignSlots(_inventorySystem);
        SlectHotbarUISlot(0);
    }

    public override void AssignSlots(InventorySystem inventoryToDisplay)
    {
        _slotDictionary = new();

        if (_hotbarUISlots.Length != _inventorySystem.InventorySize)
            Debug.Log($"Inventory slots are out of sync on {gameObject}");

        for (int i = 0; i < _inventorySystem.InventorySize; i++)
        {
            _slotDictionary.Add(_hotbarUISlots[i], _inventorySystem.InventorySlots[i]);
            _hotbarUISlots[i].InitializeSlot(_inventorySystem.InventorySlots[i]);
        }
    }

    protected override void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot)
            {
                slot.Key.UpdateUISlot(updatedSlot);

                if(updatedSlot == _currentlySelectedUISlot.AssignedInventorySlot)
                    EventBus.Instance.ItemSelected(_currentlySelectedUISlot.AssignedInventorySlot);

                return;
            }
        }
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SlectHotbarUISlot(int slotNumber)
    {
        if (slotNumber > _hotbarUISlots.Length || _hotbarUISlots[slotNumber] == null)
            return;

        if (_currentlySelectedUISlot != null)
            _currentlySelectedUISlot.DeselectSlot();

        _hotbarUISlots[slotNumber].SelectSlot();

        _currentlySelectedUISlot = _hotbarUISlots[slotNumber];

        EventBus.Instance.ItemSelected(_hotbarUISlots[slotNumber].AssignedInventorySlot);
    }

    public void SubscribeToEvents()
    {
        EventBus.Instance.OnInventorySlotChanged += UpdateSlot;
        EventBus.Instance.OnHotbarSlotSelected += SlectHotbarUISlot;
    }

    public void UnsubscribeFromEvents()
    {
        EventBus.Instance.OnInventorySlotChanged -= UpdateSlot;
        EventBus.Instance.OnHotbarSlotSelected -= SlectHotbarUISlot;
    }
}