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
    
    public void OnClick()
    {
        if (disabled) return;
        active = !active;
        text.text = active? onString : the_offSpring;
        if(active) onOn.Invoke(); else onOff.Invoke();
    }
}
