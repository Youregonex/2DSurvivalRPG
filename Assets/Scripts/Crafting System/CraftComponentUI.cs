using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftComponentUI : MonoBehaviour
{
    [SerializeField] private Color _componentAveilableColor;
    [SerializeField] private Color _componentNotAveilableColor;

    [SerializeField] private Image _craftComponentImage;
    [SerializeField] private TextMeshProUGUI _componentAmountRequired;

    public bool ComponentPresentInInventory { get; private set; } = false;

    private Image _componentBorder;

    private void Awake()
    {
        _componentBorder = GetComponent<Image>();
    }

    public void InitializeCraftComponent()
    {
        _craftComponentImage.sprite = null;
        _componentAmountRequired.text = "";
        gameObject.SetActive(false);
    }

    public void SetComponentsData(CraftingRecipeSO recipe, int componentNumber, bool inventoryHasComponent)
    {
        _craftComponentImage.sprite = recipe.RequiredComponents[componentNumber].Item.Icon;

        int amountRequired = recipe.RequiredComponents[componentNumber].quantityRequired;

        _componentAmountRequired.text = amountRequired > 1 ? recipe.RequiredComponents[componentNumber].quantityRequired.ToString() : "";

        ComponentPresentInInventory = inventoryHasComponent;

        if (inventoryHasComponent)
            _componentBorder.color = _componentAveilableColor;
        else
            _componentBorder.color = _componentNotAveilableColor;

    }
}