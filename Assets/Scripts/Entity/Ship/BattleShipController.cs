using UnityEngine;

public class BattleShipController : MonoBehaviour
{
    private static Ship Ship1;
    private static Ship Ship2;
    private static Ship Ship3;
    private static Ship Ship4;
    private static Ship Ship5;
    [SerializeField] private Ship ship1;
    [SerializeField] private Ship ship2;
    [SerializeField] private Ship ship3;
    [SerializeField] private Ship ship4;
    [SerializeField] private Ship ship5;

    private void Awake()
    {
        Ship1 = ship1;
        Ship2 = ship2;
        Ship3 = ship3;
        Ship4 = ship4;
        Ship5 = ship5;
    }

    public static Ship GetShipBySize(int size)
    {
        return size switch
        {
            1 => Ship1,
            2 => Ship2,
            3 => Ship3,
            4 => Ship4,
            5 => Ship5,
            _ => null
        };
    }
}