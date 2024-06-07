using UnityEngine;

public class PlayerCollision : MonoBehaviour, IPlayer
{
    private PlayerInventoryHolder _inventory;

    private void Start()
    {
        _inventory = GetComponent<PlayerInventoryHolder>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();

        if (item != null)
        {
            int quantityRemaining = PickUpItem(item.ItemData, item.Quantity);

            if (quantityRemaining == 0)
            {
                item.DestroyItem();
            }
            else
            {
                item.ChangeQuantity(quantityRemaining);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }

    private int PickUpItem(ItemDataSO item, int quantity)
    {
       return _inventory.AddToInventory(item, quantity);
    }
}
