using System.Text;
using NativeWebSocket;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebsocketMessage;

public class WebSocketClient : MonoBehaviour
{
    private static WebSocketClient _instance;
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

    private static async void SendMessage(WsMessage request)
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
            case "EnemyJoined":
            {
                var requestWsMessage = new WsMessage { Type = "PlayerStartGame" };
                SendMessage(requestWsMessage);
                break;
            }
            case "StartGame":
            {
                SceneManager.LoadScene("PlayerPlacementScene");
                break;
            }
        }
    }
}