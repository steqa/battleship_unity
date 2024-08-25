using EntitySchemas;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class EnemyGameGrid : GameGrid
{
    private static Entity[,] _grid;
    private static Vector3 _position;

    private void Awake()
    {
        _position = gameObject.transform.position;
        _grid = new Entity[DataHolder.GridSize.x, DataHolder.GridSize.y];
    }

    public static void SetGrid(EntitiesDict entities)
    {
        foreach ((string key, EntityData value) in entities.Entities)
        {
            (int gridX, int gridY) = OneToTwoDimCoordinate(value.Cells[0]);
            (float worldX, float worldY) = GetWorldPosition(gridX, gridY);
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
                _grid[x, y] = ship;
            }
        }
    }

    private static (float x, float y) GetWorldPosition(int x, int y)
    {
        float newX = x + _position.x - DataHolder.GridSize.x / 2 + 0.5f;
        float newY = y + _position.z - DataHolder.GridSize.y / 2 + 0.5f;
        return (newX, newY);
    }
}