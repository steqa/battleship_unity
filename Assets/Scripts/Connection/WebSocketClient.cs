using System.Text;
using NativeWebSocket;
using Newtonsoft.Json;
using RequestJson;
using ResponseJson;
using Serialize;
using UnityEngine;

public class WebSocketClient : MonoBehaviour
{
    public string playerID;
    private WebSocket _websocket;

    private void Start()
    {
        ConnectWebSocket();
    }

    private void Update()
    {
        if (_websocket != null) _websocket.DispatchMessageQueue();
    }

    private void OnApplicationQuit()
    {
        CloseConnection();
    }

    private async void ConnectWebSocket()
    {
        if (_websocket != null) return;

        _websocket = new WebSocket(DataHolder.Holder.IP);
        _websocket.OnOpen += OnWebSocketOpen;
        _websocket.OnMessage += OnWebSocketMessage;
        _websocket.OnError += OnWebSocketError;
        _websocket.OnClose += OnWebSocketClose;

        await _websocket.Connect();
    }

    private void OnWebSocketOpen()
    {
        Debug.Log("Connection Opened!");
    }

    private void OnWebSocketMessage(byte[] bytes)
    {
        string message = Encoding.UTF8.GetString(bytes);
        Debug.Log("Message Received: " + message);
        var response = new Response();
        try
        {
            response = JsonConvert.DeserializeObject<Response>(message);
        }
        catch (JsonException e)
        {
            Debug.LogError($"Deserialization error: {e.Message}");
        }

        switch (response.Type)
        {
            case "player_id":
            {
                var player = jObject.ToObject<PlayerID>(response.Detail);
                playerID = player.Id;
                Debug.Log("Player ID: " + playerID);
                break;
            }
            case "enemy_id":
            {
                var enemy = jObject.ToObject<EnemyID>(response.Detail);
                Debug.Log("Enemy ID: " + enemy.Id);
                break;
            }
        }
    }

    private void OnWebSocketError(string error)
    {
        Debug.LogError("Error: " + error);
    }

    private void OnWebSocketClose(WebSocketCloseCode closeCode)
    {
        Debug.Log("Connection Closed: " + closeCode);
    }

    public async void SendMessage(Request request)
    {
        if (_websocket == null) return;

        request.PlayerID = playerID;
        string json = JsonConvert.SerializeObject(request);
        await _websocket.SendText(json);
        Debug.Log("Message Sent: " + json);
    }

    private async void CloseConnection()
    {
        if (_websocket == null) return;

        Debug.Log(_websocket);
        await _websocket.Close();
    }
}