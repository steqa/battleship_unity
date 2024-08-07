using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Color color;

    private void Awake()
    {
        ChangeColor(color);
    }

    public void ChangeColor(Color newColor)
    {
        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image img in images) img.color = newColor;

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI txt in texts) txt.color = newColor;
    }
}