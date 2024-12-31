using UnityEngine;

public class ItemObject : MonoBehaviour, IActionObject
{
    ItemSlot slot;

    public void Setup(ItemSO item, int count)
    {
        slot.item = item;
        slot.count = count;
    }

    public void Setup(ItemSO item, float maxValue, float curValue)
    {
        slot.item = item;
        slot.count = 1;
        slot.maxValue = maxValue;
        slot.curValue = curValue;
    }

    public void Action()
    {
        GameManager.Instance.playerInventory.AddItem(slot);
    }

    public string ActionName()
    {
        return $"Pick up";
    }
}
