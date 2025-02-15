
using System.Collections.Generic;
using UnityEngine;

public class StorageObject : MonoBehaviour, IActionObject, ILockable
{
    public List<ItemSlot> slots = new List<ItemSlot>();

    [Header("===== Lock =====")]
    [SerializeField] int maxLockCount;
    [SerializeField] int startShowLockCount;
    public bool IsLocked { get; set; }
    public List<int> LockPos { get; set; }
    public int MaxLockCount { get { return maxLockCount; } set { maxLockCount = value; } }
    public int StartShowLockCount { get { return startShowLockCount; } set { startShowLockCount = value; } }

    private void Awake()
    {
        IsLocked = true;
    }

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        //Random Spawn Item

        if (IsLocked)
        {
            LockPos = new List<int>();
            for (int i = 1; i <= maxLockCount; i++)
            {
                LockPos.Add(i);
            }
            LockPos = GameManager.Instance.ShuffleInt(LockPos);
        }
    }

    #region Action
    public void Action()
    {
        if (!GameManager.Instance.IsPhase(GamePhase.DuringGame)) return;
        if (GameManager.Instance.curPlayer == null) return;

        if (IsLocked)
        {
            UIManager.Instance.ShowLockPick(LockPos, StartShowLockCount, this);
        }
        else
        {
            ActionAfterUnlock();
        }
    }

    public string ActionName()
    {
        if (IsLocked) return "Unlock";
        else return "Open";
    }
    #endregion

    #region ILock
    public void ActionAfterUnlock()
    {
        GameManager.Instance.curStorageObj = this;
        UIManager.Instance.ToggleInventory(ShowInventoryType.Storage);
    }
    #endregion

    #region Storage

    public void AddItem(ItemSO item, int count)
    {
        if (HasItem(item, out int itemIndex))
        {
            if (item.itemStackable)
            {
                slots[itemIndex].count += count;
            }
            else
            {
                ItemSlot slot = new ItemSlot();
                slot.item = item;
                slot.count = count;
                if (slot.item is GunWeaponItemSO gun)
                {
                    slot.maxValue = gun.weaponDurability;
                    slot.curValue = gun.weaponDurability;
                }
                else if (slot.item is GasTankItemSO gasTank)
                {
                    slot.maxValue = gasTank.maxGas;
                    slot.curValue = gasTank.maxGas;
                }
                else if (slot.item is CloseWeaponItemSO closeWeapon)
                {
                    slot.maxValue = closeWeapon.weaponDurability;
                    slot.curValue = closeWeapon.weaponDurability;
                }
                slots.Add(slot);
            }
        }
        else
        {
            ItemSlot slot = new ItemSlot();
            slot.item = item;
            slot.count = count;
            slots.Add(slot);
        }
    }

    public void AddItem(ItemSO item, float maxValue, float curValue)
    {
        if (item is GunWeaponItemSO gun || item is GasTankItemSO gasTank || item is CloseWeaponItemSO closeWeapon)
        {
            ItemSlot slot = new ItemSlot();
            slot.item = item;
            slot.count = 1;
            slot.maxValue = maxValue;
            slot.curValue = curValue;
            slots.Add(slot);
        }
    }

    public void AddItem(ItemSlot slot)
    {
        if (slot.item is GunWeaponItemSO gun || slot.item is GasTankItemSO gasTank || slot.item is CloseWeaponItemSO closeWeapon)
        {
            AddItem(slot.item, slot.maxValue, slot.curValue);
        }
        else
        {
            AddItem(slot.item, slot.count);
        }
    }

    public void RemoveItem(ItemSO item, int count)
    {
        if (EnoughItems(item, count, out int itemIndex))
        {
            slots[itemIndex].count -= count;
            if (slots[itemIndex].count <= 0)
            {
                slots.RemoveAt(itemIndex);
            }
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

    #endregion

}
