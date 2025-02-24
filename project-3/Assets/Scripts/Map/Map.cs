using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public MapTypeSO curMapType;
    [Header("===== Map Size =====")]
    public int mapSize;
    public float cellSize;
    public Vector3 origin;
    [Header("===== Map Prefab =====")]
    public List<GameObject> AllMapFloors = new List<GameObject>();
    public List<GameObject> AllRoads = new List<GameObject>();
    public GameObject FourWayRoad;
    [SerializeField] GameObject ExitPoint;

    public Cell[,] grid;

    public void Setup(MapTypeSO mapTypeSO)
    {
        curMapType = mapTypeSO;
        grid = GenerateGrid();
        UIManager.Instance.ClearParent(transform);
        RandomRoad();
        GenerateRoad();
        GenerateTile();
    }

    Cell[,] GenerateGrid()
    {
        Cell[,] grid = new Cell[mapSize, mapSize];
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                Cell cell = new Cell(x, z);
                grid[x, z] = cell;
            }
        }
        return grid;
    }

    public Vector3 GettWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + origin;
    }

    public Vector3 GettWorldPosition(Cell cell)
    {
        if (cell == null) return Vector3.zero;
        return new Vector3(cell.x, 0, cell.z) * cellSize + origin;
    }

    public void GetXZ(Vector3 worldPos, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPos - origin).x / cellSize);
        z = Mathf.FloorToInt((worldPos - origin).z / cellSize);
    }

    void InitExitPoint(Vector3 position, float rotationY)
    {
        Quaternion rotation = Quaternion.Euler(0, rotationY, 0);
        GameObject go = Instantiate(ExitPoint, position, rotation);
        go.transform.SetParent(transform);
    }

    void RandomRoad()
    {
        if (curMapType is GeneralMap generalMap)
        {
            if (grid != null)
            {
                int isXRoad = Random.Range(0, 10);
                if (isXRoad > 4)
                {
                    int xIndex = Random.Range(0, mapSize);
                    int zIndex = Random.Range(0, mapSize);
                    for (int x = 0; x < mapSize; x++)
                    {
                        for (int z = 0; z < mapSize; z++)
                        {
                            if (x == xIndex)
                            {
                                grid[x, z].isRoad = true;
                                grid[x, z].yRotation = 90;
                            }
                            if (z == zIndex)
                            {
                                grid[x, z].isRoad = true;
                            }

                            if (x == xIndex && z == zIndex)
                            {
                                grid[x, z].yRotation = 0;
                                grid[x, z].isFourWay = true;
                            }
                        }
                    }

                    GameManager.Instance.playerSpawnPoint = GettWorldPosition(mapSize - 1, zIndex);
                    int rand = Random.Range(0, 9);
                    if (rand >= 0 && rand < 4)
                    {
                        InitExitPoint(GettWorldPosition(0, zIndex), 0);
                    }
                    else if (rand >= 3 && rand < 7)
                    {
                        InitExitPoint(GettWorldPosition(xIndex, mapSize - 1), 90);
                    }
                    else
                    {
                        InitExitPoint(GettWorldPosition(xIndex, 0), -90);
                    }
                }
                else
                {
                    int zIndex = Random.Range(0, mapSize);
                    for (int x = 0; x < mapSize; x++)
                    {
                        grid[x, zIndex].isRoad = true;
                        grid[x, zIndex].yRotation = 0;
                    }

                    GameManager.Instance.playerSpawnPoint = GettWorldPosition(mapSize - 1, zIndex);
                    InitExitPoint(GettWorldPosition(0, zIndex), 0);
                }

                for (int x = 0; x < mapSize; x++)
                {
                    for (int z = 0; z < mapSize; z++)
                    {
                        if (z == mapSize - 1 && !grid[x, z].isRoad && !grid[x, z].hasTile)
                        {
                            grid[x, z].canPressBuildingTile = true;
                        }
                    }
                }

            }
        }
        else if (curMapType is CustomMap customMap)
        {
            if (grid != null)
            {
                for (int i = 0; i < customMap.road.Count; i++)
                {
                    grid[customMap.road[i].position.x, customMap.road[i].position.y].isRoad = customMap.road[i].isRoad;
                    grid[customMap.road[i].position.x, customMap.road[i].position.y].yRotation = customMap.road[i].YRotation;
                    grid[customMap.road[i].position.x, customMap.road[i].position.y].isFourWay = customMap.road[i].isFourWay;
                    if (customMap.road[i].isSpawnPoint)
                    {
                        GameManager.Instance.playerSpawnPoint = GettWorldPosition(customMap.road[i].position.x, customMap.road[i].position.y);
                    }

                    if (customMap.road[i].exitPoint)
                    {
                        InitExitPoint(GettWorldPosition(customMap.road[i].position.x, customMap.road[i].position.y), customMap.road[i].exitPointRotation);
                    }
                }
            }
        }
    }

    void GenerateRoad()
    {
        if (grid != null)
        {
            for (int x = 0; x < mapSize; x++)
            {
                for (int z = 0; z < mapSize; z++)
                {
                    Cell cell = grid[x, z];
                    if (cell.isRoad)
                    {
                        if (!cell.isFourWay)
                        {
                            int roadIndex = Random.Range(0, AllRoads.Count);
                            GameObject roadObj = Instantiate(AllRoads[roadIndex], GettWorldPosition(cell), Quaternion.Euler(new Vector3(0, grid[x, z].yRotation, 0)));
                            roadObj.transform.SetParent(transform);
                        }
                        else
                        {
                            GameObject roadObj = Instantiate(FourWayRoad, GettWorldPosition(cell), Quaternion.Euler(new Vector3(0, grid[x, z].yRotation, 0)));
                            roadObj.transform.SetParent(transform);
                        }
                    }
                    else
                    {
                        int floorIndex = Random.Range(0, AllMapFloors.Count);
                        GameObject floorObj = Instantiate(AllMapFloors[floorIndex], GettWorldPosition(cell), Quaternion.Euler(new Vector3(0, grid[x, z].yRotation, 0)));
                        floorObj.transform.SetParent(transform);
                    }
                }
            }
        }

    }

    List<Cell> GetBuildingEmptyCell()
    {
        List<Cell> cells = new List<Cell>();
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                Cell c = grid[x, z];
                if (!c.isRoad && !c.hasTile && c.canPressBuildingTile)
                {
                    cells.Add(c);
                }
            }
        }
        return cells;
    }

    List<Cell> GetNoneBuildingEmptyCell()
    {
        List<Cell> cells = new List<Cell>();
        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                Cell c = grid[x, z];
                if (!c.isRoad && !c.hasTile && !c.canPressBuildingTile)
                {
                    cells.Add(c);
                }
            }
        }
        return cells;
    }

    void GenerateTile()
    {
        if (curMapType is GeneralMap generalMap)
        {
            int b = GetBuildingEmptyCell().Count;
            int n = GetNoneBuildingEmptyCell().Count;
            List<Tile> allTile = generalMap.GetAllTilePrefab(b, n);
            if (allTile.Count > 0)
            {
                for (int x = 0; x < allTile.Count; x++)
                {
                    bool isBuilding = allTile[x].isBuilding;
                    GameObject prefab = allTile[x].TilePrefab;
                    if (isBuilding)
                    {
                        List<Cell> emptyBuildingCell = GetBuildingEmptyCell();
                        int rand = Random.Range(0, emptyBuildingCell.Count);
                        Cell cell = emptyBuildingCell[rand];
                        GameObject go = Instantiate(prefab, GettWorldPosition(cell), Quaternion.identity);
                        go.transform.SetParent(transform);
                        cell.hasTile = true;

                        BuildingTile buildingTile = go.GetComponent<BuildingTile>();
                        buildingTile.SpawnInSideBuiding(transform);
                    }
                    else
                    {
                        List<Cell> emptyNoneBuildingCell = GetNoneBuildingEmptyCell();
                        int rand = Random.Range(0, emptyNoneBuildingCell.Count);
                        Cell cell = emptyNoneBuildingCell[rand];
                        GameObject go = Instantiate(prefab, GettWorldPosition(cell), Quaternion.identity);
                        go.transform.SetParent(transform);
                        cell.hasTile = true;

                    }
                }
            }
        }
        else if (curMapType is CustomMap customMap)
        {
            if (customMap.customTileSets.Count > 0)
            {
                for (int x = 0; x < customMap.customTileSets.Count; x++)
                {
                    bool isBuilding = customMap.customTileSets[x].tile.isBuilding;
                    GameObject prefab = customMap.customTileSets[x].tile.TilePrefab;
                    Cell cell = grid[customMap.customTileSets[x].position.x, customMap.customTileSets[x].position.y];
                    GameObject go = Instantiate(prefab, GettWorldPosition(cell), Quaternion.identity);
                    go.transform.SetParent(transform);
                    cell.hasTile = true;

                    if (isBuilding)
                    {
                        BuildingTile buildingTile = go.GetComponent<BuildingTile>();
                        buildingTile.SpawnInSideBuiding(transform);
                    }
                }
            }
        }
    }

}

public class Cell
{
    public int x;
    public int z;

    public bool isFourWay;
    public float yRotation;

    public bool canPressBuildingTile;

    public bool isRoad;
    public bool hasTile;

    public Cell(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}
