using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    private Entity flyingEntity;
    
    [SerializeField] private ShipCounter shipCounter;
    
    [SerializeField] private GameObject playerGround;
    [SerializeField] private RaycastGround raycastGround;
    
    [SerializeField] private Grid playerGrid;

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
    
    public bool PlaceEntity(int placeX, int placeY)
    {
        bool available = !PlaceIsTaken(placeX, placeY);
        if (available)
        {
            for (int x = 0; x < flyingEntity.size.x; x++)
            {
                for (int y = 0; y < flyingEntity.size.y; y++)
                {
                    playerGrid.SetGridEntity(placeX + x, placeY + y, flyingEntity);
                }
            }
            flyingEntity = null;
            isFlying = false;
            return true;
        }
     
        return false;
    }

    public bool PlaceIsTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingEntity.size.x; x++)
        {
            for (int y = 0; y < flyingEntity.size.y; y++)
            {
                if (playerGrid.GetGridEntity(placeX + x, placeY + y) != null) return true;
            }
        }

        return false;
    }
    
    public void RemoveEntity(Entity entity)
    {
        playerGrid.DeleteGridEntity(entity);
        Destroy(entity.GameObject());
    }
    
    private void MoveFlyingEntity()
    {
        if (!isFlying) return;

        (int x, int y) = raycastGround.GetPositionOnGrid(playerGrid);
        (x, y) = LimitCoordinates(x, y);
        (x, y) = CorrectCoordinates(x, y);
            
        (float gridSizeX, float gridSizeY) = playerGrid.GetGridSize();
        float worldX = x + 0.5f - (gridSizeX / 2);
        float worldY = y + 0.5f - (gridSizeY / 2);
            
        Vector3 newPosition = new Vector3(worldX, 0, worldY);
        flyingEntity.transform.position = playerGround.transform.TransformPoint(newPosition);
        flyingEntity.x = x;
        flyingEntity.y = y;
    }
    
    private (int x, int y) CorrectCoordinates(int x, int y)
    {
        switch (flyingEntity.direction)
        {
            case 1:
                y -= (flyingEntity.lenght - 1) / 2;
                x -= (flyingEntity.width - 1) / 2;
                break;
            case 2:
                y -= flyingEntity.width / 2;
                x -= (flyingEntity.lenght - 1) / 2;
                break;
            case 3:
                y -= flyingEntity.lenght / 2;
                x -= flyingEntity.width / 2;
                break;
            case 4:
                y -= (flyingEntity.width - 1) / 2;
                x -= flyingEntity.lenght / 2;
                break;
        }

        return (x, y);
    }

    private (int x, int y) LimitCoordinates(int x, int y)
    {
        int leftOffset = 0;
        int rightOffset = 0;
        int topOffset = 0;
        int bottomOffset = 0;
        
        int topSide = (flyingEntity.width - 1) / 2;
        int rightSide = flyingEntity.lenght / 2;
        int bottomSide = flyingEntity.width / 2;
        int leftSide = (flyingEntity.lenght - 1) / 2;

        switch (flyingEntity.direction)
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
        
        (int gridSizeX, int gridSizeY) = playerGrid.GetGridSize();

        if (y < leftOffset) y = leftOffset;
        else if (y > gridSizeY - rightOffset - 1) y = gridSizeY - rightOffset - 1;

        if (x < topOffset) x = topOffset;
        else if (x > gridSizeX - bottomOffset - 1) x = gridSizeX - bottomOffset - 1;

        return (x, y);
    }
}
