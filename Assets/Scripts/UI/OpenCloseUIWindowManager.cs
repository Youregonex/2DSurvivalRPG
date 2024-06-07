using UnityEngine;

public class OpenCloseUIWindowManager : MonoBehaviour, IEventListener
{
    private InventoryUIController _inventoryUIController;
    private CraftUIController _craftUIController;
    private ItemDescriptionWindow _itemDescriptionWindow;

    private void Awake()
    {
        _inventoryUIController = UIComponentProvider.Instance.InventoryUIController;
        _craftUIController = UIComponentProvider.Instance.CraftUIController;
        _itemDescriptionWindow = UIComponentProvider.Instance.ItemDescriptionWindow;

        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void CloseAllWindows()
    {
        _inventoryUIController.CloseAllPanels();
        _itemDescriptionWindow.HideItemDescription();
        _craftUIController.HideCraftingMenu();
    }

    private void OpenCraftMenu()
    {
        if (_craftUIController.IsCraftingMenuOpened)
        {
            CloseCraftMenu();
            return;
        }

        _craftUIController.DisplayCraftMenu();

        if (!_inventoryUIController.IsPlayerInventoryOpened)
            _inventoryUIController.DisplayPlayerInventory();
    }

    private void CloseCraftMenu()
    {
        _craftUIController.HideCraftingMenu();

        if (_inventoryUIController.IsPlayerInventoryOpened)
            _inventoryUIController.HidePlayerInventory();
    }

    private void OpenPlayerInventory()
    {
        if (_inventoryUIController.IsPlayerInventoryOpened)
        {
            ClosePlayerInventory();
            return;
        }

        _inventoryUIController.DisplayPlayerInventory();

        if (!_craftUIController.IsCraftingMenuOpened)
            _craftUIController.DisplayCraftMenu();
    }

    private void ClosePlayerInventory()
    {
        _inventoryUIController.HidePlayerInventory();

        if (_craftUIController.IsCraftingMenuOpened)
            _craftUIController.HideCraftingMenu();

        _itemDescriptionWindow.HideItemDescription();
    }

    private void DisplayCustomInventory(InventorySystem inventory) => _inventoryUIController.DisplayCustomInventory(inventory);

    private void HideCustomInventory()
    {
        _inventoryUIController.HideCustomInventory();
        _itemDescriptionWindow.HideItemDescription();
    }

    public void SubscribeToEvents()
    {
        EventBus.Instance.OnPlayerInventoryDisplayRequested += OpenPlayerInventory;
        EventBus.Instance.OnCraftingDisplayRequested += OpenCraftMenu;
        EventBus.Instance.OnEscKeyPressed += CloseAllWindows;
        EventBus.Instance.OnDynamicInventoryDisplayRequested += DisplayCustomInventory;
        EventBus.Instance.OnDynamicInventoryHideRequested += HideCustomInventory;
    }

    public void UnsubscribeFromEvents()
    {
        EventBus.Instance.OnPlayerInventoryDisplayRequested -= OpenPlayerInventory;
        EventBus.Instance.OnCraftingDisplayRequested -= OpenCraftMenu;
        EventBus.Instance.OnEscKeyPressed -= CloseAllWindows;
        EventBus.Instance.OnDynamicInventoryDisplayRequested -= DisplayCustomInventory;
        EventBus.Instance.OnDynamicInventoryHideRequested -= HideCustomInventory;
    }
}
