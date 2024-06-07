using System.Collections.Generic;
using UnityEngine;

public class PlayerCraftingSystem : MonoBehaviour, IEventListener
{
    [SerializeField] private CraftingRecipeDatabase _recipeDatabase;

    private List<CraftingRecipeSO> _availableCraftingRecipes;

    private PlayerInventoryHolder _playerInventory;

    private void Awake()
    {
        _playerInventory = GetComponent<PlayerInventoryHolder>();
        _availableCraftingRecipes = _recipeDatabase.CraftingRecipes;

        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void RequestCraftMenuUpdate() => EventBus.Instance.CraftMenuUpdateRequest(_playerInventory, _availableCraftingRecipes);

    public void TryCraftingItem(CraftingRecipeSO craft, int quantity = 1)
    {
        foreach (CraftingComponent component in craft.RequiredComponents)
        {
            _playerInventory.RemoveFromInventory(component.Item, component.quantityRequired);
        }

        int inventoryCantFit = _playerInventory.AddToInventory(craft.CraftingItem, quantity);

        if(inventoryCantFit != 0)
        {
            ItemFactory.Instance.CreateItemAtPlayerPosition(craft.CraftingItem, inventoryCantFit, false);
        }
    }

    public PlayerInventoryHolder GetPlayerInventory() => _playerInventory;
    public List<CraftingRecipeSO> GetAvailableCraftingRecipes() => _availableCraftingRecipes;

    public void SubscribeToEvents()
    {
        EventBus.Instance.OnInventoryUIItemClick += RequestCraftMenuUpdate;

        EventBus.Instance.OnInventoryChanged += RequestCraftMenuUpdate;
    }

    public void UnsubscribeFromEvents()
    {
        EventBus.Instance.OnInventoryUIItemClick -= RequestCraftMenuUpdate;

        EventBus.Instance.OnInventoryChanged -= RequestCraftMenuUpdate;
    }
}