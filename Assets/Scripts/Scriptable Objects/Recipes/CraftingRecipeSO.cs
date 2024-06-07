using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName ="Crafting System/Crafting Recipe")]
public class CraftingRecipeSO : ScriptableObject
{
    public List<CraftingComponent> RequiredComponents;
    public ItemDataSO CraftingItem;
    public int CraftingTierRequired;
}

