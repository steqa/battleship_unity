using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{
    [SerializeField] private Color blinkColor;
    [SerializeField] private float blinkDuration;

    private bool _isBlinking;

    public void Blink(Graphic graphic)
    {
        if (!_isBlinking) StartCoroutine(Blink_(graphic));
    }

    private IEnumerator Blink_(Graphic graphic)
    {
        _isBlinking = true;

        var uimanager = graphic.GetComponentInParent<UIManager>();

        Color originalColor;

        if (uimanager) originalColor = uimanager.color;
        else originalColor = graphic.color;

        if (uimanager) uimanager.ChangeColor(blinkColor);
        else graphic.color = blinkColor;

        yield return new WaitForSeconds(blinkDuration);

        if (uimanager) uimanager.ChangeColor(originalColor);
        else graphic.color = originalColor;

        _isBlinking = false;
    }
}