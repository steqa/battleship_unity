using System.Collections.Generic;
using EntityJson;
using Newtonsoft.Json.Linq;
using RequestJson;
using Serialize;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private WebSocketClient websocketClient;
    [SerializeField] private GameGrid grid;
    [SerializeField] private ShipCounter shipCounter;

    public void StartGameBtnEvent()
    {
        if (shipCounter.GetCurrentShipsCount() != shipCounter.GetMaxShipsCount()) return;

        var readyJson = new Ready();

        var board = "";

        (int gridSizeX, int gridSizeY) = grid.GetGridSize();
        for (var y = 0; y < gridSizeY; y++)
        for (var x = 0; x < gridSizeX; x++)
        {
            Entity gridEntity = grid.GetGridEntity(x, y);
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

        var request = new Request
        {
            Type = "ready",
            Detail = json
        };
        websocketClient.SendMessage(request);
    }
}