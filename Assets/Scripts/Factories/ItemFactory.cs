using UnityEngine;

public class ItemFactory : Factory
{
    private static ItemFactory instance = null;

    public static ItemFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(ItemFactory)) as ItemFactory;
                if (instance == null)
                    Debug.LogError("SingletoneBase<T>: Could not find GameObject of type " + typeof(ItemFactory).Name);
            }
            return instance;
        }
    }

    public GameObject CreateItem(ItemDataSO itemData, int itemQuantity = 1)
    {
        GameObject newItem = CreateInstance(itemData);

        newItem = SetUpItemData(newItem, itemData, itemQuantity);

        return newItem;
    }

    public GameObject CreateInstanceAtPosition(ItemDataSO itemData, Vector3 position, int itemQuantity = 1)
    {
        GameObject newItem = CreateInstanceAtPosition(itemData, position);

        newItem = SetUpItemData(newItem, itemData, itemQuantity);

        return newItem;
    }

    public GameObject CreateItemAtPlayerPosition(ItemDataSO itemData, int itemQuantity = 1, bool useOffset = false)
    {
        GameObject newItem = CreateInstanceAtPlayerPosition(itemData, useOffset);

        newItem = SetUpItemData(newItem, itemData, itemQuantity);

        return newItem;
    }

    private GameObject SetUpItemData(GameObject newItem, ItemDataSO itemData, int itemQuantity)
    {
        Item item = newItem.transform.GetChild(0).GetComponent<Item>();

        item.SetItemData(itemData, itemQuantity);

        return item.transform.parent.gameObject;
    }
}