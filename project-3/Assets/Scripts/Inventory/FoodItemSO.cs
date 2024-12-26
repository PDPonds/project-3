using UnityEngine;

[CreateAssetMenu(fileName = "FoodItemSO", menuName = "Scriptable Objects/Item/Food")]
public class FoodItemSO : ItemSO
{
    public int increaseAmount;
    public float eatDuration;

    public FoodItemSO()
    {
        itemType = ItemType.Food;
    }
}
