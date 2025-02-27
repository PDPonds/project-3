using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectMapPrefab : MonoBehaviour
{
    [SerializeField] Button map_1;
    [SerializeField] TextMeshProUGUI map_1_name;
    [SerializeField] Image map_1_Line;
    [SerializeField] Button map_2;
    [SerializeField] TextMeshProUGUI map_2_name;
    [SerializeField] Image map_2_Line;
    [HideInInspector] public int map_1_Distance;
    [HideInInspector] public int map_2_Distance;

    public void Setup(MapTypeSO mapTypeSO_1, MapTypeSO mapTypeSO_2)
    {
        map_1.gameObject.SetActive(true);
        map_2.gameObject.SetActive(true);

        map_1_Line.gameObject.SetActive(true);
        map_2_Line.gameObject.SetActive(true);

        Image img_1 = map_1.GetComponent<Image>();
        Image img_2 = map_2.GetComponent<Image>();

        map_1_Distance = GameManager.Instance.GenerateTileDistance();
        map_2_Distance = GameManager.Instance.GenerateTileDistance();

        img_1.sprite = mapTypeSO_1.mapIcon;
        map_1_name.text = $"{mapTypeSO_1.mapName} ({map_1_Distance} kg.)";

        img_2.sprite = mapTypeSO_2.mapIcon;
        map_2_name.text = $"{mapTypeSO_2.mapName} ({map_2_Distance} kg.)";

        map_1.onClick.AddListener(() => SelectMap_1(mapTypeSO_1));
        map_2.onClick.AddListener(() => SelectMap_2(mapTypeSO_2));

    }

    public void Setup(MapTypeSO mapTypeSO_1)
    {
        map_1.gameObject.SetActive(true);
        map_2.gameObject.SetActive(false);
        map_1_Line.gameObject.SetActive(false);
        map_2_Line.gameObject.SetActive(false);
        Image img_1 = map_1.GetComponent<Image>();
        map_1_Distance = GameManager.Instance.GenerateTileDistance();
        img_1.sprite = mapTypeSO_1.mapIcon;
        map_1_name.text = $"{mapTypeSO_1.mapName} ({map_1_Distance} kg.)";
        map_1.onClick.AddListener(() => SelectMap_1(mapTypeSO_1));
    }

    public void SetupPreviousMap(MapTypeSO mapTypeSO_1, MapTypeSO mapTypeSO_2, MapTypeSO selectMap)
    {
        map_1.gameObject.SetActive(true);
        map_2.gameObject.SetActive(true);

        map_1_Line.gameObject.SetActive(true);
        map_2_Line.gameObject.SetActive(true);

        Image img_1 = map_1.GetComponent<Image>();
        Image img_2 = map_2.GetComponent<Image>();

        img_1.sprite = mapTypeSO_1.mapIcon;
        map_1_name.text = $"{mapTypeSO_1.mapName}";

        img_2.sprite = mapTypeSO_2.mapIcon;
        map_2_name.text = $"{mapTypeSO_2.mapName}";

        if (selectMap == mapTypeSO_1)
        {
            map_1.interactable = true;
            map_2.interactable = false;
            map_1_Line.color = Color.red;
            map_2_Line.color = Color.black;
        }
        else
        {
            map_1.interactable = false;
            map_2.interactable = true;
            map_1_Line.color = Color.black;
            map_2_Line.color = Color.red;
        }
    }

    public void SetupPreviousMap(MapTypeSO map)
    {
        map_1.gameObject.SetActive(true);
        map_2.gameObject.SetActive(false);
        map_1_Line.gameObject.SetActive(false);
        map_2_Line.gameObject.SetActive(false);
        Image img_1 = map_1.GetComponent<Image>();

        img_1.sprite = map.mapIcon;
        map_1_name.text = $"{map.mapName}";

        map_1.interactable = false;
    }

    void SelectMap_1(MapTypeSO map)
    {
        map_1.interactable = false;
        map_2.interactable = true;
        GameManager.Instance.SelectMap(map);
        GameManager.Instance.currentMapDistanceOnSelect = map_1_Distance;
        UIManager.Instance.UpdateDistanceText();
    }
    void SelectMap_2(MapTypeSO map)
    {
        map_1.interactable = true;
        map_2.interactable = false;
        GameManager.Instance.SelectMap(map);
        GameManager.Instance.currentMapDistanceOnSelect = map_2_Distance;
        UIManager.Instance.UpdateDistanceText();
    }

}

