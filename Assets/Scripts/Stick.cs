using UnityEngine;

public class Stick : MonoBehaviour
{
    public bool isHorizontal = true;
    private static GameObject previewPiece;

   
    public Vector3 GetBarSnapPosition(Transform pieceTransform)
    {
        Vector3 worldPos = pieceTransform.position + transform.localPosition;
        return isHorizontal ?
            GridManager.Instance.GetClosestHorizontalBarPosition(worldPos) :
            GridManager.Instance.GetClosestVerticalBarPosition(worldPos);
    }

    
    public Vector3 GetSnappedPiecePosition(Transform pieceTransform)
    {
        Vector3 barSnap = GetBarSnapPosition(pieceTransform);
        return barSnap - transform.localPosition;
    }

    public static void ShowPreview(Piece originalPiece, Vector3 position)
    {
        if (previewPiece == null)
        {
            previewPiece = Object.Instantiate(originalPiece.gameObject, position, Quaternion.identity);
            foreach (SpriteRenderer sr in previewPiece.GetComponentsInChildren<SpriteRenderer>())
            {
                Color c = sr.color;
                c.a = 0.5f;
                sr.color = c;
            }
            Piece pieceComponent = previewPiece.GetComponent<Piece>();
            if (pieceComponent != null)
                Object.Destroy(pieceComponent);
        }
        else
        {
            previewPiece.transform.position = position;
        }
    }

   
    public static void HidePreview()
    {
        if (previewPiece != null)
        {
            Object.Destroy(previewPiece);
            previewPiece = null;
        }
    }

    public Transform GetTargetBarTransform(Transform pieceTransform)
    {
        Vector3 worldPos = pieceTransform.position + transform.localPosition;
        if (isHorizontal)
        {
            Vector3 snapPos = GridManager.Instance.GetClosestHorizontalBarPosition(worldPos);
            int barX = Mathf.RoundToInt(snapPos.x / GridManager.Instance.cellSize);
            int barY = Mathf.RoundToInt((snapPos.y + (GridManager.Instance.cellSize / 2)) / GridManager.Instance.cellSize);
            if (barX < 0 || barX >= GridManager.Instance.gridWidth || barY < 0 || barY >= GridManager.Instance.gridHeight + 1)
                return null;
            return GridManager.Instance.horizontalBars[barX, barY]?.transform;
        }
        else
        {
            Vector3 snapPos = GridManager.Instance.GetClosestVerticalBarPosition(worldPos);
            int barY = Mathf.RoundToInt(snapPos.y / GridManager.Instance.cellSize);
            int barX = Mathf.RoundToInt((snapPos.x + (GridManager.Instance.cellSize / 2)) / GridManager.Instance.cellSize);
            if (barX < 0 || barX >= GridManager.Instance.gridWidth + 1 || barY < 0 || barY >= GridManager.Instance.gridHeight)
                return null;
            return GridManager.Instance.verticalBars[barX, barY]?.transform;
        }
    }

    public static Transform FindParentBarForPiece(Piece piece, Vector3 piecePosition)
    {
        foreach (Stick stick in piece.GetComponentsInChildren<Stick>())
        {
            Transform barTransform = stick.GetTargetBarTransform(piece.transform);
            if (barTransform != null)
                return barTransform;
        }
        return null;
    }
}
