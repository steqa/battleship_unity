using UnityEngine;

public class RaycastGround : MonoBehaviour
{
    [SerializeField] private GameObject playerGround;
    [SerializeField] private GameObject enemyGround;
    [SerializeField] private LayerMask groundLayerMask;

    private Camera _mainCamera;
    private Vector3 _worldPosition;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (DataHolder.HandleActions)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayerMask))
            {
                _worldPosition = ray.GetPoint(hit.distance);

                if (hit.transform.gameObject == playerGround)
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
                else if (hit.transform.gameObject == enemyGround)
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
            }
        }
    }

    public (int x, int y) GetPositionOnGrid(GameObject grid)
    {
        float gridSizeX = DataHolder.GridSize.x;
        float gridSizeY = DataHolder.GridSize.y;
        Vector3 localPosition = grid.transform.InverseTransformPoint(_worldPosition);
        int x = Mathf.RoundToInt(localPosition.x - 0.5f + gridSizeX / 2);
        int y = Mathf.RoundToInt(localPosition.z - 0.5f + gridSizeY / 2);
        return (x, y);
    }
}