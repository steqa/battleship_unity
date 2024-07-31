using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new Vector2Int(10, 10);
    public int gap = 1;
    
    private Entity[,] grid;
    
    private void Awake()
    {
        grid = new Entity[gridSize.x, gridSize.y];
        
    }

    public (int x, int y) GetGridSize()
    {
        return (gridSize.x, gridSize.y);
    }

    public Entity GetGridEntity(int x, int y)
    {
        return grid[x, y];
    }
    
    public void SetGridEntity(Entity entity)
    {
        for (int x = 0; x < entity.size.x; x++)
        {
            for (int y = 0; y < entity.size.y; y++)
            {
                grid[entity.x + x, entity.y + y] = entity;
            }
        }
    }

    public void DeleteGridEntity(Entity entity)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == entity)
                {
                    grid[x, y] = null;
                }
            }
        }
    }
    
    public bool PlaceIsTaken(Entity entity)
    {
        for (int x = -gap; x < entity.size.x + gap; x++)
        {
            for (int y = -gap; y < entity.size.y + gap; y++)
            {
                int gridX = entity.x + x;
                int gridY = entity.y + y;
                if ((0 <= gridX && gridX <= gridSize.x - 1) && (0 <= gridY && gridY <= gridSize.y - 1))
                {
                    if (GetGridEntity(entity.x + x, entity.y + y) != null) return true;
                }
            }
        }

        return false;
    }

    public void ClearGrid()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                var entity = GetGridEntity(x, y);
                Destroy(entity.GameObject());
                grid[x, y] = null;
            }
        }
    }
}
