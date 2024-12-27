using UnityEngine;

[CreateAssetMenu(fileName = "GasTankItemSO", menuName = "Scriptable Objects/Item/GasTank")]
public class GasTankItemSO : ItemSO
{
    public int fillAmount;
    public float fillDuration;
    public float maxGas;

    public GasTankItemSO()
    {
        itemStackable = false;
        itemType = ItemType.Gastank;
    }
}
