using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    [NonSerialized] public static bool MenuIsOpen;
    [SerializeField] private Canvas menu;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) i_EnableMenu();
    }

    public void i_EnableMenu()
    {
        menu.enabled = !menu.enabled;
        MenuIsOpen = menu.enabled;
        if (menu.enabled) EntityController.StopPlacingEntity();
    }

    public void i_Exit()
    {
        CustomWebSocketClient.CloseConnection();
        SceneManager.LoadScene("MainMenuScene");
        MenuIsOpen = false;
    }
}