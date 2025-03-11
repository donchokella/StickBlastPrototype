using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x, y;
    public bool isFilled = false;

    public bool topFilled = false;
    public bool bottomFilled = false;
    public bool leftFilled = false;
    public bool rightFilled = false;

    public SpriteRenderer sr;

    public void Init(int _x, int _y)
    {
        x = _x;
        y = _y;
        sr = GetComponent<SpriteRenderer>();
    }

    public bool IsFullyEncircled()
    {
        // Tüm kenarlar dolu ve hücre henüz doldurulmamışsa
        return topFilled && bottomFilled && leftFilled && rightFilled && !isFilled;
    }

    public void FillCell()
    {
        isFilled = true;
        if (sr != null)
            sr.color = Color.yellow; 
    }

    public void ClearCell()
    {
        isFilled = false;
        topFilled = bottomFilled = leftFilled = rightFilled = false;
        if (sr != null)
            sr.color = Color.white;
    }

    public void ResetCell()
    {
        ClearCell();
    }
}
