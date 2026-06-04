using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SecretButton : MonoBehaviour
{
    public TextMeshProUGUI text;
    private bool active = false;
    public UnityEvent onOn, onOff;
    public string onString, the_offSpring;
    public bool disabled;
    public GameObject onGO, offGO;
    
    public void OnClick()
    {
        if (disabled) return;
        active = !active;
        if(text)text.text = active? onString : the_offSpring;
        if(onGO)onGO.SetActive(active);
        if(offGO)offGO.SetActive(!active);
        if(active) onOn.Invoke(); else onOff.Invoke();
    }
}
