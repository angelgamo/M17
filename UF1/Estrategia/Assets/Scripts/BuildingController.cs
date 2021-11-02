using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections;

public class BuildingController : MonoBehaviour
{
    // Tile
    Tilemap collisions;
    [SerializeField]
    Tile tile;
    
    // Cell
    public enum Size { _2x2, _3x3, _3x4, _4x4}
    Size size;
    float cellSize;

    // Build
    [SerializeField]
    GameObject[] buildings;
    GameObject building;
    Vector2Int buildSize;
    int buildSelected;
    bool change;
    bool canBuild;

    // Offset
    Vector3 offsetMouse;
    Vector3 offsetBuilding;
    Vector3Int offsetTile;
    Vector3Int offsetCenterCell;

    // Vector
    Vector3 mouseWorldPos;
    Vector3Int last;
    Vector3Int coordinate;
    Vector3 centerCellPosition;

    // Color
    Color green;
    Color red;

    // Player
    public Player player;

    // UI
    public Image exitBuilMode;

    public Selector2 selector2;

    int countAstar;

    public delegate void buildCost(int logs, int gold, int rocks);
    public event buildCost onBuildCost;

    public delegate void clearBuildCost();
    public event clearBuildCost onClearBuildCost;


    public int ResourceHouses;
    public float logsTime = 1f;
    public float foodTime = 1f;
    public float rocksTime = 2f;
    public float goldTime = 5f;

    private void Start()
    {
        collisions = GameObject.Find("collisions").GetComponent<Tilemap>();
        cellSize = collisions.cellSize.y;
        offsetBuilding = new Vector3(0, cellSize / 2);
        offsetCenterCell = new Vector3Int(1, 1, 0);

        UpdateOffset();
        canBuild = false;

        green = new Color(0, 1, 0, .5f);
        red = new Color(1, 0, 0, .5f);

        ResourceHouses = 1;
        StartCoroutine(GenerateLogs());
        StartCoroutine(GenerateFood());
        StartCoroutine(GenerateRocks());
        StartCoroutine(GenerateGold());
    }

    private void OnEnable()
    {
        exitBuilMode.color = Color.white;
        selector2.enabled = false;
    }

    private void OnDisable()
    {
        Destroy(building);
        exitBuilMode.color = Color.black;
        selector2.enabled = true;
        if (onBuildCost != null) onClearBuildCost.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown("1")) ChangeBuild(1);
        else if (Input.GetKeyDown("2")) ChangeBuild(2);
        else if (Input.GetKeyDown("3")) ChangeBuild(3);
        else if (Input.GetKeyDown(KeyCode.Escape)) this.enabled = false;

        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offsetMouse;
        coordinate = collisions.WorldToCell(mouseWorldPos);
        coordinate.z = (int)collisions.transform.position.z;

        centerCellPosition = collisions.CellToWorld(coordinate + offsetCenterCell) - (offsetMouse + offsetBuilding);

        if (last != coordinate && building != null || change && building != null)
        {
            change = false;

            last = coordinate;
            building.transform.position = centerCellPosition;

            if (CheckCollisions(buildSize.x, buildSize.y))
            {
                building.GetComponent<SpriteRenderer>().color = red;
                canBuild = false;
            } else
            {
                building.GetComponent<SpriteRenderer>().color = green;
                canBuild = true;
            }
        }

