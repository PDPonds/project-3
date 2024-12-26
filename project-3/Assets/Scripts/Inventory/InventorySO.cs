using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySO", menuName = "Scriptable Objects/InventorySO")]
public class InventorySO : ScriptableObject
{
    public List<ItemSlot> slots = new List<ItemSlot>();

    public void AddItem(ItemSO item, int count)
    {
        if (HasItem(item, out int itemIndex))
        {
            slots[itemIndex].count += count;
        }
        else
        {
            ItemSlot slot = new ItemSlot();
            slot.item = item;
            slot.count = count;
            slots.Add(slot);
        }
    }

    public void RemoveItem(ItemSO item, int count)
    {
        if (EnoughItems(item, count, out int itemIndex))
        {
            slots[itemIndex].count -= count;
        }
    }

    public bool EnoughItems(ItemSO item, int count, out int index)
    {
        if (HasItem(item, out int itemIndex))
        {
            if (slots[itemIndex].count >= count)
            {
                index = itemIndex;
                return true;
            }
        }
        index = -1;
        return false;
    }

    public bool HasItem(ItemSO item, out int itemIndex)
    {
        if (slots.Count > 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].item == item)
                {
                    itemIndex = i;
                    return true;
                }
            }
        }
        itemIndex = -1;
        return false;
    }

}

[Serializable]
public class ItemSlot
{
    public ItemSO item;
    public int count;

    public float GetSlotWeight()
    {
        float weight = item.itemWeigth * count;
        return weight;
    }

}
