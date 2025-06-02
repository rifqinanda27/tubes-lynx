using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Item ditambahkan");

            // Tambah ke inventory
            Inventory.instance.AddItem(itemData);

            // Hapus dari scene
            Destroy(gameObject);
        }
    }
}
