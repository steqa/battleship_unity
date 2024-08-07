using UnityEngine;

public class RaycastGround : MonoBehaviour
{
    [SerializeField] private GameObject playerGround;

    [SerializeField] private LayerMask groundLayerMask;

    private Camera _mainCamera;
    private Vector3 _worldPosition;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayerMask))
        {
            _worldPosition = ray.GetPoint(hit.distance);

            if (hit.transform.gameObject == playerGround)
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
        }
    }

    public (int x, int y) GetPositionOnGrid(GameGrid gameGrid)
    {
        (float gridSizeX, float gridSizeY) = gameGrid.GetGridSize();

        int x = Mathf.RoundToInt(_worldPosition.x - 0.5f + gridSizeX / 2 - gameGrid.transform.position.x);
        int y = Mathf.RoundToInt(_worldPosition.z - 0.5f + gridSizeY / 2 - gameGrid.transform.position.z);

        return (x, y);
    }
}