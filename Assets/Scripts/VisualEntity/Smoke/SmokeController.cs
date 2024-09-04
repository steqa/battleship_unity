using UnityEngine;

public class SmokeController : VisualEntityController
{
    [SerializeField] private Smoke smoke;
    [SerializeField] private Transform smokesContainer;

    public void PlaceSmoke(int x, int y)
    {
        PlaceVisualEntity(smoke, x, y, smokesContainer);
    }
}