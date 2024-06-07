using UnityEngine;

public abstract class ItemDataSO : ScriptableObject
{
    public int ItemID;
    public string ItemName;
    [field: TextArea] public string Description;
    public int MaxStackSize;
    public Sprite Icon;
    public ItemTypeEnum ItemType;
    public GameObject ItemPrefab;
}
