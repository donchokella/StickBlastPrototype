using UnityEngine;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector3 originalPosition;
    private bool isDragging = false;

    public float snapDistance = 0.5f;
    private GameObject previewPiece;

    void Start()
    {
        originalPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        originalPosition = transform.position;
        Debug.Log("OnBeginDrag tetiklendi");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;
        transform.position = worldPos; // Gerektiginde offset eklemek icin

        Vector3 snapPosition = GridManager.Instance.GetSnapPosition(transform.position);

        if (Vector3.Distance(transform.position, snapPosition) <= snapDistance &&
            GridManager.Instance.CanPlacePiece(this, snapPosition))
        {
            ShowPreview(snapPosition);
        }
        else
        {
            HidePreview();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        Vector3 snapPosition = GridManager.Instance.GetSnapPosition(transform.position);
        if (Vector3.Distance(transform.position, snapPosition) <= snapDistance &&
            GridManager.Instance.CanPlacePiece(this, snapPosition))
        {
            transform.position = snapPosition;
            PlacePiece(snapPosition);
        }
        else
        {
            transform.position = originalPosition;
        }
        HidePreview();
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
            // Önizlemenin sürükleme işlemini devre dışı bırak
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
        {
            Destroy(previewPiece);
        }
    }

    void PlacePiece(Vector3 pos)
    {
        GridManager.Instance.PlacePieceAtPosition(pos, this);
    }
}
