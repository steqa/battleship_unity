using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualGridController : MonoBehaviour
{
    private static SmokeController _smokeController;
    private static CloudController _cloudController;
    private static EnemyGameGrid _enemyGameGrid;
    [SerializeField] private VisualGrid visualGrid;
    [SerializeField] private EnemyGameGrid enemyGameGrid;
    [SerializeField] private SmokeController smokeController;
    [SerializeField] private CloudController cloudController;

    private void Awake()
    {
        _enemyGameGrid = enemyGameGrid;
        _smokeController = smokeController;
        _cloudController = cloudController;
    }

    public static void RemoveCloud(int cell)
    {
        (int x, int y) = OneToTwoDimCoordinate(cell);
        _cloudController.RemoveVisualEntity(x, y);
    }

    public static void PlaceSmokeInsteadCloud(int cell)
    {
        (int x, int y) = OneToTwoDimCoordinate(cell);
        _cloudController.RemoveVisualEntity(x, y);
        _smokeController.PlaceSmoke(x, y);
    }

    public static void PlaceHoleOverEntity(string entityID)
    {
        List<int> entityCells = _enemyGameGrid.GetEntityCells(entityID);
        foreach (int cell in entityCells)
        {
            (int x, int y) = OneToTwoDimCoordinate(cell);
            _smokeController.RemoveVisualEntity(x, y);
        }
    }

    public static void ShowEntity(int cell)
    {
        _enemyGameGrid.EnableEntity(cell);
    }

    private static (int x, int y) OneToTwoDimCoordinate(int coordinate)
    {
        int x = coordinate % DataHolder.GridSize.x;
        int y = coordinate / DataHolder.GridSize.y;
        return (x, y);
    }
}