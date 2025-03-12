using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSize = 1.0f;

    public GameObject cellPrefab;
    public GameObject horizontalBarPrefab;
    public GameObject verticalBarPrefab;
    public GameObject nodePrefab; 

    [HideInInspector]
    public Cell[,] cells;

    public Bar[,] horizontalBars;
    public Bar[,] verticalBars;

    public List<Vector3> horizontalBarPositions = new List<Vector3>();
    public List<Vector3> verticalBarPositions = new List<Vector3>();

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
                Vector3 cellPos = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject cellGO = Instantiate(cellPrefab, cellPos, Quaternion.identity, transform);
                Cell cell = cellGO.GetComponent<Cell>();
                cell.Init(x, y);
                cells[x, y] = cell;
            }
        }

        horizontalBars = new Bar[gridWidth, gridHeight + 1];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight + 1; y++)
            {
                Vector3 barPos = new Vector3(x * cellSize, y * cellSize - (cellSize / 2), 0);
                GameObject barGO = Instantiate(horizontalBarPrefab, barPos, Quaternion.identity, transform);
                Bar bar = barGO.GetComponent<Bar>();
                bar.gridX = x;
                bar.gridY = y;
                bar.isHorizontal = true;
                horizontalBars[x, y] = bar;
                horizontalBarPositions.Add(barGO.transform.position);
            }
        }

        verticalBars = new Bar[gridWidth + 1, gridHeight];
        for (int x = 0; x < gridWidth + 1; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 barPos = new Vector3(x * cellSize - (cellSize / 2), y * cellSize, 0);
                GameObject barGO = Instantiate(verticalBarPrefab, barPos, Quaternion.identity, transform);
                Bar bar = barGO.GetComponent<Bar>();
                bar.gridX = x;
                bar.gridY = y;
                bar.isHorizontal = false;
                verticalBars[x, y] = bar;
                verticalBarPositions.Add(barGO.transform.position);
            }
        }

        if (nodePrefab != null)
        {
            for (int x = 0; x < gridWidth + 1; x++)
            {
                for (int y = 0; y < gridHeight + 1; y++)
                {
                    Vector3 nodePos = new Vector3(x * cellSize - (cellSize / 2), y * cellSize - (cellSize / 2), 0);
                    GameObject nodeGO = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                    Node node = nodeGO.GetComponent<Node>();
                    if (node != null)
                    {
                        node.gridX = x;
                        node.gridY = y;
                    }
                }
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
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight + 1; y++)
            {
                horizontalBars[x, y].SetFilled(false);
            }
        }
        for (int x = 0; x < gridWidth + 1; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                verticalBars[x, y].SetFilled(false);
            }
        }
    }

    public Vector3 GetClosestHorizontalBarPosition(Vector3 currentPosition)
    {
        Vector3 closest = Vector3.zero;
        float minDist = Mathf.Infinity;
        foreach (Vector3 pos in horizontalBarPositions)
        {
            float dist = Vector3.Distance(currentPosition, pos);
            if (dist < minDist)
            {
                minDist = dist;
                closest = pos;
            }
        }
        return closest;
    }

    public Vector3 GetClosestVerticalBarPosition(Vector3 currentPosition)
    {
        Vector3 closest = Vector3.zero;
        float minDist = Mathf.Infinity;
        foreach (Vector3 pos in verticalBarPositions)
        {
            float dist = Vector3.Distance(currentPosition, pos);
            if (dist < minDist)
            {
                minDist = dist;
                closest = pos;
            }
        }
        return closest;
    }

    public void CheckAndFillCells()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                bool topBar = horizontalBars[x, y + 1].isFilled;
                bool bottomBar = horizontalBars[x, y].isFilled;
                bool leftBar = verticalBars[x, y].isFilled;
                bool rightBar = verticalBars[x + 1, y].isFilled;
                if (topBar && bottomBar && leftBar && rightBar && !cells[x, y].isFilled)
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
            bool rowFilled = true;
            for (int x = 0; x < gridWidth; x++)
            {
                if (!cells[x, y].isFilled)
                {
                    rowFilled = false;
                    break;
                }
            }
            if (rowFilled)
            {
                GameManager.Instance.AddScore(50);
                for (int x = 0; x < gridWidth; x++)
                {
                    cells[x, y].ClearCell();
                }
            }
        }

        for (int x = 0; x < gridWidth; x++)
        {
            bool colFilled = true;
            for (int y = 0; y < gridHeight; y++)
            {
                if (!cells[x, y].isFilled)
                {
                    colFilled = false;
                    break;
                }
            }
            if (colFilled)
            {
                GameManager.Instance.AddScore(50);
                for (int y = 0; y < gridHeight; y++)
                {
                    cells[x, y].ClearCell();
                }
            }
        }
    }


    public bool TryPlacePieceAtPosition(Vector3 pos, Piece piece)
    {
        List<Bar> barsToFill = new List<Bar>();
        foreach (Transform child in piece.transform)
        {
            Vector3 childWorldPos = pos + child.localPosition;
            Stick stick = child.GetComponent<Stick>();
            if (stick == null)
            {
                Debug.LogWarning("Stick component missing; defaulting to horizontal.");
                stick = child.gameObject.AddComponent<Stick>();
                stick.isHorizontal = true;
            }
            if (stick.isHorizontal)
            {
                Vector3 snapPos = GetClosestHorizontalBarPosition(childWorldPos);
                int barX = Mathf.RoundToInt(snapPos.x / cellSize);
                int barY = Mathf.RoundToInt((snapPos.y + (cellSize / 2)) / cellSize);
                if (barX < 0 || barX >= gridWidth || barY < 0 || barY >= gridHeight + 1)
                    return false;
                Bar bar = horizontalBars[barX, barY];
                if (!bar.isHorizontal || bar.isFilled)
                    return false;
                barsToFill.Add(bar);
            }
            else // Vertical stick
            {
                Vector3 snapPos = GetClosestVerticalBarPosition(childWorldPos);
                int barY = Mathf.RoundToInt(snapPos.y / cellSize);
                int barX = Mathf.RoundToInt((snapPos.x + (cellSize / 2)) / cellSize);
                if (barX < 0 || barX >= gridWidth + 1 || barY < 0 || barY >= gridHeight)
                    return false;
                Bar bar = verticalBars[barX, barY];
                if (bar.isHorizontal || bar.isFilled)
                    return false;
                barsToFill.Add(bar);
            }
        }
        foreach (Bar bar in barsToFill)
        {
            bar.SetFilled(true);
        }
        CheckAndFillCells();
        CheckForBlast();
        return true;
    }

    public bool CanPlacePiece(Piece piece, Vector3 snapPosition)
    {
        foreach (Transform child in piece.transform)
        {
            Vector3 childWorldPos = snapPosition + child.localPosition;
            Stick stick = child.GetComponent<Stick>();
            if (stick == null)
            {
                Debug.LogWarning("Stick component missing; defaulting to horizontal.");
                stick = child.gameObject.AddComponent<Stick>();
                stick.isHorizontal = true;
            }
            if (stick.isHorizontal)
            {
                Vector3 barPos = GetClosestHorizontalBarPosition(childWorldPos);
                int barX = Mathf.RoundToInt(barPos.x / cellSize);
                int barY = Mathf.RoundToInt((barPos.y + cellSize / 2) / cellSize);
                if (barX < 0 || barX >= gridWidth || barY < 0 || barY >= gridHeight + 1)
                    return false;
                Bar bar = horizontalBars[barX, barY];
                if (!bar.isHorizontal || bar.isFilled)
                    return false;
            }
            else
            {
                Vector3 barPos = GetClosestVerticalBarPosition(childWorldPos);
                int barY = Mathf.RoundToInt(barPos.y / cellSize);
                int barX = Mathf.RoundToInt((barPos.x + cellSize / 2) / cellSize);
                if (barX < 0 || barX >= gridWidth + 1 || barY < 0 || barY >= gridHeight)
                    return false;
                Bar bar = verticalBars[barX, barY];
                if (bar.isHorizontal || bar.isFilled)
                    return false;
            }
        }
        return true;
    }

    public Vector3 GetSnapPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / cellSize);
        int y = Mathf.RoundToInt(position.y / cellSize);
        return new Vector3(x * cellSize, y * cellSize, 0);
    }
}
