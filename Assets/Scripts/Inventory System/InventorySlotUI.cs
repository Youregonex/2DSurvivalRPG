using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image _itemSprite;
    [SerializeField] protected TextMeshProUGUI _itemQuantityText;
    [SerializeField] protected InventorySlot _assignedInventorySlot;

    protected Button _button;

    public InventorySlot AssignedInventorySlot => _assignedInventorySlot;

    private InventoryDisplay _parentDisplay;

    protected virtual void Awake()
    {
        ClearSlot();

        _button = GetComponent<Button>();
        _button?.onClick.AddListener(OnUISlotClick);

        _parentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    public void InitializeSlot(InventorySlot slot)
    {
        _assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if(slot.Item != null)
        {
            _itemSprite.sprite = slot.Item.Icon;
            _itemSprite.color = Color.white;

            if (slot.ItemQuantity > 1)
                _itemQuantityText.text = slot.ItemQuantity.ToString();
            else
                _itemQuantityText.text = "";
        }
        else
        {
            ClearSlot();
        }
    }

    public void UpdateUISlot()
    {
        if (_assignedInventorySlot != null)
            UpdateUISlot(_assignedInventorySlot);
    }

    public void ClearSlot()
    {
        _assignedInventorySlot?.ClearSlot();

        _itemSprite.sprite = null;
        _itemSprite.color = Color.clear;
        _itemQuantityText.text = "";
    }

    public void OnUISlotClick() => _parentDisplay.SlotClicked(this);

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_assignedInventorySlot.Item != null)
            EventBus.Instance.PointerEnterInventoryUISlot(this);
    }

    public void OnPointerExit(PointerEventData eventData) => EventBus.Instance.PointerExitInventoryUISlot();

}