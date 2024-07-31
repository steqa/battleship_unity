using UnityEngine;
using Object = UnityEngine.Object;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new Vector2Int(10, 10);
    
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
    
    public void SetGridEntity(int x, int y, Entity entity)
    {
        grid[x, y] = entity;
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
}
