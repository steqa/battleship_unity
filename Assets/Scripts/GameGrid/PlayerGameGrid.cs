using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class PlayerGameGrid : GameGrid
{
    private static Entity[,] Grid;

    private void Start()
    {
        ShipsContainerController.ChangePosition(gameObject.transform.position.x, gameObject.transform.position.y);
    }

    public static void SetGrid(Entity[,] grid)
    {
        Grid = grid;
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

    public void DrownEntity(int cell)
    {
        (int x, int y) = OneToTwoDimCoordinate(cell);
        Entity entity = Grid[x, y];
        entity.transform.position += new Vector3(0, -0.4f, 0);
        var random = new Random();
        int rotationZ = random.Next(-45, 46);
        entity.transform.Rotate(new Vector3(0, 0, rotationZ));
        int layer = LayerMask.NameToLayer("Default");
        SetLayerRecursively(entity.GameObject(), layer);
    }
}