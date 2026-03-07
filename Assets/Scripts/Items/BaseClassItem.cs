using UnityEngine;

public class BaseClassItem : MonoBehaviour
{
    public string itemName;
    public int quantity = 1;

    public virtual void UseItem()
    {
        Debug.Log("Using " + itemName);
    }
}
