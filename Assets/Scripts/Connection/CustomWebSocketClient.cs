using System.Text;
using EntitySchemas;
using NativeWebSocket;
using Newtonsoft.Json;
using Player;
using Serialize;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebsocketMessage;

public class CustomWebSocketClient : MonoBehaviour
{
    private static CustomWebSocketClient _instance;
    private static WebSocket _websocket;
    private static string _playerID;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_websocket != null) _websocket.DispatchMessageQueue();
    }

    private void OnApplicationQuit()
    {
        CloseConnection();
    }

    public static async void ConnectWebSocket(string url)
    {
        if (_websocket != null) return;

        _websocket = new WebSocket(url);
        _websocket.OnOpen += OnWebSocketOpen;
        _websocket.OnClose += OnWebSocketClose;
        _websocket.OnError += OnWebSocketError;
        _websocket.OnMessage += OnWebSocketMessage;

        await _websocket.Connect();
    }

    public static async void CloseConnection()
    {
        if (_websocket == null) return;

        Debug.Log("Connection Closed!");
        await _websocket.Close();
        _websocket = null;
    }

    public static async void SendMessage(WsMessage request)
    {
        if (_websocket == null) return;

        string json = JsonConvert.SerializeObject(request);
        await _websocket.SendText(json);
        Debug.Log("Message Sent: " + json);
    }

    private static void OnWebSocketOpen()
    {
        Debug.Log("Connection Opened!");
    }

    private static void OnWebSocketClose(WebSocketCloseCode closeCode)
    {
        Debug.Log("Connection Closed: " + closeCode);
        DataHolder.PlayerID = null;
    }

    private static void OnWebSocketError(string error)
    {
        Debug.LogError("Error: " + error);
    }

    private static void OnWebSocketMessage(byte[] bytes)
    {
        string data = Encoding.UTF8.GetString(bytes);
        Debug.Log("Message Received: " + data);
        var wsMessage = new WsMessage();
        try
        {
            wsMessage = JsonConvert.DeserializeObject<WsMessage>(data);
        }
        catch (JsonException e)
        {
            Debug.LogError($"Deserialization error: {e.Message}");
        }

        switch (wsMessage.Type)
        {
            case WsResponseTypes.EnemyJoined:
            {
                SendMessage(new WsMessage { Type = WsRequestTypes.PlayerStartSession });
                break;
            }
            case WsResponseTypes.EnemyLeft:
            {
                CloseConnection();
                SceneManager.LoadScene("MainMenuScene");
                MainMenuController.ShowEnemyLeftNotification();
                break;
            }
            case WsResponseTypes.StartSession:
            {
                DataHolder.PlayerTurn = false;
                DataHolder.HandleActions = true;
                SceneManager.LoadScene("PlayerPlacementScene");
                break;
            }
            case WsResponseTypes.EnemyPlacementReady:
            {
                SendMessage(new WsMessage { Type = WsRequestTypes.PlayerStartGame });
                break;
            }
            case WsResponseTypes.StartGame:
            {
                PlayerGameGrid.SetGrid(PlacementGrid.GetGrid());
                SceneManager.LoadScene("BattleScene");
                break;
            }
            case WsResponseTypes.EnemyEntities:
            {
                var entities = jObject.ToObject<EntitiesDict>(wsMessage.Detail);
                EnemyGameGrid.SetGrid(entities);
                break;
            }
            case WsResponseTypes.YourTurn:
            {
                DataHolder.PlayerTurn = true;
                break;
            }
            case WsResponseTypes.PlayerHit:
            {
                var response = jObject.ToObject<HitResponse>(wsMessage.Detail);
                switch (response.Status)
                {
                    case "hit":
                        EnemyVisualGridController.PlaceSmokeInsteadCloud(response.Cell);
                        break;
                    case "miss":
                        EnemyVisualGridController.RemoveCloud(response.Cell);
                        break;
                    case "destroy":
                        EnemyVisualGridController.PlaceHoleOverEntity(response.EntityID);
                        EnemyVisualGridController.ShowEntity(response.Cell);
                        break;
                }

                break;
            }
            case WsResponseTypes.EnemyHit:
            {
                var response = jObject.ToObject<HitResponse>(wsMessage.Detail);
                switch (response.Status)
                {
                    case "hit":
                        PlayerVisualGridController.PlaceSmoke(response.Cell);
                        break;
                    case "miss":
                        PlayerVisualGridController.PlaceBullet(response.Cell);
                        break;
                    case "destroy":
                        PlayerVisualGridController.PlaceHoleOverEntity(response.EntityID);
                        PlayerVisualGridController.DrownEntity(response.Cell);
                        break;
                }

                break;
            }
            case WsResponseTypes.Win:
            {
                VictoryDefeatScreen.EnableVictoryScreen();
                DataHolder.PlayerTurn = false;
                break;
            }
            case WsResponseTypes.Defeat:
            {
                VictoryDefeatScreen.EnableDefeatScreen();
                DataHolder.PlayerTurn = false;
                break;
            }
        }
    }
}