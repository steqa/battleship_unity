using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static (int x, int y) OneToTwoDimCoordinate(int coordinate)
    {
        int x = coordinate % DataHolder.GridSize.x;
        int y = coordinate / DataHolder.GridSize.y;
        return (x, y);
    }

    public int TwoToOneDimCoordinate(int x, int y)
    {
        int cell = (y + 1) * DataHolder.GridSize.y - (DataHolder.GridSize.x - (x + 1)) - 1;
        return cell;
    }

    public static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;
        obj.layer = newLayer;
        foreach (Transform child in obj.transform) SetLayerRecursively(child.gameObject, newLayer);
    }

    public static (float x, float y) GetWorldPosition(Transform grid, int x, int y)
    {
        float newX = x + grid.position.x - DataHolder.GridSize.x / 2 + 0.5f;
        float newY = y + grid.position.z - DataHolder.GridSize.y / 2 + 0.5f;
        return (newX, newY);
    }
}