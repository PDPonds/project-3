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
        go.transform.localPosition = new Vector3(-50f, 0, 0);
        Building building = go.GetComponent<Building>();
        outSideDoor.SetLinkDoor(building.inSideDoor);
        outSideDoor.isBuildingTile = true;
        outSideDoor.buildingObj = go;
        building.inSideDoor.SetLinkDoor(outSideDoor);
        building.inSideDoor.isBuildingTile = false;
        building.inSideDoor.buildingObj = go;

        go.SetActive(false);
    }

}
