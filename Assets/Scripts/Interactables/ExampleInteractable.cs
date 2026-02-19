using UnityEngine;

public class ExampleInteractable : Interactable
{
    public SpriteRenderer spriteRenderer;
    public Color newColor;

    private Color originalColor;

    private void Start()
    {
        originalColor = spriteRenderer.color;
    }

    public override void Interact(GameObject player)
    {
        base.Interact(player);

        if(spriteRenderer.color == originalColor)
        {
            spriteRenderer.color = newColor;
        }
        else
        {
            spriteRenderer.color = originalColor;
        }
    }
}
