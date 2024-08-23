using System.Linq;
using TMPro;
using UnityEngine;

public class ShipCounter : MonoBehaviour
{
    [SerializeField] private Ship ship1;
    [SerializeField] private TextMeshPro ship1Counter;
    [SerializeField] private TextMeshPro ship1CounterMax;

    [SerializeField] private Ship ship2;
    [SerializeField] private TextMeshPro ship2Counter;
    [SerializeField] private TextMeshPro ship2CounterMax;

    [SerializeField] private Ship ship3;
    [SerializeField] private TextMeshPro ship3Counter;
    [SerializeField] private TextMeshPro ship3CounterMax;

    [SerializeField] private Ship ship4;
    [SerializeField] private TextMeshPro ship4Counter;
    [SerializeField] private TextMeshPro ship4CounterMax;

    [SerializeField] private Ship ship5;
    [SerializeField] private TextMeshPro ship5Counter;
    [SerializeField] private TextMeshPro ship5CounterMax;

    [SerializeField] private ShipController shipController;

    private void Start()
    {
        ship1CounterMax.text = shipController.GetShipMaxCount(ship1.name).ToString();
        ship2CounterMax.text = shipController.GetShipMaxCount(ship2.name).ToString();
        ship3CounterMax.text = shipController.GetShipMaxCount(ship3.name).ToString();
        ship4CounterMax.text = shipController.GetShipMaxCount(ship4.name).ToString();
        ship5CounterMax.text = shipController.GetShipMaxCount(ship5.name).ToString();
    }

    public int GetCurrentShipsCount()
    {
        int count = int.Parse(ship1Counter.text) + int.Parse(ship2Counter.text) + int.Parse(ship3Counter.text) +
                    int.Parse(ship4Counter.text) + int.Parse(ship5Counter.text);
        return count;
    }

    public int GetMaxShipsCount()
    {
        int count = int.Parse(ship1CounterMax.text) + int.Parse(ship2CounterMax.text) +
                    int.Parse(ship3CounterMax.text) + int.Parse(ship4CounterMax.text) + int.Parse(ship5CounterMax.text);
        return count;
    }

    public void AddShipCount(string shipName)
    {
        TextMeshPro shipCounter = GetShipCounterByShipName(shipName);
        if (shipCounter == null) return;

        int newValue = int.Parse(shipCounter.text) + 1;
        shipCounter.text = newValue.ToString();
    }

    public void SubtractShipCount(string shipName)
    {
        TextMeshPro shipCounter = GetShipCounterByShipName(shipName);
        if (shipCounter == null) return;

        int newValue = int.Parse(shipCounter.text) - 1;
        shipCounter.text = newValue.ToString();
    }

    public bool LimitShipCount(string shipName)
    {
        TextMeshPro shipCounter = GetShipCounterByShipName(shipName);
        if (shipCounter == null) return false;
        int currentShipCount = int.Parse(shipCounter.text);

        TextMeshPro shipCounterMax = GetShipCounterMaxByShipName(shipName);
        if (shipCounterMax == null) return false;
        int maxShipCount = int.Parse(shipCounterMax.text);

        if (currentShipCount >= maxShipCount)
        {
            PrefabItem[] prefabItems = FindObjectsOfType<PrefabItem>();
            PrefabItem prefabItem = prefabItems.FirstOrDefault(c => c.prefabName == shipName);
            if (prefabItem != null) prefabItem.BlinkWarning();
            return false;
        }

        return true;
    }

    private TextMeshPro GetShipCounterByShipName(string shipName)
    {
        TextMeshPro shipCounter = null;
        if (shipName == ship1.name) shipCounter = ship1Counter;
        else if (shipName == ship2.name) shipCounter = ship2Counter;
        else if (shipName == ship3.name) shipCounter = ship3Counter;
        else if (shipName == ship4.name) shipCounter = ship4Counter;
        else if (shipName == ship5.name) shipCounter = ship5Counter;

        return shipCounter;
    }

    private TextMeshPro GetShipCounterMaxByShipName(string shipName)
    {
        TextMeshPro shipCounterMax = null;
        if (shipName == ship1.name) shipCounterMax = ship1CounterMax;
        else if (shipName == ship2.name) shipCounterMax = ship2CounterMax;
        else if (shipName == ship3.name) shipCounterMax = ship3CounterMax;
        else if (shipName == ship4.name) shipCounterMax = ship4CounterMax;
        else if (shipName == ship5.name) shipCounterMax = ship5CounterMax;

        return shipCounterMax;
    }
}