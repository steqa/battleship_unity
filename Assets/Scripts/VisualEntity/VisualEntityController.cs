using Unity.VisualScripting;
using UnityEngine;

public class VisualEntityController : MonoBehaviour
{
    [SerializeField] private VisualGrid visualGrid;

    public void PlaceVisualEntity(VisualEntity visualEntity, int x, int y, Transform container)
    {
        (int gridSizeX, int gridSizeY) = visualGrid.GetGridSize();
        float worldX = x + 0.5f - gridSizeX / 2 + visualGrid.transform.position.x;
        float worldY = y + 0.5f - gridSizeY / 2 + visualGrid.transform.position.z;
        VisualEntity entity = Instantiate(visualEntity, new Vector3(worldX, 0, worldY), Quaternion.identity);
        entity.transform.parent = container;
        (entity.X, entity.Y) = (x, y);
        visualGrid.SetGridEntity(entity);
    }

    public void RemoveVisualEntity(int x, int y)
    {
        VisualEntity entity = visualGrid.GetGridEntity(x, y);
        visualGrid.DeleteGridEntity(x, y);
        Destroy(entity.GameObject());
    }
}