        if (Input.GetMouseButtonDown(0) && canBuild && building != null && !player.mouseOverUI)
        {
            Build();
        }
    }

   

    void Build()
    {
        BuildControl build = building.GetComponent<BuildControl>();
        if (CreateBuilding(build.logsCost, build.rocksCost, build.goldCost))
        {
            change = true;
            DrawCollisions();

            building.GetComponent<SpriteRenderer>().color = Color.white;
            building.GetComponent<BuildControl>().Create(last, this);
            building = Instantiate(buildings[buildSelected], new Vector3(999,999,999), Quaternion.identity);
            size = building.GetComponent<BuildControl>().size;
            buildSize = building.GetComponent<BuildControl>().getBuildSize();
            UpdateOffset();

            countAstar = 2;
            StartCoroutine(UpdateAstar());

        }
    }

    IEnumerator UpdateAstar()
    {
        while (countAstar > 0)
        {
            countAstar--;
            //AstarPath.active.Scan();
            yield return new WaitForSeconds(0.05f);
        }
        
    }

    bool CreateBuilding(int logsCost, int rocksCost, int goldCost)
    {
        if (player.logsCount >= logsCost && player.rocksCount >= rocksCost && player.goldCount >= goldCost)
        {
            player.logsCount -= logsCost;
            player.rocksCount -= rocksCost;
            player.goldCount -= goldCost;

            return true;
        }
        return false;
    }

    public void ChangeBuild(int i)
    {
        this.enabled = true;
        change = true;

        buildSelected = i;
        Destroy(building);
        building = Instantiate(buildings[buildSelected]);
        size = building.GetComponent<BuildControl>().size;
        buildSize = building.GetComponent<BuildControl>().getBuildSize();
        UpdateOffset();

        BuildControl buildControl = building.GetComponent<BuildControl>();

        if (onBuildCost != null) onBuildCost.Invoke(buildControl.logsCost, buildControl.goldCost, buildControl.rocksCost);
    }

    void OffsetTileReset()
    {
        offsetTile.x = 0;
        offsetTile.y = 0;
        offsetTile.z = 0;
    }

    bool CheckCollisions(int xSize, int ySize)
    {
        for (int y = 0; y < ySize; y++)
        {
            offsetTile.y = y;
            for (int x = 0; x < xSize; x++)
            {
                offsetTile.x = x;
                if (collisions.HasTile(last + offsetTile)) return true;
            }
        }
        return false;
    }

    void DrawCollisions()
    {
        OffsetTileReset();

        for (int y = 0; y < buildSize.y; y++)
        {
            offsetTile.y = y;
            for (int x = 0; x < buildSize.x; x++)
            {
                offsetTile.x = x;
                collisions.SetTile(last + offsetTile, tile);
            }
        }
    }

    public void DestroyBuild(BuildControl build)
    {
        OffsetTileReset();

        for (int y = 0; y < build.getBuildSize().y; y++)
        {
            offsetTile.y = y;
            for (int x = 0; x < build.getBuildSize().x; x++)
            {
                offsetTile.x = x;
                collisions.SetTile(build.getBuiltAt() + offsetTile, null);
            }
        }

        countAstar = 2;
        StartCoroutine(UpdateAstar());
    }

    void UpdateOffset()
    {
        switch (size)
        {
            case Size._2x2:
                offsetMouse.x = 0f;
                offsetMouse.y = -cellSize / 2;
                break;
            case Size._3x3:
                offsetMouse.x = 0f;
                offsetMouse.y = -cellSize;
                break;
            case Size._3x4:
                offsetMouse.x = cellSize / 2;
                offsetMouse.y = - (cellSize + cellSize/ 4);
                break;
            case Size._4x4:
                offsetMouse.x = 0f;
                offsetMouse.y = -cellSize * 1.5f;
                break;
        }
    }

    IEnumerator GenerateLogs()
    {
        while (true)
        {
            player.logsCount += ResourceHouses;
            yield return new WaitForSeconds(logsTime);
        }
    }

    IEnumerator GenerateFood()
    {
        while (true)
        {
            player.foodCount += ResourceHouses;
            yield return new WaitForSeconds(foodTime);
        }
    }

    IEnumerator GenerateRocks()
    {
        while (true)
        {
            player.rocksCount += ResourceHouses;
            yield return new WaitForSeconds(rocksTime);
        }
    }

    IEnumerator GenerateGold()
    {
        while (true)
        {
            player.goldCount += ResourceHouses;
            yield return new WaitForSeconds(goldTime);
        }
    }
}
