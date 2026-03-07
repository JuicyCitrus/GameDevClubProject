using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; private set; }

    public int inventorySize = 20;
    public InventoryItem[] inventoryItems;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Start()
    {
        inventoryItems = new InventoryItem[inventorySize];
    }

    public void AddItem(BaseClassItem item)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventoryItems[i]?.item?.itemName == null)
            {
                inventoryItems[i].item = item;
                inventoryItems[i].stacks = item.quantity;
                return;
            }
            else if (inventoryItems[i]?.item?.itemName != null)
            {
                if(inventoryItems[i].item.itemName == item.itemName)
                {
                    inventoryItems[i].stacks += item.quantity;
                    return;
                }
            }
        }
    }

    public void RemoveItem(BaseClassItem item) 
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventoryItems[i].item == item)
            {
                inventoryItems[i] = null;
                Debug.Log("Removed " + item.itemName + " from inventory.");
                return;
            }
        }
        Debug.Log("Item " + item.itemName + " not found in inventory.");
    }
}

[System.Serializable]
public class InventoryItem
{
    public BaseClassItem item;
    public int stacks;
}
