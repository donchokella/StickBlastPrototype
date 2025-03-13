using UnityEngine;

public class Bar : MonoBehaviour
{
    public bool isFilled = false;

    public int gridX;
    public int gridY;

    // Bar orientation: true = horizontal, false = vertical
    public bool isHorizontal;

    public void SetFilled(bool filled)
    {
        isFilled = filled;
        
    }

    //for debugging
    public override string ToString()
    {
        return $"Bar[{gridX},{gridY}] - Filled: {isFilled} - {(isHorizontal ? "Horizontal" : "Vertical")}";
    }
}
