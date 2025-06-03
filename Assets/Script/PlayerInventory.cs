using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory;

    void Update()
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory is not assigned!");
            return;
        }

        // Gunakan HealthPotion jika tombol Alpha1 ditekan
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventory.UseItem(Item.ItemType.HealthPotion);
        }

        // Gunakan EnergyPotion jika tombol Alpha2 ditekan
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventory.UseItem(Item.ItemType.EnergyPotion);
        }

        // Gunakan StrengthPotion jika tombol Alpha1 ditekan
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventory.UseItem(Item.ItemType.StrenghtPotion);
        }

        // Gunakan ImmunityPotion jika tombol Alpha2 ditekan
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            inventory.UseItem(Item.ItemType.ImmunityPotion);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            ItemPickup itemPickup = collision.GetComponent<ItemPickup>();
            if (itemPickup != null)
            {
                if (inventory != null)
                {
                    inventory.AddItem(itemPickup.item);  // Tambahkan item ke inventaris
                    
                    // Hanya update UI dengan quantity -1
                    var existingItem = inventory.items.Find(i => i.itemData.itemName == itemPickup.item.itemName);
                    if (existingItem != null && existingItem.quantity > 0)
                    {
                        InventoryUI.instance.RefreshUI();  // Memperbarui UI setelah pengambilan item
                    }
                    
                    Destroy(collision.gameObject);       // Hancurkan item setelah diambil
                }
                else
                {
                    Debug.LogError("Inventory is not assigned when picking up item.");
                }
            }
            else
            {
                Debug.LogError("ItemPickup component is missing on the collided object.");
            }
        }
    }
}
