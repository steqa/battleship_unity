using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour
{
    [SerializeField] private Color blinkColor;
    [SerializeField] private float blinkDuration;
    
    private bool isBlinking = false;
    
    public void Blink(Graphic graphic)
    {
        if (!isBlinking)
        {
            StartCoroutine(Blink_(graphic));
        }
    }

    private IEnumerator Blink_(Graphic graphic)
    {
        isBlinking = true;

        var uimanager = graphic.GetComponentInParent<UIManager>();
        
        Color originalColor;

        if (uimanager) originalColor = uimanager.color;
        else originalColor = graphic.color;

        if (uimanager) uimanager.ChangeColor(blinkColor);
        else graphic.color = blinkColor;
        
        yield return new WaitForSeconds(blinkDuration);

        if (uimanager) uimanager.ChangeColor(originalColor);
        else graphic.color = originalColor;
        
        isBlinking = false;
    }
}
