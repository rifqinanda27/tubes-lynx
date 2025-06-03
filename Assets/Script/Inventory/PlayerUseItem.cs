using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{
    private bool isUsingHealthPotion = false; // Untuk mencegah penggunaan ganda HealthPotion
    private bool isUsingEnergyPotion = false; // Untuk mencegah penggunaan ganda EnergyPotion
    private bool isUsingStrengthPotion = false; // Untuk mencegah penggunaan ganda StrengthPotion
    private bool isUsingImmunityPotion = false; // Untuk mencegah penggunaan ganda ImmunityPotion

    private void Update()
    {
        // Gunakan HealthPotion jika tombol Alpha1 ditekan dan belum digunakan
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isUsingHealthPotion)
        {
            isUsingHealthPotion = true;
            Inventory.instance.UseItem(Item.ItemType.HealthPotion); // Panggil UseItem untuk HealthPotion
        }

        // Gunakan EnergyPotion jika tombol Alpha2 ditekan dan belum digunakan
        if (Input.GetKeyDown(KeyCode.Alpha2) && !isUsingEnergyPotion)
        {
            isUsingEnergyPotion = true;
            Inventory.instance.UseItem(Item.ItemType.EnergyPotion); // Panggil UseItem untuk EnergyPotion
        }

        // Gunakan StrengthPotion jika tombol Alpha3 ditekan dan belum digunakan
        if (Input.GetKeyDown(KeyCode.Alpha3) && !isUsingStrengthPotion)
        {
            isUsingStrengthPotion = true;
            Inventory.instance.UseItem(Item.ItemType.StrenghtPotion); // Panggil UseItem untuk StrengthPotion
        }

        // Gunakan ImmunityPotion jika tombol Alpha4 ditekan dan belum digunakan
        if (Input.GetKeyDown(KeyCode.Alpha4) && !isUsingImmunityPotion)
        {
            isUsingImmunityPotion = true;
            Inventory.instance.UseItem(Item.ItemType.ImmunityPotion); // Panggil UseItem untuk ImmunityPotion
        }

        // Reset status penggunaan item saat tombol dilepas
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            isUsingHealthPotion = false; // Reset ketika tombol dilepas
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            isUsingEnergyPotion = false; // Reset ketika tombol dilepas
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            isUsingStrengthPotion = false; // Reset ketika tombol dilepas
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            isUsingImmunityPotion = false; // Reset ketika tombol dilepas
        }
    }
}
