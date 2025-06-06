using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory;

    private bool hasPickedUpItem = false;  // Flag untuk mencegah pemanggilan ganda

    private void Update()
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

        // Gunakan StrengthPotion jika tombol Alpha3 ditekan
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventory.UseItem(Item.ItemType.StrenghtPotion);
        }

        // Gunakan ImmunityPotion jika tombol Alpha4 ditekan
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            inventory.UseItem(Item.ItemType.ImmunityPotion);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item") && !hasPickedUpItem)
        {
            hasPickedUpItem = true;  // Mencegah pemanggilan ganda

            Debug.Log("Item collided: " + collision.gameObject.name);
            ItemPickup itemPickup = collision.GetComponent<ItemPickup>();
            if (itemPickup != null)
            {
                Debug.Log("Calling AddItem for " + itemPickup.item.itemName);
                if (inventory != null)
                {
                    inventory.AddItem(itemPickup.item);  // Tambahkan item ke inventaris
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
