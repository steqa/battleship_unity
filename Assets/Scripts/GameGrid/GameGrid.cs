using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static (int x, int y) OneToTwoDimCoordinate(int coordinate)
    {
        int x = coordinate % DataHolder.GridSize.x;
        int y = coordinate / DataHolder.GridSize.y;
        return (x, y);
    }

    public static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;
        obj.layer = newLayer;
        foreach (Transform child in obj.transform) SetLayerRecursively(child.gameObject, newLayer);
    }
}