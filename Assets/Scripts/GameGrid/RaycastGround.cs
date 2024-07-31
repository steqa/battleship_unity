using System;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class RaycastGround : MonoBehaviour
{
    [SerializeField] private GameObject playerGround;
    
    [SerializeField] private LayerMask groundLayerMask;
    
    private Camera mainCamera;
    private Vector3 worldPosition;
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        RaycastHit hit;
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            worldPosition = ray.GetPoint(hit.distance);

            if (hit.transform.gameObject == playerGround)
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
            }
        }
    }

    public (int x, int y) GetPositionOnGrid(GameGrid gameGrid)
    {
        (float gridSizeX, float gridSizeY) = gameGrid.GetGridSize();

        int x = Mathf.RoundToInt(worldPosition.x - 0.5f + (gridSizeX / 2) - gameGrid.transform.position.x);
        int y = Mathf.RoundToInt(worldPosition.z - 0.5f + (gridSizeY / 2) - gameGrid.transform.position.z);

        return (x, y);
    }
}
