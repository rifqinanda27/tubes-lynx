using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<InventoryItem> items = new List<InventoryItem>();  // Daftar item yang ada di inventaris

    private void Awake()
    {
        // Singleton Pattern untuk memastikan hanya ada satu instance dari Inventory
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);  // Hancurkan objek jika instance sudah ada
    }

    // Menambahkan item ke dalam inventory
    public void AddItem(Item newItem)
    {
        if (newItem == null)
        {
            Debug.LogError("Attempted to add a null item.");
            return;
        }

        // Cari apakah item sudah ada di inventory
        InventoryItem existingItem = items.Find(i => i.itemData.itemName == newItem.itemName);

        if (existingItem != null)
        {
            // Jika item sudah ada, tambah kuantitasnya hanya jika belum ditambah pada frame ini
            existingItem.quantity += 1;
            Debug.Log($"{newItem.itemName} tambah item sudah ada {existingItem.quantity}.");
        }
        else
        {
            // Jika item belum ada, tambahkan item baru dengan kuantitas 1
            items.Add(new InventoryItem(newItem, 1));  // Pastikan item baru ditambahkan dengan kuantitas 1
            Debug.Log($"{newItem.itemName} tambah item baru.");
        }

        // Memanggil UI untuk diperbarui
        if (InventoryUI.instance != null)
        {
            // Pastikan hanya memanggil RefreshUI sekali setelah semua item ditambahkan
            InventoryUI.instance.RefreshUI();
        }
    }

    // Menggunakan item dari inventory
    public void UseItem(Item.ItemType itemType)
    {
        // Temukan item berdasarkan tipe
        InventoryItem item = items.Find(i => i.itemData.itemType == itemType);
        if (item != null && item.quantity > 0)
        {
            item.quantity--; // Kurangi quantity item
            Debug.Log($"Used {item.itemData.itemName}");

            // Hapus item jika quantity <= 0
            if (item.quantity <= 0)
            {
                items.Remove(item); // Hapus item dari inventaris jika quantity = 0
                Debug.Log($"{item.itemData.itemName} removed from inventory.");
            }

            // Panggil untuk memperbarui UI setelah item digunakan
            if (InventoryUI.instance != null)
            {
                InventoryUI.instance.RefreshUI();
            }
        }
        else
        {
            Debug.LogWarning("Item not found or quantity is 0.");
        }
    }

    // Menampilkan semua item di inventaris untuk debugging (Opsional)
    public void PrintInventory()
    {
        foreach (var item in items)
        {
            Debug.Log($"Item: {item.itemData.itemName}, Quantity: {item.quantity}");
        }
    }
}

// Kelas InventoryItem untuk menyimpan data item dan quantity
[System.Serializable]
public class InventoryItem
{
    public Item itemData;  // ItemData adalah Scriptable Object
    public int quantity;

    public InventoryItem(Item itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
    }
}
