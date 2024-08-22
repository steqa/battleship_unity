using UnityEngine;

public class RemoverController : MonoBehaviour
{
    [SerializeField] private Remover remover;

    [SerializeField] private ShipCounter shipCounter;
    [SerializeField] private EntityController entityController;
    [SerializeField] private GameGrid grid;

    private Entity _lastHoveredEntity;

    private void Update()
    {
        if (_lastHoveredEntity != null)
        {
            _lastHoveredEntity.SetColorNormal();
            _lastHoveredEntity = null;
        }

        if (Input.GetKeyDown(KeyCode.C) && !GameMenuController.MenuIsOpen)
            entityController.StartStopPlacingEntity(remover);

        Entity flyingEntity = entityController.GetFlyingEntity();
        if (flyingEntity is Remover rm)
        {
            ColorizeEntity(rm.X, rm.Y);
            if (Input.GetMouseButtonDown(0) && EntityController.FlyingEntityAvailableForUse) RemoveEntity(rm.X, rm.Y);
        }
    }

    private void RemoveEntity(int x, int y)
    {
        Entity gridEntity = grid.GetGridEntity(x, y);
        if (gridEntity is Ship ship) shipCounter.SubtractShipCount(ship.name);

        entityController.RemoveEntity(gridEntity);
    }

    private void ColorizeEntity(int x, int y)
    {
        Entity gridEntity = grid.GetGridEntity(x, y);
        if (gridEntity != null)
        {
            gridEntity.SetColorStatus(false);
            _lastHoveredEntity = gridEntity;
        }
    }
}