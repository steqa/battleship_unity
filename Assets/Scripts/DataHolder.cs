using UnityEngine;

public class DataHolder : MonoBehaviour
{
    public const string HttpApiUrl = "http://localhost:5555/api/v1";
    public const string WsApiUrl = "ws://localhost:5555/api/v1";
    public static string PlayerID;
    public static bool HandleActions;
    private static DataHolder _instance;

    public static Vector2Int GridSize = new(10, 10);
    public static bool PlayerTurn = false;


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
}