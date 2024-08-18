using TMPro;
using UnityEngine;

public class PrefabItem : MonoBehaviour
{
    public string prefabName;
    [SerializeField] private BtnColor btnColor;
    [SerializeField] private PrefabColor prefabColor;
    [SerializeField] private CounterColor counterColor;
    [SerializeField] private BackgroundColor backgroundColor;

    public void BlinkWarning()
    {
        if (btnColor != null) btnColor.BlinkWarningColor();
        if (prefabColor != null) prefabColor.BlinkWarningColor();
        if (counterColor != null) counterColor.BlinkWarningColor();
        if (backgroundColor != null) backgroundColor.BlinkWarningColor();
    }

    public static void ChangeColor(Transform obj, Color color)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        foreach (Material mat in renderer.materials)
            mat.color = color;
    }

    public static void ChangeFontColor(Transform obj, Color color)
    {
        TextMeshPro[] texts = obj.GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro text in texts)
            text.color = color;
    }

    public static void ChangeFontStyle(Transform obj, FontStyles style)
    {
        TextMeshPro[] texts = obj.GetComponentsInChildren<TextMeshPro>();
        foreach (TextMeshPro text in texts)
            text.fontStyle = style;
    }
}