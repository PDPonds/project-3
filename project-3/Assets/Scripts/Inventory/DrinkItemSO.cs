using UnityEngine;

[CreateAssetMenu(fileName = "DrinkItemSO", menuName = "Scriptable Objects/Item/Drink")]
public class DrinkItemSO : ItemSO
{
    public int increaseThristyAmount;
    public float drinkDuration;

    public DrinkItemSO()
    {
        itemStackable = true;
        itemCanHoldInHand = true;
        itemType = ItemType.Drink;
    }
}
