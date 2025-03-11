using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSize = 1.0f;
    public GameObject cellPrefab;

    [HideInInspector]
    public Cell[,] cells;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        cells = new Cell[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GameObject cellGO = Instantiate(cellPrefab, new Vector3(x * cellSize, y * cellSize, 0), Quaternion.identity);
                cellGO.transform.parent = transform;
                Cell cell = cellGO.GetComponent<Cell>();
                if (cell == null)
                {
                    cell = cellGO.AddComponent<Cell>();
                }
                cell.Init(x, y);
                cells[x, y] = cell;
            }
        }
    }

    public void ResetGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                cells[x, y].ResetCell();
            }
        }
    }

    public void CheckAndFillCells()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (cells[x, y].IsFullyEncircled())
                {
                    cells[x, y].FillCell();
                    GameManager.Instance.AddScore(10); 
                }
            }
        }
    }

    public void CheckForBlast()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            bool fullRow = true;
            for (int x = 0; x < gridWidth; x++)
            {
                if (!cells[x, y].isFilled)
                {
                    fullRow = false;
                    break;
                }
            }
            if (fullRow)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    cells[x, y].ClearCell();
                    GameManager.Instance.AddScore(20);
                }
            }
        }

        for (int x = 0; x < gridWidth; x++)
        {
            bool fullCol = true;
            for (int y = 0; y < gridHeight; y++)
            {
                if (!cells[x, y].isFilled)
                {
                    fullCol = false;
                    break;
                }
            }
            if (fullCol)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    cells[x, y].ClearCell();
                    GameManager.Instance.AddScore(20);
                }
            }
        }
    }

    public Vector3 GetSnapPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / cellSize);
        int y = Mathf.RoundToInt(position.y / cellSize);
        return new Vector3(x * cellSize, y * cellSize, 0);
    }

    public bool CanPlacePiece(Piece piece, Vector3 snapPosition)
    {
        foreach (Transform child in piece.transform)
        {
            Vector3 childWorldPos = snapPosition + child.localPosition;
            int gridX = Mathf.RoundToInt(childWorldPos.x / cellSize);
            int gridY = Mathf.RoundToInt(childWorldPos.y / cellSize);
            if (gridX < 0 || gridX >= gridWidth || gridY < 0 || gridY >= gridHeight)
                return false;
            if (cells[gridX, gridY].isFilled)
                return false;
        }
        return true;
    }

    public void PlacePieceAtPosition(Vector3 pos, Piece piece)
    {
        foreach (Transform child in piece.transform)
        {
            Vector3 childWorldPos = pos + child.localPosition;
            int x = Mathf.RoundToInt(childWorldPos.x / cellSize);
            int y = Mathf.RoundToInt(childWorldPos.y / cellSize);
            if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
            {
                cells[x, y].FillCell();
            }
        }
        CheckAndFillCells();
        CheckForBlast();
    }
}
