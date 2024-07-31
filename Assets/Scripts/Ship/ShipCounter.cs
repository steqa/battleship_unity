using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class ShipCounter : MonoBehaviour
{
    [SerializeField] private Ship ship1;
    [SerializeField] private TextMeshProUGUI ship1Counter;
    [SerializeField] private TextMeshProUGUI ship1CounterMax;

    [SerializeField] private Ship ship2;
    [SerializeField] private TextMeshProUGUI ship2Counter;
    [SerializeField] private TextMeshProUGUI ship2CounterMax;

    [SerializeField] private Ship ship3;
    [SerializeField] private TextMeshProUGUI ship3Counter;
    [SerializeField] private TextMeshProUGUI ship3CounterMax;

    [SerializeField] private Ship ship4;
    [SerializeField] private TextMeshProUGUI ship4Counter;
    [SerializeField] private TextMeshProUGUI ship4CounterMax;

    [SerializeField] private Ship ship5;
    [SerializeField] private TextMeshProUGUI ship5Counter;
    [SerializeField] private TextMeshProUGUI ship5CounterMax;

    [SerializeField] private ShipController shipController;

    [SerializeField] private BlinkText blinkText;

    private void Start()
    {
        ship1CounterMax.text = shipController.GetShipMaxCount(ship1.name).ToString();
        ship2CounterMax.text = shipController.GetShipMaxCount(ship2.name).ToString();
        ship3CounterMax.text = shipController.GetShipMaxCount(ship3.name).ToString();
        ship4CounterMax.text = shipController.GetShipMaxCount(ship4.name).ToString();
        ship5CounterMax.text = shipController.GetShipMaxCount(ship5.name).ToString();
    }

    public void AddShipCount(string shipName)
    {
        TextMeshProUGUI shipCounter = GetShipCounterByShipName(shipName);
        if (shipCounter == null) return;

        int newValue = int.Parse(shipCounter.text) + 1;
        shipCounter.text = newValue.ToString();
    }
    
    public void SubtractShipCount(string shipName)
    {
        TextMeshProUGUI shipCounter = GetShipCounterByShipName(shipName);
        if (shipCounter == null) return;
    
        int newValue = int.Parse(shipCounter.text) - 1;
        shipCounter.text = newValue.ToString();
    }

    public bool LimitShipCount(string shipName)
    {
        TextMeshProUGUI shipCounter = GetShipCounterByShipName(shipName);
        if (shipCounter == null) return false;
        int currentShipCount = int.Parse(shipCounter.text);
        
        TextMeshProUGUI shipCounterMax = GetShipCounterMaxByShipName(shipName);
        if (shipCounterMax == null) return false;
        int maxShipCount = int.Parse(shipCounterMax.text);

        if (currentShipCount >= maxShipCount)
        {
            blinkText.Blink(shipCounter);
            return false;
        }
        return true;
    }

    private TextMeshProUGUI GetShipCounterByShipName(string shipName)
    {
        TextMeshProUGUI shipCounter = null;
        if (shipName == ship1.name) shipCounter = ship1Counter;
        else if (shipName == ship2.name) shipCounter = ship2Counter;
        else if (shipName == ship3.name) shipCounter = ship3Counter;
        else if (shipName == ship4.name) shipCounter = ship4Counter;
        else if (shipName == ship5.name) shipCounter = ship5Counter;
        
        return shipCounter;
    }

    private TextMeshProUGUI GetShipCounterMaxByShipName(string shipName)
    {
        TextMeshProUGUI shipCounterMax = null;
        if (shipName == ship1.name) shipCounterMax = ship1CounterMax;
        else if (shipName == ship2.name) shipCounterMax = ship2CounterMax;
        else if (shipName == ship3.name) shipCounterMax = ship3CounterMax;
        else if (shipName == ship4.name) shipCounterMax = ship4CounterMax;
        else if (shipName == ship5.name) shipCounterMax = ship5CounterMax;
        
        return shipCounterMax;
    }
}
