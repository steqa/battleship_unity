using UnityEngine;

public class RaycastGround : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);
                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                ShipController.MoveFlyingShip(x, y);
                GridObjectRemover.MoveFlyingRemover(x, y);
            }
    }

    public static (int x, int y) LimitPlayerObjectPlacement(int x, int y, int objectSizeX, int objectSizeY)
    {
        (int gridSizeX, int gridSizeY) = Grid.GetGridSize();
        
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
