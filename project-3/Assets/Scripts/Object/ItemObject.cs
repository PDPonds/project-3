using UnityEngine;

public class ItemObject : MonoBehaviour, IActionObject
{
    [SerializeField] ItemSlot slot;

    public void Setup(ItemSO item, int count)
    {
        slot.item = item;
        slot.count = count;
        InitVisual();
    }

    public void Setup(ItemSO item, float maxValue, float curValue)
    {
        slot.item = item;
        slot.count = 1;
        slot.maxValue = maxValue;
        slot.curValue = curValue;
        InitVisual();
    }

    void InitVisual()
    {
        if (slot.item != null)
        {
            GameObject visual = Instantiate(slot.item.itemPrefab, transform);
        }
    }

    public void Action()
    {
        UIManager.Instance.CloseInteractiveChoice();
        GameManager.Instance.playerInventory.AddItem(slot);
        Destroy(gameObject);
    }

    public string ActionName()
    {
        return $"Pick up";
    }
}
