using System;
using Unity.Burst.Intrinsics;
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

    [SerializeField] private GameObject playerGround;
    [SerializeField] private ShipCounter shipCounter;
    [SerializeField] private EntityController entityController;

    
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            StartStopPlacingShip(ship1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            StartStopPlacingShip(ship2);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            StartStopPlacingShip(ship3);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            StartStopPlacingShip(ship4);
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            StartStopPlacingShip(ship5);
        }
        
        Entity flyingEntity = entityController.GetFlyingEntity();
        if (flyingEntity is Ship ship)
        {
            ColorizeShip(ship);
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                ship.Rotate();
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                PlaceShip(ship);
            }
        }
    }

    private void StartStopPlacingShip(Ship ship)
    {
        bool placementAvailable = shipCounter.LimitShipCount(ship.name);
        if (placementAvailable == false) return;
        
        entityController.StartStopPlacingEntity(ship);
    }
    
    private void PlaceShip(Ship ship)
    {
        ship.SetColorNormal();
        string shipName = entityController.GetFlyingEntity().name;
        bool isPlaced = entityController.PlaceEntity(ship.x, ship.y);
        if (isPlaced) shipCounter.AddShipCount(shipName);
    }

    private void ColorizeShip(Ship ship)
    {
        bool available = !entityController.PlaceIsTaken(ship.x, ship.y);
        ship.SetColorStatus(available);
    }
    
    public int GetShipMaxCount(string shipName)
    {
        int count = 0;
        if (shipName == ship1.name) count = ship1Count;
        else if (shipName == ship2.name) count = ship2Count;
        else if (shipName == ship3.name) count = ship3Count;
        else if (shipName == ship4.name) count = ship4Count;
        else if (shipName == ship5.name) count = ship5Count;
        return count;
    }
}
