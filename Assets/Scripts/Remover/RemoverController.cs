using Unity.VisualScripting;
using UnityEngine;
using Object = System.Object;

public class RemoverController : MonoBehaviour
{
    [SerializeField] private Remover remover;

    [SerializeField] private ShipCounter shipCounter;
    [SerializeField] private EntityController entityController;
    [SerializeField] private Grid grid;
    
    private Entity lastHoveredEntity;
    
    private void Update()
    {
        if (lastHoveredEntity != null)
        {
            lastHoveredEntity.SetColorNormal();
            lastHoveredEntity = null;
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            entityController.StartStopPlacingEntity(remover);
        }
        
        Entity flyingEntity = entityController.GetFlyingEntity();
        if (flyingEntity is Remover rm)
        {
            ColorizeEntity(rm.x, rm.y);
            
            if (Input.GetMouseButtonDown(0))
            {
                RemoveEntity(rm.x, rm.y);
            }
        }
    }

    private void RemoveEntity(int x, int y)
    {
        Entity gridEntity = grid.GetGridEntity(x, y);
        if (gridEntity is Ship ship)
        {
            shipCounter.SubtractShipCount(ship.name);
        }
        
        entityController.RemoveEntity(gridEntity);
    }

    private void ColorizeEntity(int x, int y)
    {
        Entity gridEntity = grid.GetGridEntity(x, y);
        if (gridEntity != null)
        {
            gridEntity.SetColorStatus(false);
            lastHoveredEntity = gridEntity;
        }
    }
}
