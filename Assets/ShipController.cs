using System;
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

    private Ship flyingShip;

    [SerializeField] private GameObject playerGround;
    private Grid playerGrid;
    
    private GridObjectRemover gridObjectRemover;
    private RaycastGround raycastGround;

    private void Awake()
    {
        playerGrid = playerGround.GetComponent<Grid>();
        gridObjectRemover = GetComponent<GridObjectRemover>();
        raycastGround = GetComponent<RaycastGround>();
    }

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
            
        if (Input.GetKeyDown(KeyCode.R))
        {
            flyingShip?.Rotate();
        }

        if (Input.GetMouseButtonDown(1))
        {
            StopPlacingShip();
        }
    }

    private void StartStopPlacingShip(Ship ship)
    {
        gridObjectRemover.StopPlacingRemover();
        
        if (flyingShip != null)
        {
            if (flyingShip.name == ship.name)
            {
                StopPlacingShip();
                return;
            }
            StopPlacingShip();
        }

        if (ship != null) StartPlacingShip(ship);
    }
    
    private void StartPlacingShip(Ship shipPrefab)
    {
        flyingShip = Instantiate(shipPrefab);
    }

    public void StopPlacingShip()
    {
        if (flyingShip != null) Destroy(flyingShip.gameObject);
        flyingShip = null;
    }
    
    public void MoveFlyingShip(int x, int y)
    {
        if (flyingShip == null) return;
        
        switch (flyingShip.direction)
        {
            case 1:
                y -= flyingShip.lenght / 2;
                if (flyingShip.lenght % 2 == 0) y += 1;
                
                x -= flyingShip.width / 2;
                if (flyingShip.width % 2 == 0) x += 1;

                break;
            case 2:
                x -= flyingShip.lenght / 2;
                if (flyingShip.lenght % 2 == 0) x += 1;
                
                y -= flyingShip.width / 2;
                if (flyingShip.width % 2 == 0) y += 1;
                
                break;
            case 3:
                y -= flyingShip.lenght / 2;

                x -= flyingShip.width / 2;
                if (flyingShip.width % 2 == 0) x += 1;
                
                break;
            case 4:
                x -= flyingShip.lenght / 2;
                
                y -= flyingShip.width / 2;
                if (flyingShip.width % 2 == 0) y += 1;
                
                break;
            default:
                Debug.LogError("Invalid direction");
                break;
        }

        (x, y) = raycastGround.LimitPlayerObjectPlacement(x, y, flyingShip.size.x, flyingShip.size.y);

        (float relativeX, float relativeY) = raycastGround.GetRelativePosition(x, y);
        Vector3 relativePosition = new Vector3(relativeX, 0, relativeY);
        flyingShip.transform.position = playerGround.transform.TransformPoint(relativePosition);
        
        bool available = !PlaceIsTaken(x, y);
        flyingShip.SetTransparent(available);
        
        if (available && Input.GetMouseButtonDown(0))
        {
            PlaceShip(x, y);
        }
    }
    
    private bool PlaceIsTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingShip.size.x; x++)
        {
            for (int y = 0; y < flyingShip.size.y; y++)
            {
                if (playerGrid.GetGridObject(placeX + x, placeY + y) != null) return true;
            }
        }

        return false;
    }

    private void PlaceShip(int placeX, int placeY)
    {
        for (int x = 0; x < flyingShip.size.x; x++)
        {
            for (int y = 0; y < flyingShip.size.y; y++)
            {
                playerGrid.SetGridObject(placeX + x, placeY + y, flyingShip);
            }
        }
        
        flyingShip.SetNormal();
        flyingShip = null;
    }
}
