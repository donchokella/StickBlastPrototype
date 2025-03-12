using UnityEngine;

public class Stick : MonoBehaviour
{
    // true = horizontal, false = vertical
    public bool isHorizontal = true;

    public Vector3 GetBarSnapPosition()
    {
        if (isHorizontal)
        {
            return GridManager.Instance.GetClosestHorizontalBarPosition(transform.position);
        }
        else
        {
            return GridManager.Instance.GetClosestVerticalBarPosition(transform.position);
        }
    }
}
