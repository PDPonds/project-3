using UnityEngine;

[CreateAssetMenu(fileName = "GasTankItemSO", menuName = "Scriptable Objects/Item/GasTank")]
public class GasTankItemSO : ItemSO
{
    public int fillAmount;
    public float fillDuration;

    public GasTankItemSO()
    {
        itemType = ItemType.Gastank;
    }
}
