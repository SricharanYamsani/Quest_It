using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIHighlight : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler
{
    [SerializeField] Color defColor;
    [SerializeField] Color tweenColor;
    [SerializeField] float duration;
    [SerializeField] Image image;

    Tween colorTween;

    //----------------------------------------------------
    public void OnPointerEnter(PointerEventData eventData)
    {
        colorTween = image.DOColor(tweenColor, duration);
    }

    public void OnSelect(BaseEventData eventData)
    {
        //do your stuff when selected
    }
    
    //---------------------------------------------------
    public void OnPointerExit(PointerEventData eventData)
    {
        if (colorTween != null)
        {
            colorTween.Kill();
        }
        colorTween = image.DOColor(defColor, duration);
    }

    //----------------------
    private void OnDisable()
    {
        if (colorTween != null)
        {
            colorTween.Kill();
        }
        image.color = defColor;
    }
}