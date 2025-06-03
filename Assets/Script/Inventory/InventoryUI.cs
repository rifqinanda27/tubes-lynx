using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public GameObject inventoryPanel;      // Panel yang akan menampilkan inventory
    public Transform itemParent;           // Tempat untuk meletakkan slot item
    public GameObject itemSlotPrefab;      // Prefab untuk slot item
    public Inventory inventory;            // Referensi ke inventory

    private void Awake()
    {
        instance = this;

        // Pengecekan null untuk itemSlotPrefab dan inventoryPanel di Inspector
        if (itemSlotPrefab == null)
        {
            Debug.LogError("Item Slot Prefab is not assigned!");
        }

        if (inventoryPanel == null)
        {
            Debug.LogError("Inventory Panel is not assigned!");
        }

        // Memastikan inventory sudah terhubung
        if (inventory == null)
        {
            Debug.LogError("Inventory is not assigned!");
        }
    }

    private void Update()
    {
        // Toggle panel inventory dengan tombol I
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    public void RefreshUI()
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory is not assigned!");
            return;
        }

        Debug.Log("Refreshing Inventory UI");

        // Hapus semua item slot yang sudah ada
        foreach (Transform child in itemParent)
        {
            Destroy(child.gameObject);  // Hapus slot lama
        }

        // Tambahkan slot baru untuk setiap item yang ada
        foreach (var item in inventory.items)
        {
            // Pastikan hanya item dengan quantity lebih besar dari 0 yang diinstansiasi
            if (item.quantity > 0)
            {
                GameObject slot = Instantiate(itemSlotPrefab, itemParent);
                var image = slot.GetComponentInChildren<Image>();  // Ambil komponen Image
                var text = slot.GetComponentInChildren<TextMeshProUGUI>();    // Ambil komponen TextMeshPro

                if (image != null && item.itemData.icon != null)
                {
                    image.sprite = item.itemData.icon;  // Set gambar item
                }

                // Set jumlah item dengan mengurangi 1 untuk UI
                if (text != null)
                {
                    text.text = Mathf.Max(1, item.quantity).ToString();  // Tampilkan jumlah item -1
                    Debug.Log($"Updating UI for {item.itemData.itemName} with quantity {item.quantity}");
                }
            }
        }
    }
}
