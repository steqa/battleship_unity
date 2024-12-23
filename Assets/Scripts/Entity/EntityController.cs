using Unity.VisualScripting;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    private static Entity _flyingEntity;

    public static Color NormalColor;
    public static Color SuccessColor;
    public static Color WarningColor;

    public static bool FlyingEntityAvailableForUse;
    [SerializeField] private GameObject ground;
    [SerializeField] private RaycastGround raycastGround;

    [SerializeField] private PlacementGrid placementGrid;

    [SerializeField] private GameObject shipsContainer;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color successColor;
    [SerializeField] private Color warningColor;

    private void Awake()
    {
        NormalColor = normalColor;
        SuccessColor = successColor;
        WarningColor = warningColor;
    }

    private void Update()
    {
        MoveFlyingEntity();
        if (Input.GetMouseButtonDown(1)) StopPlacingEntity();
        if (_flyingEntity && _flyingEntity.isRotatable && Input.GetKeyDown(KeyCode.R)) _flyingEntity.Rotate();
    }

    public Entity GetFlyingEntity()
    {
        return _flyingEntity;
    }

    public void StartStopPlacingEntity(Entity entity)
    {
        if (_flyingEntity != null && _flyingEntity.name == entity.name)
        {
            StopPlacingEntity();
            return;
        }

        StopPlacingEntity();
        if (entity != null) StartPlacingEntity(entity);
    }

    private void StartPlacingEntity(Entity entity)
    {
        _flyingEntity = Instantiate(entity, new Vector3(0, 50, 0), Quaternion.identity);
        _flyingEntity.transform.parent = shipsContainer.transform;
    }

    public static void StopPlacingEntity()
    {
        if (_flyingEntity != null) Destroy(_flyingEntity.gameObject);
        _flyingEntity = null;
        FlyingEntityAvailableForUse = false;
    }

    public bool PlaceEntity()
    {
        bool available = !placementGrid.PlaceIsTaken(_flyingEntity);
        if (available)
        {
            placementGrid.SetGridEntity(_flyingEntity);
            _flyingEntity = null;
            FlyingEntityAvailableForUse = false;
            return true;
        }

        return false;
    }

    public void RemoveEntity(Entity entity)
    {
        placementGrid.DeleteGridEntity(entity);
        Destroy(entity.GameObject());
        Destroy(_flyingEntity.GameObject());
        _flyingEntity = null;
        FlyingEntityAvailableForUse = false;
    }

    private void MoveFlyingEntity()
    {
        if (!_flyingEntity) return;
        _flyingEntity.transform.position = ground.transform.TransformPoint(GetNewEntityPosition(_flyingEntity));
        FlyingEntityAvailableForUse = true;
    }

    private Vector3 GetNewEntityPosition(Entity entity)
    {
        (int gridSizeX, int gridSizeY) = placementGrid.GetGridSize();

        (int x, int y) = raycastGround.GetPositionOnGrid(placementGrid.GameObject());
        (x, y) = LimitCoordinates(x, y, entity, gridSizeX, gridSizeY);
        (x, y) = CorrectCoordinates(x, y, entity);
        entity.X = x;
        entity.Y = y;

        float worldX = x + 0.5f - gridSizeX / 2;
        float worldY = y + 0.5f - gridSizeY / 2;

        return new Vector3(worldX, 0, worldY);
    }

    public (int x, int y) CorrectCoordinates(int x, int y, Entity entity)
    {
        switch (entity.Direction)
        {
            case 1:
                y -= (entity.Length - 1) / 2;
                x -= (entity.Width - 1) / 2;
                break;
            case 2:
                y -= entity.Width / 2;
                x -= (entity.Length - 1) / 2;
                break;
            case 3:
                y -= entity.Length / 2;
                x -= entity.Width / 2;
                break;
            case 4:
                y -= (entity.Width - 1) / 2;
                x -= entity.Length / 2;
                break;
        }

        return (x, y);
    }

    public (int x, int y) LimitCoordinates(int x, int y, Entity entity, int gridSizeX, int gridSizeY)
    {
        var leftOffset = 0;
        var rightOffset = 0;
        var topOffset = 0;
        var bottomOffset = 0;

        int topSide = (entity.Width - 1) / 2;
        int rightSide = entity.Length / 2;
        int bottomSide = entity.Width / 2;
        int leftSide = (entity.Length - 1) / 2;

        switch (entity.Direction)
        {
            case 1:
                topOffset = topSide;
                rightOffset = rightSide;
                bottomOffset = bottomSide;
                leftOffset = leftSide;
                break;
            case 2:
                rightOffset = topSide;
                bottomOffset = rightSide;
                leftOffset = bottomSide;
                topOffset = leftSide;
                break;
            case 3:
                bottomOffset = topSide;
                leftOffset = rightSide;
                topOffset = bottomSide;
                rightOffset = leftSide;
                break;
            case 4:
                leftOffset = topSide;
                topOffset = rightSide;
                rightOffset = bottomSide;
                bottomOffset = leftSide;
                break;
        }

        if (y < leftOffset) y = leftOffset;
        else if (y > gridSizeY - rightOffset - 1) y = gridSizeY - rightOffset - 1;

        if (x < topOffset) x = topOffset;
        else if (x > gridSizeX - bottomOffset - 1) x = gridSizeX - bottomOffset - 1;

        return (x, y);
    }
}