using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item; // Item yang akan diambil

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (item != null)
            {
                Inventory.instance.AddItem(item);  // Tambahkan item ke inventory
                Destroy(gameObject);  // Hancurkan item setelah diambil
            }
            else
            {
                Debug.LogError("Item is null!");
            }
        }
    }
}

