using UnityEngine;

public class PickUpItem : Interactable
{
    public BaseClassItem item;

    public override void Interact(GameObject player)
    {
        InventorySystem.Instance.AddItem(item);
        this.gameObject.SetActive(false);
    }
}