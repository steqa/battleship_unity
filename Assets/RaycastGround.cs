using System;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class RaycastGround : MonoBehaviour
{
    [SerializeField] private GameObject playerGround;
    [SerializeField] private LayerMask groundLayerMask;
    
    private Camera mainCamera;

    private Grid playerGrid;

    private ShipController shipController;
    private GridObjectRemover gridObjectRemover;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        playerGrid = playerGround.GetComponent<Grid>();
        shipController = GetComponent<ShipController>();
        gridObjectRemover = GetComponent<GridObjectRemover>();
    }

    private void Update()
    {
        RaycastHit hit;
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            Vector3 worldPosition = ray.GetPoint(hit.distance);

            (int x, int y) = GetGridPosition(worldPosition.x, worldPosition.z);
            
            if (hit.transform.gameObject == playerGround)
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
            }

            shipController.MoveFlyingShip(x, y);
            gridObjectRemover.MoveFlyingRemover(x, y);
        }
    }

    private (int x, int y) GetGridPosition(float x, float y)
    {
        (int gridSizeX, int gridSizeY) = playerGrid.GetGridSize();
        int newX = Mathf.RoundToInt(x - 0.5f + (gridSizeX / 2) - playerGrid.transform.position.x);
        int newY = Mathf.RoundToInt(y - 0.5f + (gridSizeY / 2) - playerGrid.transform.position.z);
        return (newX, newY);
    }
    
    public (float x, float y) GetRelativePosition(float x, float y)
    {
        (int gridSizeX, int gridSizeY) = playerGrid.GetGridSize();
        return (x + 0.5f - (gridSizeX / 2), y + 0.5f - (gridSizeY / 2));
    }

    public (int x, int y) LimitPlayerObjectPlacement(int x, int y, int objectSizeX, int objectSizeY)
    {
        (int gridSizeX, int gridSizeY) = playerGrid.GetGridSize();
        
        if (x < 0)
        {
            x = 0;
        } else if (x > gridSizeX - objectSizeX)
        {
            x = gridSizeX - objectSizeX;
        }
        
        if (y < 0)
        {
            y = 0;
        } else if (y > gridSizeY - objectSizeY)
        {
            y = gridSizeY - objectSizeY;
        }
        
        return (x, y);
    }
}
