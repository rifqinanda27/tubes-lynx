using UnityEngine;

public class ChestInteract : MonoBehaviour
{
    public Animator animator;  // Animator for chest opening animation
    public ItemDrop itemDropScript;  // Reference to the ItemDrop script to handle item dropping
    private bool isPlayerNear = false;
    private bool isOpened = false;

    void Update()
    {
        if (isPlayerNear && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        animator.SetTrigger("OpenChest");
        isOpened = true;
        Debug.Log("Chest opened!");

        // Call DropItem method from ItemDrop to handle item dropping
        itemDropScript.DropItem();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
