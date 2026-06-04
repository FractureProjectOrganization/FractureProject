using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderAdditional : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Slider slider;
    public Sprite def, highlight;
    public Image TargetGraphic;
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void OnSelect (BaseEventData eventData) 
    {
        TargetGraphic.sprite = highlight;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        TargetGraphic.sprite = def;
    }
}
