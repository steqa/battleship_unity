using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryDefeatScreen : MonoBehaviour
{
    private static GameObject _victoryScreen;
    private static GameObject _defeatScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject defeatScreen;

    private void Awake()
    {
        _victoryScreen = victoryScreen;
        _defeatScreen = defeatScreen;
    }

    public static void EnableVictoryScreen()
    {
        _victoryScreen.SetActive(true);
        DataHolder.HandleActions = false;
        GameMenuController.AvailableForOpening = false;
    }

    public static void EnableDefeatScreen()
    {
        _defeatScreen.SetActive(true);
        DataHolder.HandleActions = false;
        GameMenuController.AvailableForOpening = false;
    }

    public void i_Exit()
    {
        CustomWebSocketClient.CloseConnection();
        SceneManager.LoadScene("MainMenuScene");
    }
}