using UnityEngine;

public class Bar : MonoBehaviour
{
    public bool isFilled = false;
    private SpriteRenderer sr;

    public int gridX;
    public int gridY;

    // Bar orientation: true = horizontal, false = vertical
    public bool isHorizontal;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }

    public void SetFilled(bool filled)
    {
        isFilled = filled;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (sr != null)
        {
            sr.color = isFilled ? Color.green : Color.white;
        }
    }

    public override string ToString()
    {
        return $"Bar[{gridX},{gridY}] - Filled: {isFilled} - {(isHorizontal ? "Horizontal" : "Vertical")}";
    }
}
