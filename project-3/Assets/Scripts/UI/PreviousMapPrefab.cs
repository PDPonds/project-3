using UnityEngine;
using UnityEngine.UI;

public class PreviousMapPrefab : MonoBehaviour
{
    public void Setup(MapTypeSO mapTypeSO)
    {
        Image img = GetComponent<Image>();
        img.sprite = mapTypeSO.mapIcon;
        img.color = Color.grey;
    }
}
