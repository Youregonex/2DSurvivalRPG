using System.Collections.Generic;
using UnityEngine;

public class CraftOptionsDisplay : MonoBehaviour, IEventListener
{
    [SerializeField] private List<CraftOptionUI> _craftOptionsUI;
    [SerializeField] private CraftOptionUI _craftOptionPrefab;

    private const int INITIAL_POOL_SIZE = 20;

    private void Awake()
    {
        for (int i = 0; i < INITIAL_POOL_SIZE; i++)
        {
            CreateCraftOptionUIInstance();
        }
    }

    public void DisplayCraftOptions(List<CraftingRecipeSO> availableCraftOptions, PlayerInventoryHolder playereInventory)
    {
        DisableAllCraftOptions();

        int craftOptionsNeededForDisplay = availableCraftOptions.Count;

        if(craftOptionsNeededForDisplay > _craftOptionsUI.Count)
        {
            int craftOptionsToCreate = craftOptionsNeededForDisplay - _craftOptionsUI.Count;

            for (int i = 0; i < craftOptionsToCreate; i++)
            {
                CreateCraftOptionUIInstance();
            }
        }

        for (int i = 0; i < craftOptionsNeededForDisplay; i++)
        {
            _craftOptionsUI[i].gameObject.SetActive(true);

            bool canCraft = true;

            List<CraftingComponent> componentsForCraft = availableCraftOptions[i].RequiredComponents;

            foreach(CraftingComponent component in componentsForCraft)
            {
                if(!playereInventory.ContainsCraftComponent(component))
                {
                    canCraft = false;
                    break;
                }
            }

            _craftOptionsUI[i].SetCraftOptionData(availableCraftOptions[i], canCraft);
        }
    }

    private void InitializeCraftOption(CraftOptionUI craftOption)
    {
        craftOption.SetEmptyCraftOptionData();
        craftOption.transform.SetParent(this.transform);
        craftOption.gameObject.SetActive(false);
    }

    private CraftOptionUI CreateCraftOptionUIInstance()
    {
        CraftOptionUI craftOption = Instantiate(_craftOptionPrefab);
        _craftOptionsUI.Add(craftOption);

        InitializeCraftOption(craftOption);
        EventBus.Instance.OnCraftOptionClick += CraftDetailsRequested;
        return craftOption;
    }

    private void CraftDetailsRequested(CraftingRecipeSO craftRecipe)
    {
        EventBus.Instance.CraftDetailsRequested(craftRecipe);
    }

    private void DisableAllCraftOptions()
    {
        if (_craftOptionsUI.Count == 0)
            return;

        foreach (CraftOptionUI craftOption in _craftOptionsUI)
        {
            craftOption.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        foreach (CraftOptionUI craftOption  in _craftOptionsUI)
        {
            EventBus.Instance.OnCraftOptionClick -= CraftDetailsRequested;
        }   
    }

    public void SubscribeToEvents()
    {
        EventBus.Instance.OnCraftOptionClick += CraftDetailsRequested;
    }

    public void UnsubscribeFromEvents()
    {
        EventBus.Instance.OnCraftOptionClick -= CraftDetailsRequested;
    }
}