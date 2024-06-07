
[System.Serializable]
public struct CraftingComponent
{
    public ItemDataSO Item;
    public int quantityRequired;

    public CraftingComponent(ItemDataSO item, int quantity)
    {
        Item = item;
        quantityRequired = quantity;
    }
}
