using UnityEngine;
using Object = UnityEngine.Object;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new Vector2Int(10, 10);
    
    private Object[,] grid;
    
    private void Awake()
    {
        grid = new Object[gridSize.x, gridSize.y];
        
    }

    public (int x, int y) GetGridSize()
    {
        return (gridSize.x, gridSize.y);
    }

    public Object GetGridObject(int x, int y)
    {
        return grid[x, y];
    }
    
    public void SetGridObject(int x, int y, Object obj)
    {
        grid[x, y] = obj;
    }

    public void DeleteGridObject(Object obj)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == obj)
                {
                    grid[x, y] = null;
                }
            }
        }
    }
}
