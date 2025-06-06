using UnityEngine;
using UnityEngine.Tilemaps;  // Tambahkan namespace ini

public class ItemPickup : MonoBehaviour
{
    public Item item;  // Menambahkan referensi ke item yang akan diambil

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Lakukan sesuatu ketika pemain mengambil item
            Debug.Log("Item diambil!");
            Inventory.instance.AddItem(item);  // Menambahkan item ke inventaris
            Destroy(gameObject); // Menghapus item dari scene
        }

        if (other.GetComponent<TilemapCollider2D>() != null)  // Menggunakan TilemapCollider2D untuk mendeteksi Tilemap
        {
            // Menghentikan pergerakan item saat bersentuhan dengan Tilemap
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;  // Mengganti velocity dengan linearVelocity
            GetComponent<Rigidbody2D>().isKinematic = true;       // Set isKinematic agar item tidak terpengaruh fisika lagi
            Debug.Log("Item berhenti karena terkena Tilemap");
        }
    }
}