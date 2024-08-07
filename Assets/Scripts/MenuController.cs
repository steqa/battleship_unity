using TMPro;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private string shipsPlacementScene;
    [SerializeField] private TMP_InputField ipInput;

    public void JoinGame()
    {
        DataHolder.Holder.IP = ipInput.text;
        sceneController.LoadScene(shipsPlacementScene);
    }
}