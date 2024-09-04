using System.Collections.Generic;
using EntitySchemas;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class EnemyGameGrid : GameGrid
{
    private static Entity[,] Grid;
    private static Transform _transform;
    private static EntitiesDict _entities;
    private bool _entitiesPlaced;

    private void Awake()
    {
        Grid = new Entity[DataHolder.GridSize.x, DataHolder.GridSize.y];
        _transform = gameObject.transform;
    }

    private void Update()
    {
        if (!_entitiesPlaced)
            if (_entities != null)
            {
                _SetGrid(_entities);
                _entitiesPlaced = true;
            }
    }

    public Entity GetGridEntity(int x, int y)
    {
        return Grid[x, y];
    }

    public List<int> GetEntityCells(string entityID)
    {
        var entityCells = new List<int>();
        for (var x = 0; x < DataHolder.GridSize.x; x++)
        for (var y = 0; y < DataHolder.GridSize.y; y++)
        {
            Entity entity = Grid[x, y];
            if (entity != null && entity.Uuid == entityID) entityCells.Add(TwoToOneDimCoordinate(x, y));
        }

        return entityCells;
    }

    public void EnableEntity(int cell)
    {
        (int x, int y) = OneToTwoDimCoordinate(cell);
        Entity entity = Grid[x, y];
        entity.GameObject().SetActive(true);
    }

    public static (float x, float y) GetWorldPositionOnGrid(int x, int y)
    {
        return GetWorldPosition(_transform, x, y);
    }

    public static void SetGrid(EntitiesDict entities)
    {
        _entities = entities;
    }

    private void _SetGrid(EntitiesDict entities)
    {
        foreach ((string key, EntityData value) in entities.Entities)
        {
            (int gridX, int gridY) = OneToTwoDimCoordinate(value.Cells[0]);
            (float worldX, float worldY) = GetWorldPositionOnGrid(gridX, gridY);
            int rotationY = (value.Direction - 1) * 90;
            var random = new Random();
            int rotationZ = random.Next(-45, 46);
            Ship shipPrefab = BattleShipController.GetShipBySize(value.Size);

            Ship ship = Instantiate(
                shipPrefab,
                new Vector3(worldX, -0.4f, worldY),
                Quaternion.Euler(0, rotationY, rotationZ)
            );
            ship.Uuid = key;
            ship.Direction = value.Direction;
            int layer = LayerMask.NameToLayer("Default");
            SetLayerRecursively(ship.GameObject(), layer);
            ship.GameObject().SetActive(false);

            foreach (int cell in value.Cells)
            {
                (int x, int y) = OneToTwoDimCoordinate(cell);
                Grid[x, y] = ship;
            }
        }
    }
}