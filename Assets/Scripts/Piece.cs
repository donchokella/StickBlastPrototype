using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Piece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector3 originalPosition;
    private bool isDragging = false;
    public float snapDistance = 0.5f;
    public Vector3 dragOffset = new Vector3(0.5f, 0.5f, 0f);

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

        Vector3 snapPos;
        if (TrySnapAllSticks(out snapPos))
        {
            Stick.ShowPreview(this, snapPos);
        }
        else
        {
            Stick.HidePreview();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        Vector3 snapPos;
        if (TrySnapAllSticks(out snapPos) && GridManager.Instance.TryPlacePieceAtPosition(snapPos, this))
        {
            transform.position = snapPos;
            Transform parentBar = Stick.FindParentBarForPiece(this, snapPos);
            if (parentBar != null)
            {
                transform.SetParent(parentBar);
            }
            PieceSelectionManager.Instance.RemovePieceFromSelection(gameObject);
            GridManager.Instance.RegisterPlacedPiece(gameObject);
            GridManager.Instance.CheckForBlast();

            this.enabled = false;
        }
        else
        {
            transform.position = originalPosition;
        }
        Stick.HidePreview();
    }

    public bool TrySnapAllSticks(out Vector3 finalSnap)
    {
        finalSnap = Vector3.zero;
        Stick[] sticks = GetComponentsInChildren<Stick>();
        if (sticks.Length == 0)
            return false;

        Vector3 candidate = sticks[0].GetBarSnapPosition(transform) - sticks[0].transform.localPosition;
        if (!GridManager.Instance.CanPlacePieceForStick(this, candidate, sticks[0].isHorizontal))
            return false;

        float tolerance = 2f;
        for (int i = 1; i < sticks.Length; i++)
        {
            Vector3 cand = sticks[i].GetBarSnapPosition(transform) - sticks[i].transform.localPosition;
            if (Vector3.Distance(candidate, cand) > tolerance)
                return false;
            if (!GridManager.Instance.CanPlacePieceForStick(this, cand, sticks[i].isHorizontal))
                return false;
        }
        finalSnap = candidate;
        return true;
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
