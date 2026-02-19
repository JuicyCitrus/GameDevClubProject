using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool canInteract = true;
    public GameObject playerReference = null;

    public virtual void Interact(GameObject player)
    {
        // Debug.Log("Interacted with " + gameObject.name);
        playerReference = player;
    }
}
