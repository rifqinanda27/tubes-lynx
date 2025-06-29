using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject[] itemsToDrop;  // Array untuk item yang akan dijatuhkan
    public float dropChance = 0.5f;  // Peluang item untuk dijatuhkan, 50% drop chance

    public void DropItem()
    {
        // Jika ada item yang bisa dijatuhkan
        if (itemsToDrop.Length > 0)
        {
            // Tentukan apakah item akan dijatuhkan berdasarkan peluang drop
            if (Random.value <= dropChance)
            {
                // Pilih item secara acak untuk dijatuhkan
                int randomIndex = Random.Range(0, itemsToDrop.Length);
                GameObject item = itemsToDrop[randomIndex];

                // Check if the item is null before instantiating
                if (item != null)
                {
                    Instantiate(item, transform.position, Quaternion.identity);  // Menjatuhkan item di posisi peti
                    Debug.Log("Item dropped!");
                }
                else
                {
                    Debug.LogWarning("Item is null, cannot drop.");
                }
            }
        }
    }
}
