using UnityEngine;
using System.Collections.Generic;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _playerInventory;

    private DynamicInventoryDisplay _playerInvntoryDisplay;
    private PlayerHotbarInventoryDisplay _playerHotbar;
    private DynamicInventoryDisplay _customContainerDisplay;

    private List<InventoryDisplay> _inventories = new();

    public bool IsPlayerInventoryOpened { get; private set; } = false;

    private void Awake()
    {
        _playerInvntoryDisplay = UIComponentProvider.Instance.PlayerInventoryDisplay;
        _playerHotbar = UIComponentProvider.Instance.PlayerHotbar;
        _customContainerDisplay = UIComponentProvider.Instance.CustomContainerDisplay;

        _inventories.Add(_customContainerDisplay);
        _inventories.Add(_playerHotbar);
        _inventories.Add(_playerInvntoryDisplay);

        _customContainerDisplay.gameObject.SetActive(false);
        _playerInvntoryDisplay.gameObject.SetActive(false);

    }

    public void DisplayCustomInventory(InventorySystem inventoryToDisplay)
    {
        _customContainerDisplay.gameObject.SetActive(true);
        _customContainerDisplay.RefreshDynamicInventory(inventoryToDisplay);
    }

    public void HideCustomInventory()
    {
        _customContainerDisplay.gameObject.SetActive(false);
    }

    public void DisplayPlayerInventory()
    {
        IsPlayerInventoryOpened = true;
        _playerInvntoryDisplay.gameObject.SetActive(true);
        _playerInvntoryDisplay.RefreshDynamicInventory(_playerInventory.SecondaryInventorySystem);
    }

    public void CloseAllPanels()
    {
        _customContainerDisplay.gameObject.SetActive(false);
        _playerInvntoryDisplay.gameObject.SetActive(false);
    }

    public void HidePlayerInventory()
    {
        _playerInvntoryDisplay.gameObject.SetActive(false);
        IsPlayerInventoryOpened = false;
    }

    public List<InventorySystem> GetOpenedInventories()
    {
        List<InventorySystem> openedInventories = new();

        foreach (InventoryDisplay display in _inventories)
        {
            if (display.gameObject.activeInHierarchy)
                openedInventories.Add(display.InventorySystem);
        }

        return openedInventories;
    }
}