using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x, y;
    public bool isFilled = false;
    public SpriteRenderer sr;

    public void Init(int _x, int _y)
    {
        x = _x;
        y = _y;
        sr = GetComponent<SpriteRenderer>();
    }

    public void FillCell()
    {
        isFilled = true;
        if (sr != null)
            sr.color = Color.cyan;
    }

    public void ClearCell()
    {
        isFilled = false;
        if (sr != null)
            sr.color = Color.white;
    }

    public void ResetCell()
    {
        ClearCell();
    }
}
