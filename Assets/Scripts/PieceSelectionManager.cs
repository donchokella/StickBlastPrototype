using UnityEngine;
using System.Collections.Generic;

public class PieceSelectionManager : MonoBehaviour
{
    [Header("Manuel Oluşturulmuş Parça Prefab'ları (I, L, U)")]
    public GameObject[] piecePrefabs;

    [Header("Seçim Alanı")]
    public Transform selectionArea;

    private List<GameObject> currentPieces = new List<GameObject>();

    void Start()
    {
        GenerateNewPieces();
    }

    public void GenerateNewPieces()
    {
        foreach (GameObject piece in currentPieces)
        {
            Destroy(piece);
        }
        currentPieces.Clear();

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, piecePrefabs.Length);
            GameObject newPiece = Instantiate(piecePrefabs[randomIndex], selectionArea);
            newPiece.transform.localPosition = new Vector3(i * 1.5f-1.5f, 0, 0); // Parçalar arası mesafe ayarı
            currentPieces.Add(newPiece);
        }
    }
}
