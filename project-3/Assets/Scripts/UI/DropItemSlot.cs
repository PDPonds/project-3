using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum HandSlotType
{
    Hand_1, Hand_2, HeadEquipment, BodyEquipment, LegEquipment, FootEquipment, Inventory
}

public class DropItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] HandSlotType type;
    [SerializeField] Transform parent;

    public void DropAction(ItemSlotPrefab itemSlotPrefab)
    {
        switch (type)
        {
            case HandSlotType.Hand_1:

                if (!itemSlotPrefab.curSlot.item.itemCanHoldInHand) return;

                if (itemSlotPrefab.GetLastParent() == UIManager.Instance.inventoryParent)
                {
                    if (HasItemInSlot(out ItemSlotPrefab curItemSlotPrefabInSlot))
                    {
                        GameManager.Instance.playerInventory.AddItem(curItemSlotPrefabInSlot.curSlot);
                    }

                    GameManager.Instance.playerInventory.RemoveItem(itemSlotPrefab.curSlot.item, itemSlotPrefab.curSlot.count);
                }

                if (itemSlotPrefab.GetLastParent() == UIManager.Instance.handSlotParent_2)
                {
                    if (HasItemInSlot(out ItemSlotPrefab curItemSlotPrefabInSlot))
                    {
                        GameManager.Instance.curPlayer.handSlot_2.item = curItemSlotPrefabInSlot.curSlot.item;
                        GameManager.Instance.curPlayer.handSlot_2.count = curItemSlotPrefabInSlot.curSlot.count;
                        GameManager.Instance.curPlayer.handSlot_2.maxValue = curItemSlotPrefabInSlot.curSlot.maxValue;
                        GameManager.Instance.curPlayer.handSlot_2.curValue = curItemSlotPrefabInSlot.curSlot.curValue;
                    }
                    else
                    {
                        GameManager.Instance.curPlayer.handSlot_2.item = null;
                        GameManager.Instance.curPlayer.handSlot_2.count = 0;
                        GameManager.Instance.curPlayer.handSlot_2.maxValue = 0;
                        GameManager.Instance.curPlayer.handSlot_2.curValue = 0;
                    }
                }

                GameManager.Instance.curPlayer.handSlot_1.item = itemSlotPrefab.curSlot.item;
                GameManager.Instance.curPlayer.handSlot_1.count = itemSlotPrefab.curSlot.count;
                GameManager.Instance.curPlayer.handSlot_1.maxValue = itemSlotPrefab.curSlot.maxValue;
                GameManager.Instance.curPlayer.handSlot_1.curValue = itemSlotPrefab.curSlot.curValue;

                break;
            case HandSlotType.Hand_2:

                if (!itemSlotPrefab.curSlot.item.itemCanHoldInHand) return;

                if (itemSlotPrefab.GetLastParent() == UIManager.Instance.inventoryParent)
                {

                    if (HasItemInSlot(out ItemSlotPrefab curItemSlotPrefabInSlot))
                    {
                        GameManager.Instance.playerInventory.AddItem(curItemSlotPrefabInSlot.curSlot);
                    }

                    GameManager.Instance.playerInventory.RemoveItem(itemSlotPrefab.curSlot.item, itemSlotPrefab.curSlot.count);
                }

                if (itemSlotPrefab.GetLastParent() == UIManager.Instance.handSlotParent_1)
                {
                    if (HasItemInSlot(out ItemSlotPrefab curItemSlotPrefabInSlot))
                    {
                        GameManager.Instance.curPlayer.handSlot_1.item = curItemSlotPrefabInSlot.curSlot.item;
                        GameManager.Instance.curPlayer.handSlot_1.count = curItemSlotPrefabInSlot.curSlot.count;
                        GameManager.Instance.curPlayer.handSlot_1.maxValue = curItemSlotPrefabInSlot.curSlot.maxValue;
                        GameManager.Instance.curPlayer.handSlot_1.curValue = curItemSlotPrefabInSlot.curSlot.curValue;
                    }
                    else
                    {
                        GameManager.Instance.curPlayer.handSlot_1.item = null;
                        GameManager.Instance.curPlayer.handSlot_1.count = 0;
                        GameManager.Instance.curPlayer.handSlot_1.maxValue = 0;
                        GameManager.Instance.curPlayer.handSlot_1.curValue = 0;
                    }
                }


                GameManager.Instance.curPlayer.handSlot_2.item = itemSlotPrefab.curSlot.item;
                GameManager.Instance.curPlayer.handSlot_2.count = itemSlotPrefab.curSlot.count;
                GameManager.Instance.curPlayer.handSlot_2.maxValue = itemSlotPrefab.curSlot.maxValue;
                GameManager.Instance.curPlayer.handSlot_2.curValue = itemSlotPrefab.curSlot.curValue;

                break;
            case HandSlotType.HeadEquipment:
                break;
            case HandSlotType.BodyEquipment:
                break;
            case HandSlotType.LegEquipment:
                break;
            case HandSlotType.FootEquipment:
                break;
            case HandSlotType.Inventory:

                if (itemSlotPrefab.GetLastParent() != UIManager.Instance.inventoryParent)
                {
                    if (itemSlotPrefab.GetLastParent() == UIManager.Instance.handSlotParent_1)
                    {
                        GameManager.Instance.playerInventory.AddItem(GameManager.Instance.curPlayer.handSlot_1);

                        GameManager.Instance.curPlayer.handSlot_1.item = null;
                        GameManager.Instance.curPlayer.handSlot_1.count = 0;
                        GameManager.Instance.curPlayer.handSlot_1.maxValue = 0;
                        GameManager.Instance.curPlayer.handSlot_1.curValue = 0;
                    }

                    if (itemSlotPrefab.GetLastParent() == UIManager.Instance.handSlotParent_2)
                    {
                        GameManager.Instance.playerInventory.AddItem(GameManager.Instance.curPlayer.handSlot_2);
                        GameManager.Instance.curPlayer.handSlot_2.item = null;
                        GameManager.Instance.curPlayer.handSlot_2.count = 0;
                        GameManager.Instance.curPlayer.handSlot_2.maxValue = 0;
                        GameManager.Instance.curPlayer.handSlot_2.curValue = 0;
                    }
                }

                break;
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemSlotPrefab itemSlotPrefab = eventData.pointerDrag.GetComponent<ItemSlotPrefab>();
        if (itemSlotPrefab == null) return;
        DropAction(itemSlotPrefab);
    }

    bool HasItemInSlot(out ItemSlotPrefab itemSlotPrefab)
    {
        if (transform.childCount > 0)
        {
            itemSlotPrefab = transform.GetChild(0).GetComponent<ItemSlotPrefab>(); ;
            return true;
        }

        itemSlotPrefab = null;
        return false;
    }

}
