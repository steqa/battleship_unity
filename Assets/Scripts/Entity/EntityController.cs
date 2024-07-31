using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    private Entity flyingEntity;
    
    [SerializeField] private GameObject ground;
    [SerializeField] private RaycastGround raycastGround;
    
    [SerializeField] private Grid grid;

    private bool isFlying = false;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StopPlacingEntity();
        }

        MoveFlyingEntity();
    }

    public Entity GetFlyingEntity()
    {
        return flyingEntity;
    }

    public void StartStopPlacingEntity(Entity entity)
    {
        if (flyingEntity != null && flyingEntity.name == entity.name)
        {
            StopPlacingEntity();
            return;
        }
        StopPlacingEntity();
        if (entity != null) StartPlacingEntity(entity);
    }

    private void StartPlacingEntity(Entity entity)
    {
        flyingEntity = Instantiate(entity);
        isFlying = true;
    }
    
    private void StopPlacingEntity()
    {
        if (flyingEntity != null) Destroy(flyingEntity.gameObject);
        flyingEntity = null;
        isFlying = false;
    }
    
    public bool PlaceEntity()
    {
        bool available = !grid.PlaceIsTaken(flyingEntity);
        if (available)
        {
            grid.SetGridEntity(flyingEntity);
            flyingEntity = null;
            isFlying = false;
            return true;
        }
     
        return false;
    }

    public void RemoveEntity(Entity entity)
    {
        grid.DeleteGridEntity(entity);
        Destroy(entity.GameObject());
    }
    
    private void MoveFlyingEntity()
    {
        if (!isFlying) return;

        (int gridSizeX, int gridSizeY) = grid.GetGridSize();
        
        (int x, int y) = raycastGround.GetPositionOnGrid(grid);
        (x, y) = LimitCoordinates(x, y, flyingEntity, gridSizeX, gridSizeY);
        (x, y) = CorrectCoordinates(x, y, flyingEntity);
        flyingEntity.x = x;
        flyingEntity.y = y;    
        
        float worldX = x + 0.5f - (gridSizeX / 2);
        float worldY = y + 0.5f - (gridSizeY / 2);
            
        Vector3 newPosition = new Vector3(worldX, 0, worldY);
        flyingEntity.transform.position = ground.transform.TransformPoint(newPosition);
    }
    
    public (int x, int y) CorrectCoordinates(int x, int y, Entity entity)
    {
        switch (entity.direction)
        {
            case 1:
                y -= (entity.lenght - 1) / 2;
                x -= (entity.width - 1) / 2;
                break;
            case 2:
                y -= entity.width / 2;
                x -= (entity.lenght - 1) / 2;
                break;
            case 3:
                y -= entity.lenght / 2;
                x -= entity.width / 2;
                break;
            case 4:
                y -= (entity.width - 1) / 2;
                x -= entity.lenght / 2;
                break;
        }

        return (x, y);
    }

    public (int x, int y) LimitCoordinates(int x, int y, Entity entity, int gridSizeX, int gridSizeY)
    {
        int leftOffset = 0;
        int rightOffset = 0;
        int topOffset = 0;
        int bottomOffset = 0;
        
        int topSide = (entity.width - 1) / 2;
        int rightSide = entity.lenght / 2;
        int bottomSide = entity.width / 2;
        int leftSide = (entity.lenght - 1) / 2;

        switch (entity.direction)
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
