using System.Collections;
using UnityEngine;

public class PrefabColor : MonoBehaviour
{
    [SerializeField] private Color startColor;
    [SerializeField] private Color warningColor;
    [SerializeField] private float warningBlinkDuration = 0.5f;

    private bool _isWarningBlinking;

    private void Start()
    {
        PrefabItem.ChangeColor(transform, startColor);
    }

    public void BlinkWarningColor()
    {
        if (!_isWarningBlinking) StartCoroutine(Blink_());
    }

    private IEnumerator Blink_()
    {
        _isWarningBlinking = true;
        PrefabItem.ChangeColor(transform, warningColor);
        yield return new WaitForSeconds(warningBlinkDuration);
        PrefabItem.ChangeColor(transform, startColor);
        _isWarningBlinking = false;
    }
}