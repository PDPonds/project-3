using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotPrefab : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] GameObject valueBorder;
    [SerializeField] Image valueFill;
    [SerializeField] TextMeshProUGUI itemCount;

    public void Setup(ItemSlot slot)
    {
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
            itemCount.gameObject.SetActive(true);
            itemCount.text = slot.count.ToString();
        }
    }

}
