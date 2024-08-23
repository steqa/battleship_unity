using UnityEngine;

public class SmokeController : MonoBehaviour
{
    [SerializeField] private VisualEntityController visualEntityController;
    [SerializeField] private Smoke smoke;

    public void i_Test()
    {
        ChangeVisualEntityToSmoke(5, 5);
    }

    public void ChangeVisualEntityToSmoke(int x, int y)
    {
        visualEntityController.RemoveVisualEntity(x, y);
        visualEntityController.PlaceVisualEntity(smoke, x, y);
    }
}