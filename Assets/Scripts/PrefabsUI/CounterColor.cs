using System.Collections;
using TMPro;
using UnityEngine;

public class CounterColor : MonoBehaviour
{
    [SerializeField] private Color startFontColor;
    [SerializeField] private FontStyles startFontStyle;
    [SerializeField] private Color warningFontColor;
    [SerializeField] private FontStyles warningFontStyle;
    [SerializeField] private float warningBlinkDuration = 0.5f;

    private bool _isWarningBlinking;

    private void Start()
    {
        PrefabItem.ChangeFontColor(transform, startFontColor);
        PrefabItem.ChangeFontStyle(transform, startFontStyle);
    }

    public void BlinkWarningColor()
    {
        if (!_isWarningBlinking) StartCoroutine(Blink_());
    }

    private IEnumerator Blink_()
    {
        _isWarningBlinking = true;
        PrefabItem.ChangeFontColor(transform, warningFontColor);
        PrefabItem.ChangeFontStyle(transform, warningFontStyle);
        yield return new WaitForSeconds(warningBlinkDuration);
        PrefabItem.ChangeFontColor(transform, startFontColor);
        PrefabItem.ChangeFontStyle(transform, startFontStyle);
        _isWarningBlinking = false;
    }
}