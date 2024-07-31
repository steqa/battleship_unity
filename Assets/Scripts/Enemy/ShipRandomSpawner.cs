using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShipRandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ground;
    [SerializeField] private Grid grid;
    [SerializeField] private ShipController shipController;
    [SerializeField] private EntityController entityController;
    [SerializeField] private Button spawnButton;
    [SerializeField] private Button clearButton;

    private bool spawned;

    private void Start()
    {
        DisableClearButton();
    }

    private void DisableSpawnButton()
    {
        spawnButton.interactable = false;
    }
    
    private void EnableSpawnButton()
    {
        spawnButton.interactable = true;
    }
    
    private void DisableClearButton()
    {
        clearButton.interactable = false;
    }
    
    private void EnableClearButton()
    {
        clearButton.interactable = true;
    }
    
    public void SpawnRandomizeShips()
    {
        if (spawned) return;
        DisableSpawnButton();
        EnableClearButton();
        spawned = true;
        (int gridSizeX, int gridSizeY) = grid.GetGridSize();
        Ship[] ships = shipController.GetShips();
        foreach (Ship s in ships)
        {
            int shipCount = shipController.GetShipMaxCount(s.name);
            for (int c = 0; c < shipCount; c++)
            {
                var ship = Instantiate(s);
                (int xPosition, int yPosition) = (0, 0);
                bool validPlace = false;
                do
                {
                    xPosition = Random.Range(0, gridSizeX);
                    yPosition = Random.Range(0, gridSizeY);
                        
                    int rotateCount = Random.Range(0, 3);
                    for (int r = 0; r < rotateCount; r++)
                    {
                        ship.Rotate();
                    }
                    
                    (xPosition, yPosition) = entityController.LimitCoordinates(xPosition, yPosition, ship, gridSizeX, gridSizeY);
                    (xPosition, yPosition) = entityController.CorrectCoordinates(xPosition, yPosition, ship);
                    ship.x = xPosition;
                    ship.y = yPosition;

                    if (!grid.PlaceIsTaken(ship)) validPlace = true;
                    
                } while (!validPlace);
                
                float worldX = xPosition + 0.5f - (gridSizeX / 2);
                float worldY = yPosition + 0.5f - (gridSizeY / 2);
                        
                Vector3 newPosition = new Vector3(worldX, 0, worldY);
                ship.transform.position = ground.transform.TransformPoint(newPosition);

                grid.SetGridEntity(ship);
            }
        }
    }

    public void ClearGrid()
    {
        if (!spawned) return;
        EnableSpawnButton();
        DisableClearButton();
        spawned = false;
        grid.ClearGrid();
    }
}
