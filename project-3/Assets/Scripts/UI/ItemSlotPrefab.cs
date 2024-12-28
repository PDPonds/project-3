using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotPrefab : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] Image itemIcon;
    [SerializeField] GameObject valueBorder;
    [SerializeField] Image valueFill;
    [SerializeField] TextMeshProUGUI itemCount;

    Transform lastParent;

    public ItemSlot curSlot = new ItemSlot();

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.curPlayer.IsState(PlayerState.ShowUI)) return;

        lastParent = transform.parent;
        transform.SetParent(UIManager.Instance.canvasTransform);

        Image img = GetComponent<Image>();
        img.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.curPlayer.IsState(PlayerState.ShowUI)) return;
        transform.position = GameManager.Instance.mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.curPlayer.IsState(PlayerState.ShowUI)) return;
        transform.SetParent(lastParent);
        Image img = GetComponent<Image>();
        img.raycastTarget = true;
        UIManager.Instance.UpdateInventory();
    }

    public void SetLastParent(Transform parent)
    {
        lastParent = parent;
    }

    public Transform GetLastParent()
    {
        return lastParent;
    }

    public void Setup(ItemSlot slot)
    {
        curSlot.item = slot.item;
        curSlot.count = slot.count;
        curSlot.curValue = slot.curValue;
        curSlot.maxValue = slot.maxValue;

        itemIcon.sprite = slot.item.itemIcon;
        if (slot.item is GunWeaponItemSO gun || slot.item is GasTankItemSO gasTank || slot.item is CloseWeaponItemSO closeWeapon)
        {
            valueBorder.SetActive(true);
            float p = slot.curValue / slot.maxValue;
            valueFill.fillAmount = p;
            itemCount.gameObject.SetActive(false);
        }
        else
        {
            valueBorder.SetActive(false);

            if (slot.count > 1)
                itemCount.gameObject.SetActive(true);
            else
                itemCount.gameObject.SetActive(false);

            itemCount.text = slot.count.ToString();
        }
    }

}
