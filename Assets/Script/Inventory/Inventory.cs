using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<Item> items = new List<Item>();

    public Transform slotParent;
    public GameObject slotPrefab;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        Debug.Log("Item ditambahkan: " + item.itemName);

        GameObject slot = Instantiate(slotPrefab, slotParent);
        slot.GetComponent<Image>().sprite = item.icon;
    }
}
