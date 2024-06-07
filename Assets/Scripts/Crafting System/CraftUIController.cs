using System.Collections.Generic;
using UnityEngine;

public class CraftUIController : MonoBehaviour, IEventListener
{
    [SerializeField] private CraftOptionsDisplay _craftOptions;
    [SerializeField] private CraftDetailsDisplay _craftDetails;

    [SerializeField] private RectTransform _craftingDetailsUI;
    [SerializeField] private RectTransform _crafOptionListUI;

    [SerializeField] private PlayerCraftingSystem _playerCraftingSystem;

    public bool IsCraftingMenuOpened { get; private set; } = false;

    private void Awake()
    {
        _craftingDetailsUI.gameObject.SetActive(false);
        _crafOptionListUI.gameObject.SetActive(false);

        SubscribeToEvents();
    }

    private void RequestItemCraft(CraftingRecipeSO craft)
    {
        _playerCraftingSystem.TryCraftingItem(craft);
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void UpdateCraftingUI(PlayerInventoryHolder inventory, List<CraftingRecipeSO> recipes)
    {
        if (!IsCraftingMenuOpened)
            return;

        PlayerInventoryHolder playerInventory = _playerCraftingSystem.GetPlayerInventory();

        _craftOptions.DisplayCraftOptions(recipes, playerInventory);
        _craftDetails.UpdateCraftComponents(inventory);
    }

    public void DisplayCraftMenu()
    {
        PlayerInventoryHolder playerInventory = _playerCraftingSystem.GetPlayerInventory();
        List<CraftingRecipeSO> availableRecipes = _playerCraftingSystem.GetAvailableCraftingRecipes();

        IsCraftingMenuOpened = true;
        _craftingDetailsUI.gameObject.SetActive(true);
        _crafOptionListUI.gameObject.SetActive(true);
        _craftOptions.DisplayCraftOptions(availableRecipes, playerInventory);
    }

    private void DisplayCraftDetails(CraftingRecipeSO craftRecipe)
    {
        _craftDetails.DisplayCraftDetails(craftRecipe);
    }

    public void HideCraftingMenu()
    {
        IsCraftingMenuOpened = false;
        _craftingDetailsUI.gameObject.SetActive(false);
        _crafOptionListUI.gameObject.SetActive(false);
    }

    public void SubscribeToEvents()
    {
        EventBus.Instance.OnCraftMenuUpdateRequest += UpdateCraftingUI;
        EventBus.Instance.OnCraftDetailsRequested += DisplayCraftDetails;
        EventBus.Instance.OnItemCraftRequested += RequestItemCraft;
    }

    public void UnsubscribeFromEvents()
    {
        EventBus.Instance.OnCraftMenuUpdateRequest -= UpdateCraftingUI;
        EventBus.Instance.OnCraftDetailsRequested -= DisplayCraftDetails;
        EventBus.Instance.OnItemCraftRequested -= RequestItemCraft;
    }
}