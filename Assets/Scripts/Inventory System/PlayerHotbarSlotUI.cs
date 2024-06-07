using UnityEngine;
using UnityEngine.UI;

public class PlayerHotbarSlotUI : InventorySlotUI
{
    [SerializeField] private Image _selectionBorder;

    public bool IsSelected { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();

        _selectionBorder.enabled = false;
    }

    public void SelectSlot()
    {
        _selectionBorder.enabled = true;
        IsSelected = true;
    }

    public void DeselectSlot()
    {
        _selectionBorder.enabled = false;
        IsSelected = false;
    }
}