using System.Collections.Generic;
using EntitySchema;
using Newtonsoft.Json.Linq;
using Player;
using Serialize;
using UnityEngine;

public class Utils : MonoBehaviour
{
    [SerializeField] private GameGrid gameGrid;
    [SerializeField] private ShipCounter shipCounter;

    public JObject GetPlacementJson()
    {
        if (gameGrid == null || shipCounter == null) return null;
        if (shipCounter.GetCurrentShipsCount() != shipCounter.GetMaxShipsCount()) return null;


        var readyJson = new PlayerPlacement();

        var board = "";

        (int gridSizeX, int gridSizeY) = gameGrid.GetGridSize();
        for (var y = 0; y < gridSizeY; y++)
        for (var x = 0; x < gridSizeX; x++)
        {
            Entity gridEntity = gameGrid.GetGridEntity(x, y);
            if (gridEntity == null)
            {
                board += "0";
            }
            else
            {
                board += "1";
                int cell = x + gridSizeX * y;

                if (readyJson.EntitiesDict.ContainsKey(gridEntity.Uuid))
                    readyJson.EntitiesDict[gridEntity.Uuid].Cells.Add(cell);
                else
                    readyJson.EntitiesDict[gridEntity.Uuid] = new EntityData
                    {
                        Size = gridEntity.Length * gridEntity.Width,
                        Cells = new List<int> { cell }
                    };
            }
        }

        readyJson.Board = board;

        JObject json = jObject.FromObject(readyJson);
        return json;
    }
}