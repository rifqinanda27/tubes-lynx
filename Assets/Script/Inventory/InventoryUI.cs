using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotParent;
    public GameObject slotPrefab;

    Inventory inventory;

    void Start()
    {
        inventory = Inventory.instance;
        inventoryPanel.SetActive(false);
        RefreshUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            if (inventoryPanel.activeSelf)
                RefreshUI();
        }
    }

    public void RefreshUI()
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in inventory.items)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotParent);
            InventorySlot slot = slotGO.GetComponent<InventorySlot>();
            slot.AddItem(item);
        }
    }
}
