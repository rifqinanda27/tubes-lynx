using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    public enum ItemType
    {
        HealthPotion,
        EnergyPotion,
        StrenghtPotion,
        ImmunityPotion
    }
}
