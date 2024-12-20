using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private Ship ship1;
    [SerializeField] private int ship1Count = 2;

    [SerializeField] private Ship ship2;
    [SerializeField] private int ship2Count = 2;

    [SerializeField] private Ship ship3;
    [SerializeField] private int ship3Count = 1;

    [SerializeField] private Ship ship4;
    [SerializeField] private int ship4Count = 1;

    [SerializeField] private Ship ship5;
    [SerializeField] private int ship5Count = 2;

    [SerializeField] private PlacementGrid placementGrid;

    [SerializeField] private ShipCounter shipCounter;
    [SerializeField] private EntityController entityController;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1) && DataHolder.HandleActions) StartStopPlacingShip(ship1);
        if (Input.GetKeyUp(KeyCode.Alpha2) && DataHolder.HandleActions) StartStopPlacingShip(ship2);
        if (Input.GetKeyUp(KeyCode.Alpha3) && DataHolder.HandleActions) StartStopPlacingShip(ship3);
        if (Input.GetKeyUp(KeyCode.Alpha4) && DataHolder.HandleActions) StartStopPlacingShip(ship4);
        if (Input.GetKeyUp(KeyCode.Alpha5) && DataHolder.HandleActions) StartStopPlacingShip(ship5);

        Entity flyingEntity = entityController.GetFlyingEntity();
        if (flyingEntity is Ship ship)
        {
            ColorizeShip(ship);
            if (Input.GetMouseButtonDown(0) && EntityController.FlyingEntityAvailableForUse) PlaceShip(ship);
        }
    }

    public Ship[] GetShips()
    {
        return new[] { ship1, ship2, ship3, ship4, ship5 };
    }

    private void StartStopPlacingShip(Ship ship)
    {
        if (shipCounter)
        {
            bool placementAvailable = shipCounter.LimitShipCount(ship.name);
            if (placementAvailable == false) return;
        }

        entityController.StartStopPlacingEntity(ship);
    }

    private void PlaceShip(Ship ship)
    {
        ship.SetColorNormal();
        string shipName = entityController.GetFlyingEntity().name;
        bool isPlaced = entityController.PlaceEntity();
        if (shipCounter && isPlaced) shipCounter.AddShipCount(shipName);
    }

    private void ColorizeShip(Ship ship)
    {
        bool available = !placementGrid.PlaceIsTaken(ship);
        ship.SetColorStatus(available);
    }

    public int GetShipMaxCount(string shipName)
    {
        var count = 0;
        if (shipName == ship1.name) count = ship1Count;
        else if (shipName == ship2.name) count = ship2Count;
        else if (shipName == ship3.name) count = ship3Count;
        else if (shipName == ship4.name) count = ship4Count;
        else if (shipName == ship5.name) count = ship5Count;
        return count;
    }
}