using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField] private VisualGrid visualGrid;
    [SerializeField] private VisualEntityController visualEntityController;
    [SerializeField] private Cloud cloud;

    private void Start()
    {
        (int gridSizeX, int gridSizeY) = visualGrid.GetGridSize();
        for (var x = 0; x < gridSizeX; x++)
        for (var y = 0; y < gridSizeY; y++)
            visualEntityController.PlaceVisualEntity(cloud, x, y);
    }
}