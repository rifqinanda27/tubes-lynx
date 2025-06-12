using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Lakukan sesuatu ketika pemain mengambil item
            Debug.Log("Item diambil!");
            // Hanya menambahkan item ke inventaris, tanpa menghancurkan objek
            // Penghancuran item dilakukan di PlayerPickup
        }

        if (other.GetComponent<TilemapCollider2D>() != null)
        {
            // Menghentikan pergerakan item saat bersentuhan dengan Tilemap
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<Rigidbody2D>().isKinematic = true;
            Debug.Log("Item berhenti karena terkena Tilemap");
        }
    }
}
