using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShipRandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ground;
    [SerializeField] private GameGrid grid;
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
                int failureAttempts = 0;
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
                    if (failureAttempts > 50)
                    {
                        spawned = true;
                        Destroy(ship.GameObject());
                        ClearGrid();
                        SpawnRandomizeShips();
                        return;
                    }
                    failureAttempts += 1;
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
