using UnityEngine;
using System.Collections.Generic;
using System;

public class PieceSelectionManager : MonoBehaviour
{
    public static PieceSelectionManager Instance;

    public event Action OnGameOver;

    [Header("Manuel Oluşturulmuş Parça Prefab'ları (I, L, U)")]
    public GameObject[] piecePrefabs;

    [Header("Seçim Alanı")]
    public Transform selectionArea;

    private List<GameObject> currentPieces = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

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
            int randomIndex = UnityEngine.Random.Range(0, piecePrefabs.Length);
            GameObject newPiece = Instantiate(piecePrefabs[randomIndex], selectionArea);
            newPiece.transform.localPosition = new Vector3(i * 5f - 5f, 0, 0);
            currentPieces.Add(newPiece);
        }
    }

    public void RemovePieceFromSelection(GameObject piece)
    {
        if (currentPieces.Contains(piece))
        {
            currentPieces.Remove(piece);
        }
        if (currentPieces.Count == 0)
        {
            GenerateNewPieces();
        }
        CheckForGameOver();
    }

    private void CheckForGameOver()
    {
        bool anyPlaceable = false;
        foreach (GameObject piece in currentPieces)
        {
            Piece p = piece.GetComponent<Piece>();
            if (p != null && p.IsPlaceable())
            {
                anyPlaceable = true;
                break;
            }
        }
        if (!anyPlaceable)
        {
            Debug.Log("Game Over: No placeable pieces in selection area!");
            OnGameOver?.Invoke();
        }
    }
}
