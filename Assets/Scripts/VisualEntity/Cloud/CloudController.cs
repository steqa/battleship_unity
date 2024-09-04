using UnityEngine;

public class CloudController : VisualEntityController
{
    [SerializeField] private Cloud cloud;
    [SerializeField] private Transform cloudsContainer;

    private void Start()
    {
        for (var x = 0; x < DataHolder.GridSize.x; x++)
        for (var y = 0; y < DataHolder.GridSize.y; y++)
            PlaceVisualEntity(cloud, x, y, cloudsContainer);
    }
}