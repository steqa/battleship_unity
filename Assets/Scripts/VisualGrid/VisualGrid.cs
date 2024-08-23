using UnityEngine;

public class VisualGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new(10, 10);
    private VisualEntity[,] _grid;

    private void Awake()
    {
        _grid = new VisualEntity[gridSize.x, gridSize.y];
    }

    public (int x, int y) GetGridSize()
    {
        return (gridSize.x, gridSize.y);
    }

    public void SetGridEntity(VisualEntity entity)
    {
        _grid[entity.X, entity.Y] = entity;
    }

    public VisualEntity GetGridEntity(int x, int y)
    {
        return _grid[x, y];
    }

    public void DeleteGridEntity(int x, int y)
    {
        _grid[x, y] = null;
    }
}