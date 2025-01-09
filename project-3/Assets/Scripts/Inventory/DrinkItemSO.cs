using UnityEngine;

[CreateAssetMenu(fileName = "DrinkItemSO", menuName = "Scriptable Objects/Item/Drink")]
public class DrinkItemSO : ItemSO
{
    public int increaseThristyAmount;
    public float drinkDuration;

    public DrinkItemSO()
    {
        itemStackable = true;
        itemType = ItemType.Drink;
    }
}
