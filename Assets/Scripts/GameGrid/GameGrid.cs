using Unity.VisualScripting;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new(10, 10);
    public int gap = 1;

    private Entity[,] _grid;

    private void Awake()
    {
        _grid = new Entity[gridSize.x, gridSize.y];
    }

    public (int x, int y) GetGridSize()
    {
        return (gridSize.x, gridSize.y);
    }

    public Entity GetGridEntity(int x, int y)
    {
        return _grid[x, y];
    }

    public void SetGridEntity(Entity entity)
    {
        for (var x = 0; x < entity.size.x; x++)
        for (var y = 0; y < entity.size.y; y++)
            _grid[entity.X + x, entity.Y + y] = entity;
    }

    public void DeleteGridEntity(Entity entity)
    {
        for (var x = 0; x < _grid.GetLength(0); x++)
        for (var y = 0; y < _grid.GetLength(1); y++)
            if (_grid[x, y] == entity)
                _grid[x, y] = null;
    }

    public bool PlaceIsTaken(Entity entity)
    {
        for (int x = -gap; x < entity.size.x + gap; x++)
        for (int y = -gap; y < entity.size.y + gap; y++)
        {
            int gridX = entity.X + x;
            int gridY = entity.Y + y;
            if (0 <= gridX && gridX <= gridSize.x - 1 && 0 <= gridY && gridY <= gridSize.y - 1)
                if (GetGridEntity(entity.X + x, entity.Y + y) != null)
                    return true;
        }

        return false;
    }

    public void ClearGrid()
    {
        for (var x = 0; x < _grid.GetLength(0); x++)
        for (var y = 0; y < _grid.GetLength(1); y++)
        {
            Entity entity = GetGridEntity(x, y);
            Destroy(entity.GameObject());
            _grid[x, y] = null;
        }
    }
}