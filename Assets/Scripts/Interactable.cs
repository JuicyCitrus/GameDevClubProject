using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool canInteract = true;

    public virtual void Interact()
    {
        // Debug.Log("Interacted with " + gameObject.name);
    }
}
