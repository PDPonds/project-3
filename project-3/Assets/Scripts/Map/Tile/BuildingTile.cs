using UnityEngine;

public class BuildingTile : MonoBehaviour
{
    [Header("===== Inside Property =====")]
    [SerializeField] GameObject inSideBuildingPrefab;

    [Header("===== Outside Property =====")]
    [SerializeField] DoorObject outSideDoor;

    public void SpawnInSideBuiding(Transform parent)
    {
        GameObject go = Instantiate(inSideBuildingPrefab, parent);
        Building building = go.GetComponent<Building>();
        outSideDoor.SetBehideDoorPosition(building.inSideDoor.fontDoorPosition);
        outSideDoor.isBuildingTile = true;
        outSideDoor.buildingObj = go;
        building.inSideDoor.SetBehideDoorPosition(outSideDoor.fontDoorPosition);
        building.inSideDoor.isBuildingTile = false;
        building.inSideDoor.buildingObj = go;

        go.SetActive(false);
    }

}
