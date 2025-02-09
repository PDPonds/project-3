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
        RandomRoad();
        GenerateMapObject();
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

    void RandomRoad()
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
            }
            else
            {
                int zIndex = Random.Range(0, mapSize);
                for (int x = 0; x < mapSize; x++)
                {
                    grid[x, zIndex].isRoad = true;
                    grid[x, zIndex].yRotation = 0;
                }
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

    void GenerateMapObject()
    {
        if (grid != null)
        {
            UIManager.Instance.ClearParent(transform);
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
        int b = GetBuildingEmptyCell().Count;
        int n = GetNoneBuildingEmptyCell().Count;
        Debug.Log($"{b} , {n}");
        List<Tile> allTile = curMapType.GetAllTilePrefab(b, n);
        if (allTile.Count > 0)
        {
            int bDebug = 0;
            int nDebug = 0;
            for (int x = 0; x < allTile.Count; x++)
            {
                bool isBuilding = allTile[x].isBuilding;
                GameObject prefab = allTile[x].TilePrefab;
                if (isBuilding)
                {
                    bDebug++;
                    List<Cell> emptyBuildingCell = GetBuildingEmptyCell();
                    int rand = Random.Range(0, emptyBuildingCell.Count);
                    Cell cell = emptyBuildingCell[rand];
                    GameObject go = Instantiate(prefab, GettWorldPosition(cell), Quaternion.identity);
                    go.transform.SetParent(transform);
                    cell.hasTile = true;

                }
                else
                {
                    nDebug++;
                    List<Cell> emptyNoneBuildingCell = GetNoneBuildingEmptyCell();
                    int rand = Random.Range(0, emptyNoneBuildingCell.Count);
                    Cell cell = emptyNoneBuildingCell[rand];
                    GameObject go = Instantiate(prefab, GettWorldPosition(cell), Quaternion.identity);
                    go.transform.SetParent(transform);
                    cell.hasTile = true;

                }
            }
            Debug.Log($"b : {bDebug} , n : {nDebug}");
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
