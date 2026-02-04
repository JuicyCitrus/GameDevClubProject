using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Header("Movement Stats")]
    public float moveSpeed = 5f;

    public Rigidbody2D playerRigidbody;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
        controls.Enable();
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = controls.Player.Move.ReadValue<Vector2>();
        Vector2 movement = new Vector2(inputVector.x, inputVector.y) * moveSpeed * Time.fixedDeltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + movement);
    }
}
