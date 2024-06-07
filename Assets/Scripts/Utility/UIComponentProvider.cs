using UnityEngine;

public class UIComponentProvider : SingletonInScene<UIComponentProvider>
{
    [field: SerializeField] public DynamicInventoryDisplay PlayerInventoryDisplay { get; private set; }
    [field: SerializeField] public DynamicInventoryDisplay CustomContainerDisplay { get; private set; }
    [field: SerializeField] public PlayerHotbarInventoryDisplay PlayerHotbar { get; private set; }
    [field: SerializeField] public InventoryUIController InventoryUIController { get; private set; }
    [field: SerializeField] public CraftUIController CraftUIController { get; private set; }
    [field: SerializeField] public ItemDescriptionWindow ItemDescriptionWindow { get; private set; }
}
