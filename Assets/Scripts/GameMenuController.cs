using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    public static bool AvailableForOpening;
    [SerializeField] private Canvas menu;

    private void Awake()
    {
        AvailableForOpening = true;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && AvailableForOpening) i_EnableMenu();
    }

    public void i_EnableMenu()
    {
        menu.enabled = !menu.enabled;
        DataHolder.HandleActions = !menu.enabled;
        if (menu.enabled) EntityController.StopPlacingEntity();
    }

    public void i_Exit()
    {
        CustomWebSocketClient.CloseConnection();
        SceneManager.LoadScene("MainMenuScene");
        DataHolder.HandleActions = false;
    }
}