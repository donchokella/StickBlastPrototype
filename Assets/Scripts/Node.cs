using UnityEngine;

public class Node : MonoBehaviour
{
    public int gridX;
    public int gridY;

    public override string ToString()
    {
        return $"Node[{gridX},{gridY}]";
    }
}
