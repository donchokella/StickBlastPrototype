using UnityEngine;

public class PieceGenerator : MonoBehaviour
{
    public GameObject stickPrefab;
    public enum PieceType { I, L, U }
    public PieceType currentType;

    public GameObject GeneratePiece(PieceType type)
    {
        GameObject pieceObj = new GameObject("Piece_" + type.ToString());
        pieceObj.AddComponent<Piece>();
        switch (type)
        {
            case PieceType.I:

                {
                    GameObject stick = Instantiate(stickPrefab, pieceObj.transform);
                    stick.transform.localPosition = new Vector3(0, 0, 0);
                }
                break;
            case PieceType.L:
                {
                    GameObject stick = Instantiate(stickPrefab, pieceObj.transform);
                    stick.transform.localPosition = new Vector3(0, 0, 0);
                }
                {
                    GameObject stick = Instantiate(stickPrefab, pieceObj.transform);
                    stick.transform.localPosition = new Vector3(0, 0, 0);
                    stick.transform.localRotation = Quaternion.Euler(0, 0, -90);
                }
                break;
            case PieceType.U:

                {
                    GameObject stick = Instantiate(stickPrefab, pieceObj.transform);
                    stick.transform.localPosition = new Vector3(-0, 0, 0);
                }
                {
                    GameObject stick = Instantiate(stickPrefab, pieceObj.transform);
                    stick.transform.localPosition = new Vector3(0, 0, 0);
                    stick.transform.localRotation = Quaternion.Euler(0, 0, -90);
                }
                {
                    GameObject stick = Instantiate(stickPrefab, pieceObj.transform);
                    stick.transform.localPosition = new Vector3(1, 0, 0);
                }
                break;
        }
        return pieceObj;
    }
}
