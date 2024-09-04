using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using WebsocketMessage;

public class PlayerPlacementController : MonoBehaviour
{
    [SerializeField] private Utils utils;

    [SerializeField] private ShipCounter shipCounter;
    [SerializeField] private GameObject waitingSpinner;
    [SerializeField] private GameObject readyBtn;
    [SerializeField] private GameObject cancelBtn;

    [NonSerialized] public bool PlayerReady;

    private void Update()
    {
        if (shipCounter.GetCurrentShipsCount() == shipCounter.GetMaxShipsCount())
        {
            if (readyBtn.activeSelf == false && PlayerReady == false) readyBtn.SetActive(true);
        }
        else
        {
            if (PlayerReady) PlayerReady = false;
            if (readyBtn.activeSelf) readyBtn.SetActive(false);
            if (cancelBtn.activeSelf) cancelBtn.SetActive(false);
            if (waitingSpinner.activeSelf) waitingSpinner.SetActive(false);
        }
    }

    public void i_ReadyBtnClick()
    {
        readyBtn.SetActive(false);
        cancelBtn.SetActive(true);
        waitingSpinner.SetActive(true);
        PlayerReady = true;
        SendPlayerPlacementReady();
    }

    public void i_CancelBtnClick()
    {
        waitingSpinner.SetActive(false);
        cancelBtn.SetActive(false);
        readyBtn.SetActive(true);
        PlayerReady = false;
        SendPlayerPlacementNotReady();
    }

    private void SendPlayerPlacementReady()
    {
        JObject placementJson = utils.GetPlacementJson();
        if (placementJson == null)
        {
            Debug.LogError("Failed to generate json!");
            return;
        }

        var request = new WsMessage
        {
            Type = WsRequestTypes.PlayerPlacementReady,
            Detail = placementJson
        };
        CustomWebSocketClient.SendMessage(request);
    }

    private void SendPlayerPlacementNotReady()
    {
        var request = new WsMessage { Type = WsRequestTypes.PlayerPlacementNotReady };
        CustomWebSocketClient.SendMessage(request);
    }
}