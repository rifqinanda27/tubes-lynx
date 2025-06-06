using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;

    public void AddItem(Item item)
    {
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        icon.sprite = null;
        icon.enabled = false;
    }
}
