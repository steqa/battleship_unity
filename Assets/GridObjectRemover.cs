using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

public class GridObjectRemover : MonoBehaviour
{
    [SerializeField] private GameObject remover;
    
    private GameObject flyingRemover;

    private Object lastHoveredObject;
    
    [SerializeField] private GameObject playerGround;
    private Grid playerGrid;
    
    private ShipController shipController;
    private RaycastGround raycastGround;
    
    private void Awake()
    {
        playerGrid = playerGround.GetComponent<Grid>();
        shipController = GetComponent<ShipController>();
        raycastGround = GetComponent<RaycastGround>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartStopPlacingRemover();
        }
    }
    
    private void StartStopPlacingRemover()
    {
        shipController.StopPlacingShip();

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
    
    public void StopPlacingRemover()
    {
        if (flyingRemover != null) Destroy(flyingRemover.gameObject);
        flyingRemover = null;
    }
    
    public void MoveFlyingRemover(int x, int y)
    {
        if (flyingRemover == null)
        {
            if (lastHoveredObject != null)
            {
                if (lastHoveredObject is Ship lastHoveredShip)
                {
                    lastHoveredShip.SetNormal();
                    lastHoveredObject = null;
                    return;
                }
            }
            else return;
        }

        (x, y) = raycastGround.LimitPlayerObjectPlacement(x, y, 1, 1);
        
        (float relativeX, float relativeY) = raycastGround.GetRelativePosition(x, y);
        Vector3 relativePosition = new Vector3(relativeX, 0, relativeY);
        flyingRemover.transform.position = playerGround.transform.TransformPoint(relativePosition);

        if (Input.GetMouseButtonDown(0))
        {
            RemoveObject(x, y);
        }

        var gridObject = playerGrid.GetGridObject(x, y);
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
    
    private void RemoveObject(int x, int y)
    {
        var ship = playerGrid.GetGridObject(x, y);
        playerGrid.DeleteGridObject(ship);
        Destroy(ship.GameObject());
    }
}
