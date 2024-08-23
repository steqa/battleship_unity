using UnityEngine;

public class PlayerGameGrid : GameGrid
{
    private static Entity[,] _grid;

    private void Start()
    {
        ShipsContainerController.ChangePosition(gameObject.transform.position.x, gameObject.transform.position.y);

        for (var x = 0; x < 10; x++)
        for (var y = 0; y < 10; y++)
        {
            Debug.Log(_grid[x, y]);
            if (_grid[x, y] != null)
                Debug.Log(_grid[x, y].Uuid);
        }
    }

    public static void SetGrid(Entity[,] grid)
    {
        _grid = grid;
    }
}