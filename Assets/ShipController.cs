using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private Ship submarine;
    [SerializeField] private int submarineCount = 2;
    
    [SerializeField] private Ship destroyer;
    [SerializeField] private int destroyerCount = 2;

    [SerializeField] private Ship cruiser;
    [SerializeField] private int cruiserCount = 1;
    
    [SerializeField] private Ship battleship;
    [SerializeField] private int battleshipCount = 1;

    [SerializeField] private Ship carrier;
    [SerializeField] private int carrierCount = 2;

    [SerializeField] private Ship test;
    [SerializeField] private Ship test2;
    
    private static Ship flyingShip;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            StartStopPlacingShip(submarine);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            StartStopPlacingShip(destroyer);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            StartStopPlacingShip(cruiser);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            StartStopPlacingShip(battleship);
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            StartStopPlacingShip(carrier);
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            StartStopPlacingShip(test);
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            StartStopPlacingShip(test2);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            flyingShip?.Rotate();
        }
    }

    private static void StartStopPlacingShip(Ship ship)
    {
        GridObjectRemover.StopPlacingRemover();
        
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
    
    private static void StartPlacingShip(Ship shipPrefab)
    {
        flyingShip = Instantiate(shipPrefab);
    }

    public static void StopPlacingShip()
    {
        if (flyingShip != null) Destroy(flyingShip.gameObject);
        flyingShip = null;
    }
    
    public static void MoveFlyingShip(int x, int y)
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

        (x, y) = RaycastGround.LimitPlayerObjectPlacement(x, y, flyingShip.size.x, flyingShip.size.y);
   
        flyingShip.transform.position = new Vector3(x, 0, y);
        
        bool available = !PlaceIsTaken(x, y);
        flyingShip.SetTransparent(available);
        
        if (available && Input.GetMouseButtonDown(0))
        {
            PlaceShip(x, y);
        }
    }
    
    private static bool PlaceIsTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingShip.size.x; x++)
        {
            for (int y = 0; y < flyingShip.size.y; y++)
            {
                if (Grid.GetGridObject(placeX + x, placeY + y) != null) return true;
            }
        }

        return false;
    }

    private static void PlaceShip(int placeX, int placeY)
    {
        for (int x = 0; x < flyingShip.size.x; x++)
        {
            for (int y = 0; y < flyingShip.size.y; y++)
            {
                Grid.SetGridObject(placeX + x, placeY + y, flyingShip);
            }
        }
        
        flyingShip.SetNormal();
        flyingShip = null;
    }
}