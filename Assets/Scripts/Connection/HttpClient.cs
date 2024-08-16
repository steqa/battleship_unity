using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class CustomHttpClient : MonoBehaviour
{
    private static CustomHttpClient _instance;

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

    public static async Task<HttpRequestResult> Post(string url, string json)
    {
        byte[] bodyRaw = new UTF8Encoding().GetBytes(json);

        using var request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.SetRequestHeader("Content-Type", "application/json");

        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        return new HttpRequestResult
        {
            Success = request.result == UnityWebRequest.Result.Success,
            ResponseText = request.downloadHandler.text,
            Error = request.error
        };
    }

    public static async Task<HttpRequestResult> Get(string url)
    {
        using var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        while (!operation.isDone)
            await Task.Yield();

        return new HttpRequestResult
        {
            Success = request.result == UnityWebRequest.Result.Success,
            ResponseText = request.downloadHandler.text,
            Error = request.error
        };
    }
}

public class HttpRequestResult
{
    public bool Success { get; set; }
    public string ResponseText { get; set; }
    public string Error { get; set; }
}