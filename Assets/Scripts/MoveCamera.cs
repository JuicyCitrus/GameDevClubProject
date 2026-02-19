using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform playerTransform;

    private void LateUpdate()
    {

        if (playerTransform != null)
        {
            Vector3 newPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }
}
