using Newtonsoft.Json.Linq;
using Player;
using Serialize;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using WebsocketMessage;
using Button = UnityEngine.UI.Button;

public class FireController : MonoBehaviour
{
    [SerializeField] private TMP_Text turnText;
    [SerializeField] private Button fireBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private VisualGrid visualGrid;
    [SerializeField] private GameObject enemyGround;
    [SerializeField] private EnemyGameGrid enemyGameGrid;
    [SerializeField] private RaycastGround raycastGround;
    [SerializeField] private GameObject scope;
    private int _gridX;
    private int _gridY;
    private bool _moveScope = true;

    private void Update()
    {
        if (DataHolder.PlayerTurn && !turnText.GameObject().activeSelf && DataHolder.HandleActions)
        {
            turnText.GameObject().SetActive(true);
            scope.GameObject().SetActive(true);
            i_StartMovingScope();
        }

        if (DataHolder.PlayerTurn)
        {
            MoveScope();
            if (Input.GetMouseButtonDown(0)) StopMovingScope();
        }
    }

    private void MoveScope()
    {
        if (!_moveScope) return;
        (int x, int y) = raycastGround.GetPositionOnGrid(enemyGround);
        if (0 <= x && x <= 9 && 0 <= y && y <= 9)
        {
            VisualEntity entity = visualGrid.GetGridEntity(x, y);
            if (entity is Cloud cloud)
            {
                (float worldX, float worldY) = EnemyGameGrid.GetWorldPositionOnGrid(x, y);
                scope.transform.position = new Vector3(worldX, 0, worldY);
                scope.GameObject().SetActive(true);
                _gridX = x;
                _gridY = y;
            }
        }
        else
        {
            scope.GameObject().SetActive(false);
        }
    }

    private void StopMovingScope()
    {
        if (!scope.GameObject().activeSelf) return;
        _moveScope = false;
        fireBtn.GameObject().SetActive(true);
        cancelBtn.GameObject().SetActive(true);
    }

    public void i_StartMovingScope()
    {
        _moveScope = true;
        fireBtn.GameObject().SetActive(false);
        cancelBtn.GameObject().SetActive(false);
    }

    public void i_Fire()
    {
        int cell = enemyGameGrid.TwoToOneDimCoordinate(_gridX, _gridY);
        Entity entity = enemyGameGrid.GetGridEntity(_gridX, _gridY);
        var entityID = "";
        if (entity != null) entityID = entity.Uuid;
        var hit = new Hit { Cell = cell, EntityID = entityID };
        JObject hitJson = jObject.FromObject(hit);
        var request = new WsMessage
        {
            Type = WsRequestTypes.Hit,
            Detail = hitJson
        };
        CustomWebSocketClient.SendMessage(request);
        DataHolder.PlayerTurn = false;
        turnText.GameObject().SetActive(false);
        fireBtn.GameObject().SetActive(false);
        cancelBtn.GameObject().SetActive(false);
        scope.GameObject().SetActive(false);
    }
}