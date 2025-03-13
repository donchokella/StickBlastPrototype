using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Piece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector3 originalPosition;
    private bool isDragging = false;
    public float snapDistance = 0.5f;
    private GameObject previewPiece;
    public Vector3 dragOffset = new Vector3(0.5f, 0.5f, 0f);

    
    //public enum PieceCategory { Default, HorizontalBar, VerticalBar }
    //public PieceCategory pieceCategory = PieceCategory.Default;

    void Start()
    {
        originalPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        originalPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;
        transform.position = worldPos + dragOffset;

        Vector3? commonSnap = GetCommonSnapPosition();
        if (commonSnap.HasValue && Vector3.Distance(transform.position, commonSnap.Value) <= snapDistance &&
            GridManager.Instance.CanPlacePiece(this, commonSnap.Value))
        {
            ShowPreview(commonSnap.Value);
        }
        else
        {
            HidePreview();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        Vector3? commonSnap = GetCommonSnapPosition();
        if (commonSnap.HasValue && Vector3.Distance(transform.position, commonSnap.Value) <= snapDistance &&
            GridManager.Instance.TryPlacePieceAtPosition(commonSnap.Value, this))
        {
            transform.position = commonSnap.Value;
            // İlk önce, parça yerleştirildiği barın child objesi haline getirilir.
            Transform parentBar = GetParentBarForPiece();
            if (parentBar != null)
            {
                transform.SetParent(parentBar);
            }
            // Parça seçim alanından kaldırılır ve yerleştirilen parçalar listesine eklenir.
            PieceSelectionManager.Instance.RemovePieceFromSelection(gameObject);
            GridManager.Instance.RegisterPlacedPiece(gameObject);
            // Sonrasında blast kontrolü yapılır.
            GridManager.Instance.CheckForBlast();
        }
        else
        {
            transform.position = originalPosition;
        }
        HidePreview();
    }

    Vector3? GetCommonSnapPosition()
    {
        foreach (Transform child in transform)
        {
            Stick stick = child.GetComponent<Stick>();
            if (stick == null)
            {
                stick = child.gameObject.AddComponent<Stick>();
                stick.isHorizontal = true;
            }

            Vector3 candidate = stick.GetBarSnapPosition() - child.localPosition;
            return candidate;
        }
        return null; // Hiç çubuk yoksa null döner.
    }

    Transform GetParentBarForPiece()
    {
        foreach (Transform child in transform)
        {
            Stick stick = child.GetComponent<Stick>();
            if (stick != null)
            {
                Vector3 childWorldPos = transform.position + child.localPosition;
                if (stick.isHorizontal)
                {
                    Vector3 snapPos = GridManager.Instance.GetClosestHorizontalBarPosition(childWorldPos);
                    int barX = Mathf.RoundToInt(snapPos.x / GridManager.Instance.cellSize);
                    int barY = Mathf.RoundToInt((snapPos.y + (GridManager.Instance.cellSize / 2)) / GridManager.Instance.cellSize);
                    return GridManager.Instance.horizontalBars[barX, barY].transform;
                }
                else
                {
                    Vector3 snapPos = GridManager.Instance.GetClosestVerticalBarPosition(childWorldPos);
                    int barY = Mathf.RoundToInt(snapPos.y / GridManager.Instance.cellSize);
                    int barX = Mathf.RoundToInt((snapPos.x + (GridManager.Instance.cellSize / 2)) / GridManager.Instance.cellSize);
                    return GridManager.Instance.verticalBars[barX, barY].transform;
                }
            }
        }
        return null;
    }

    void ShowPreview(Vector3 pos)
    {
        if (previewPiece == null)
        {
            previewPiece = Instantiate(gameObject, pos, Quaternion.identity);
            foreach (SpriteRenderer sr in previewPiece.GetComponentsInChildren<SpriteRenderer>())
            {
                Color c = sr.color;
                c.a = 0.5f;
                sr.color = c;
            }
            Destroy(previewPiece.GetComponent<Piece>());
        }
        else
        {
            previewPiece.transform.position = pos;
        }
    }

    void HidePreview()
    {
        if (previewPiece != null)
            Destroy(previewPiece);
    }

    public bool IsPlaceable()
    {
        List<Vector3> candidates = new List<Vector3>();
        candidates.AddRange(GridManager.Instance.horizontalBarPositions);
        candidates.AddRange(GridManager.Instance.verticalBarPositions);
        foreach (Vector3 candidate in candidates)
        {
            if (GridManager.Instance.CanPlacePiece(this, candidate))
                return true;
        }
        return false;
    }
}
