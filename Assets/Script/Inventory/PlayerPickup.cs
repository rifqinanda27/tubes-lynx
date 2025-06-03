using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            ItemPickup pickup = collision.GetComponent<ItemPickup>();
            Inventory.instance.AddItem(pickup.item);
            Destroy(collision.gameObject);
        }
    }
}
