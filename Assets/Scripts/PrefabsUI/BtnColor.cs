using System.Collections;
using UnityEngine;

public class BtnColor : MonoBehaviour
{
    [SerializeField] private Color startBtnColor;
    [SerializeField] private Color startTextColor;
    [SerializeField] private Color warningBtnColor;
    [SerializeField] private Color warningTextColor;
    [SerializeField] private float warningBlinkDuration = 0.5f;

    private bool _isWarningBlinking;

    private void Start()
    {
        PrefabItem.ChangeColor(transform, startBtnColor);
        PrefabItem.ChangeFontColor(transform, startTextColor);
    }

    public void BlinkWarningColor()
    {
        if (!_isWarningBlinking) StartCoroutine(Blink_());
    }

    private IEnumerator Blink_()
    {
        _isWarningBlinking = true;
        PrefabItem.ChangeColor(transform, warningBtnColor);
        PrefabItem.ChangeFontColor(transform, warningTextColor);
        yield return new WaitForSeconds(warningBlinkDuration);
        PrefabItem.ChangeColor(transform, startBtnColor);
        PrefabItem.ChangeFontColor(transform, startTextColor);
        _isWarningBlinking = false;
    }
}