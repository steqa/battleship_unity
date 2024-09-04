using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualGridController : MonoBehaviour
{
    private static SmokeController _smokeController;
    private static BulletController _bulletController;
    private static PlayerGameGrid _playerGameGrid;
    [SerializeField] private PlayerGameGrid playerGameGrid;
    [SerializeField] private VisualGrid visualGrid;
    [SerializeField] private SmokeController smokeController;
    [SerializeField] private BulletController bulletController;

    private void Awake()
    {
        _playerGameGrid = playerGameGrid;
        _smokeController = smokeController;
        _bulletController = bulletController;
    }

    public static void PlaceSmoke(int cell)
    {
        (int x, int y) = OneToTwoDimCoordinate(cell);
        _smokeController.PlaceSmoke(x, y);
    }

    public static void PlaceBullet(int cell)
    {
        (int x, int y) = OneToTwoDimCoordinate(cell);
        _bulletController.PlaceBullet(x, y);
    }

    public static void PlaceHoleOverEntity(string entityID)
    {
        List<int> entityCells = _playerGameGrid.GetEntityCells(entityID);
        foreach (int cell in entityCells)
        {
            (int x, int y) = OneToTwoDimCoordinate(cell);
            _smokeController.RemoveVisualEntity(x, y);
        }
    }

    public static void DrownEntity(int cell)
    {
        _playerGameGrid.DrownEntity(cell);
    }


    private static (int x, int y) OneToTwoDimCoordinate(int coordinate)
    {
        int x = coordinate % DataHolder.GridSize.x;
        int y = coordinate / DataHolder.GridSize.y;
        return (x, y);
    }
}