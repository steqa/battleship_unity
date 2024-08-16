using System.Collections.Generic;
using Newtonsoft.Json;
using Player;
using Session;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject sessionMenuItemPrefab;
    [SerializeField] private Transform menuItemsContainer;
    [SerializeField] private Canvas joinSessionForm;
    [SerializeField] private Canvas loadingScreen;

    private string _activeSessionName;

    private void Awake()
    {
        i_UpdateSessions();
    }

    public void i_ShowCanvas(Canvas canvas)
    {
        canvas.enabled = true;
    }

    public void i_HideCanvas(Canvas canvas)
    {
        canvas.enabled = false;
    }

    public void i_UpdateSessions()
    {
        ClearSessionList();
        GetSessions();
    }

    public void i_CreateSession(Canvas form)
    {
        string url = DataHolder.HttpApiUrl + "/session/create";
        var nameInput = form.transform.Find("Content/NameInput").GetComponent<TMP_InputField>();
        var passwordInput = form.transform.Find("Content/PasswordInput").GetComponent<TMP_InputField>();
        string json = JsonConvert.SerializeObject(
            new CreateSession { Name = nameInput.text, Password = passwordInput.text }
        );
        SessionHttpRequest(url, json);
    }

    public void i_LoginSession(Canvas form)
    {
        string url = DataHolder.HttpApiUrl + "/session/login";
        var passwordInput = form.transform.Find("Content/PasswordInput").GetComponent<TMP_InputField>();
        string json = JsonConvert.SerializeObject(
            new JoinSession { Name = _activeSessionName, Password = passwordInput.text }
        );
        SessionHttpRequest(url, json);
    }

    private async void SessionHttpRequest(string url, string json)
    {
        HttpRequestResult result = await CustomHttpClient.Post(url, json);
        if (result.Success)
        {
            var player = JsonConvert.DeserializeObject<PlayerID>(result.ResponseText);
            DataHolder.PlayerID = player.ID;
            i_ShowCanvas(loadingScreen);
            SessionWsRequest();
        }
    }

    private static void SessionWsRequest()
    {
        string url = DataHolder.WsApiUrl + "/session/ws";
        var queryParams = $"?player_id={DataHolder.PlayerID}";
        url += queryParams;
        WebSocketClient.ConnectWebSocket(url);
    }

    public void i_LeaveSession()
    {
        WebSocketClient.CloseConnection();
    }

    private async void GetSessions()
    {
        HttpRequestResult result = await CustomHttpClient.Get(DataHolder.HttpApiUrl + "/session");

        if (result.Success)
        {
            Debug.Log($"Ответ: {result.ResponseText}");
            var sessions = JsonConvert.DeserializeObject<List<Session.Session>>(result.ResponseText);
            foreach (Session.Session session in sessions) AddSessionToList(session.Name);
        }
        else
        {
            Debug.LogError($"Ошибка: {result.Error}");
        }
    }

    private void AddSessionToList(string sessionName)
    {
        GameObject sessionMenuItem = Instantiate(sessionMenuItemPrefab, menuItemsContainer);
        sessionMenuItem.GetComponentInChildren<TMP_Text>().text = sessionName;

        var joinBtn = sessionMenuItem.GetComponentInChildren<Button>();
        joinBtn.onClick.AddListener(() =>
        {
            i_ShowCanvas(joinSessionForm);
            _activeSessionName = sessionName;
        });
    }

    private void ClearSessionList()
    {
        foreach (Transform menuItem in menuItemsContainer) Destroy(menuItem.gameObject);
    }
}