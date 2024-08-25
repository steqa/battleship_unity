public class PlayerGameGrid : GameGrid
{
    private static Entity[,] _grid;

    private void Start()
    {
        ShipsContainerController.ChangePosition(gameObject.transform.position.x, gameObject.transform.position.y);
    }

    public static void SetGrid(Entity[,] grid)
    {
        _grid = grid;
    }
}