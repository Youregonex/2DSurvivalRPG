using UnityEngine;
using UnityEngine.UI;

public class CraftOptionUI : MonoBehaviour
{
    [SerializeField] private Image _craftOptionIcon;
    [SerializeField] private Color _canCraftColor;
    [SerializeField] private Color _canNotCraftColor;

    private bool _canBeCrafted; // for future sorting

    private Image _craftOptionBorder;

    private CraftingRecipeSO _craftRecipe;

    public CraftingRecipeSO CraftRecipe => _craftRecipe;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button?.onClick.AddListener(CraftOptionClicked);
        _craftOptionBorder = GetComponent<Image>();
    }

    public void SetCraftOptionData(CraftingRecipeSO newCraftRecipe, bool canCraft)
    {
        _craftRecipe = newCraftRecipe;
        _craftOptionIcon.sprite = _craftRecipe.CraftingItem.Icon;
        _canBeCrafted = canCraft;

        if (canCraft)
            _craftOptionBorder.color = _canCraftColor;
        else
            _craftOptionBorder.color = _canNotCraftColor;
    }

    public void SetEmptyCraftOptionData()
    {
        _craftRecipe = null;
        _craftOptionIcon.sprite = null;
    }

    private void CraftOptionClicked()
    {
        EventBus.Instance.CraftOptionClick(_craftRecipe);
    }
}