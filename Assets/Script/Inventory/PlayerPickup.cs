using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))  // Pastikan hanya item yang bisa diambil
        {
            ItemPickup pickup = collision.GetComponent<ItemPickup>();
            if (pickup != null)
            {
                Debug.Log("Picking up: " + pickup.item.itemName);
                Inventory.instance.AddItem(pickup.item);  // Tambahkan item ke inventaris
                Destroy(collision.gameObject);  // Hancurkan item setelah diambil
            }
        }
    }
}
