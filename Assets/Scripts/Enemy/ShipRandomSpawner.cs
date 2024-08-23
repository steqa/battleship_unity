using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShipRandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ground;
    [SerializeField] private PlacementGrid grid;
    [SerializeField] private ShipController shipController;
    [SerializeField] private EntityController entityController;
    [SerializeField] private Button spawnButton;
    [SerializeField] private Button clearButton;

    private bool _spawned;

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
        if (_spawned) return;
        DisableSpawnButton();
        EnableClearButton();
        _spawned = true;
        (int gridSizeX, int gridSizeY) = grid.GetGridSize();
        Ship[] ships = shipController.GetShips();
        foreach (Ship s in ships)
        {
            int shipCount = shipController.GetShipMaxCount(s.name);
            for (var c = 0; c < shipCount; c++)
            {
                Ship ship = Instantiate(s);
                (var xPosition, var yPosition) = (0, 0);
                var validPlace = false;
                var failureAttempts = 0;
                do
                {
                    xPosition = Random.Range(0, gridSizeX);
                    yPosition = Random.Range(0, gridSizeY);

                    int rotateCount = Random.Range(0, 3);
                    for (var r = 0; r < rotateCount; r++) ship.Rotate();

                    (xPosition, yPosition) = entityController.LimitCoordinates(
                        xPosition, yPosition, ship, gridSizeX, gridSizeY
                    );
                    (xPosition, yPosition) = entityController.CorrectCoordinates(xPosition, yPosition, ship);
                    ship.X = xPosition;
                    ship.Y = yPosition;

                    if (!grid.PlaceIsTaken(ship)) validPlace = true;
                    if (failureAttempts > 50)
                    {
                        _spawned = true;
                        Destroy(ship.GameObject());
                        ClearGrid();
                        SpawnRandomizeShips();
                        return;
                    }

                    failureAttempts += 1;
                } while (!validPlace);

                float worldX = xPosition + 0.5f - gridSizeX / 2;
                float worldY = yPosition + 0.5f - gridSizeY / 2;

                var newPosition = new Vector3(worldX, 0, worldY);
                ship.transform.position = ground.transform.TransformPoint(newPosition);

                grid.SetGridEntity(ship);
            }
        }
    }

    public void ClearGrid()
    {
        if (!_spawned) return;
        EnableSpawnButton();
        DisableClearButton();
        _spawned = false;
        grid.ClearGrid();
    }
}