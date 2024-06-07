using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CraftDetailsDisplay : MonoBehaviour
{
    [SerializeField] private Image _currentCraftDetailsImage;
    [SerializeField] private TextMeshProUGUI _currentCraftNameText;

    [SerializeField] private List<CraftComponentUI> _currentCraftComponents;
    [SerializeField] private PlayerCraftingSystem _playerCraftingSystem;
    [SerializeField] private Button _craftButton;

    private CraftingRecipeSO _currentCraftingRecipeDisplayed;

    private bool _craftIsPossible = false;

    private void Awake()
    {
        foreach (CraftComponentUI craftComponent in _currentCraftComponents)
        {
            craftComponent.InitializeCraftComponent();
        }

        _craftButton.interactable = false;
        ResetCraftDetailsWindow();
        _craftButton?.onClick.AddListener(RequestItemCraft);
    }

    private void OnDestroy()
    {
        _craftButton?.onClick.RemoveAllListeners();
    }

    private void UpdateCraftButton() => _craftButton.interactable = _craftIsPossible;

    private void DisplayCraftComponents(CraftingRecipeSO craftRecipe)
    {
        HideAllComponents();

        _craftIsPossible = true;

        int componentsAmount = craftRecipe.RequiredComponents.Count;

        if (componentsAmount > _currentCraftComponents.Count)
        {
            Debug.LogError($"Crafting recipe contains too much craft components! Possible to display: {_currentCraftComponents.Count} components!");
            _craftIsPossible = false;
            return;
        }

        for (int i = 0; i < componentsAmount; i++)
        {
            _currentCraftComponents[i].gameObject.SetActive(true);

            PlayerInventoryHolder playerInventory = _playerCraftingSystem.GetPlayerInventory();

            bool hasItem = playerInventory.ContainsCraftComponent(craftRecipe.RequiredComponents[i]);

            _currentCraftComponents[i].SetComponentsData(craftRecipe, i, hasItem);

            if (!hasItem)
                _craftIsPossible = false;
        }

        UpdateCraftButton();
    }

    private void UpdateCraftItemImage(Sprite itemSprite)
    {
        _currentCraftDetailsImage.sprite = itemSprite;
        _currentCraftDetailsImage.gameObject.SetActive(true);
    }

    private void UpdateCraftItemText(string craftItemName) => _currentCraftNameText.text = craftItemName;

    private void HideAllComponents()
    {
        foreach (CraftComponentUI craftComponent in _currentCraftComponents)
        {
            craftComponent.gameObject.SetActive(false);
        }
    }

    public void UpdateCraftComponents(PlayerInventoryHolder inventory)
    {
        if (_currentCraftingRecipeDisplayed == null)
            return;

        _craftIsPossible = true;

        int componentsAmount = _currentCraftingRecipeDisplayed.RequiredComponents.Count;

        for (int i = 0; i < componentsAmount; i++)
        {
            _currentCraftComponents[i].gameObject.SetActive(true);

            PlayerInventoryHolder playerInventory = _playerCraftingSystem.GetPlayerInventory();

            bool hasItem = playerInventory.ContainsCraftComponent(_currentCraftingRecipeDisplayed.RequiredComponents[i]);

            _currentCraftComponents[i].SetComponentsData(_currentCraftingRecipeDisplayed, i, hasItem);

            if (!hasItem)
                _craftIsPossible = false;
        }

        UpdateCraftButton();
    }

    public void DisplayCraftDetails(CraftingRecipeSO craftRecipe)
    {
        if (craftRecipe == null)
        {
            ResetCraftDetailsWindow();
        }

        DisplayCraftComponents(craftRecipe);
        UpdateCraftItemImage(craftRecipe.CraftingItem.Icon);
        UpdateCraftItemText(craftRecipe.CraftingItem.ItemName);
        UpdateCraftButton();

        _currentCraftingRecipeDisplayed = craftRecipe;
    }

    public void ResetCraftDetailsWindow()
    {
        HideAllComponents();
        _currentCraftingRecipeDisplayed = null;
        _currentCraftNameText.text = "";
        _currentCraftDetailsImage.sprite = null;
        _currentCraftDetailsImage.gameObject.SetActive(false);
    }

    public void RequestItemCraft()
    {
        if (_currentCraftingRecipeDisplayed != null)
            EventBus.Instance.ItemCraftRequested(_currentCraftingRecipeDisplayed);

        UpdateCraftButton();
    }
}