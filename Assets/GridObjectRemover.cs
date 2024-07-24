using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

public class GridObjectRemover : MonoBehaviour
{
    [SerializeField] private GameObject remover;
    
    private static GameObject flyingRemover;

    private static Object lastHoveredObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartStopPlacingRemover();
        }
    }
    
    private void StartStopPlacingRemover()
    {
        ShipController.StopPlacingShip();

        if (flyingRemover != null)
        {
            StopPlacingRemover();
        }
        else
        {
            StartPlacingRemover();
        }
    }
    
    private void StartPlacingRemover()
    {
        flyingRemover = Instantiate(remover);
    }
    
    public static void StopPlacingRemover()
    {
        if (flyingRemover != null) Destroy(flyingRemover.gameObject);
        flyingRemover = null;
    }
    
    public static void MoveFlyingRemover(int x, int y)
    {
        if (flyingRemover == null) return;

        (x, y) = RaycastGround.LimitPlayerObjectPlacement(x, y, 1, 1);
        
        flyingRemover.transform.position = new Vector3(x, 0, y);

        if (Input.GetMouseButtonDown(0))
        {
            RemoveObject(x, y);
        }

        var gridObject = Grid.GetGridObject(x, y);
        if (gridObject is Ship ship)
        {
            if (lastHoveredObject == null)
            {
                ship.SetTransparent(false);
                lastHoveredObject = ship;
            }
            else
            {
                if (lastHoveredObject is Ship lastHoveredShip)
                {
                    lastHoveredShip.SetNormal();
                    lastHoveredObject = ship;
                    ship.SetTransparent(false);
                }
            }
        }
        else
        {
            if (lastHoveredObject is Ship lastHoveredShip)
            {
                lastHoveredShip.SetNormal();
                lastHoveredObject = null;
            }
        }
    }
    
    private static void RemoveObject(int x, int y)
    {
        var ship = Grid.GetGridObject(x, y);
        Grid.DeleteGridObject(ship);
        Destroy(ship.GameObject());
    }
}
