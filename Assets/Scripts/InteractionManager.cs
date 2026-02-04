using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    public Interactable currentInteractible;
    public GameObject interactionText;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();

        currentInteractible = null;
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        if(controls.Player.Interact.WasPressedThisFrame())
        {
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Interactable>(out Interactable interactible))
        {
            currentInteractible = interactible;
            interactionText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Interactable>(out Interactable interactible))
        {
            if (currentInteractible == interactible)
            {
                currentInteractible = null;
                interactionText.SetActive(false);
            }
        }
    }

    private void Interact()
    {
        if (currentInteractible != null && currentInteractible.canInteract)
        {
            currentInteractible.Interact();
        }
    }
}
