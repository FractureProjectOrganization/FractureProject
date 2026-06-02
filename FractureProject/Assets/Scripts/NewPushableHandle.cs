using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPushableHandle : MonoBehaviour
{
    private NewPushableObject obj;
    
    [SerializeField] private GameObject outlineTrigger;

    private OutlineGradient outlineGradient;

    [Header("Controller Vibration Settings")]
    [Range(0f, 1f), Tooltip("Vibration lourde")]
    public float lowFrequency;
    [Range(0f, 1f), Tooltip("Vibration légere")]
    public float highFrequency;
    public float rumbleDuration;

    private void Start()
    {
        obj = GetComponentInParent<NewPushableObject>();
        outlineGradient = outlineTrigger.GetComponent<OutlineGradient>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            obj.isPlayerNear = true;
            StartCoroutine(Rumble());
            outlineGradient.FillOutline(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            obj.isPlayerNear = false;
            outlineGradient.FillOutline(false);
        }
    }

    private IEnumerator Rumble()
    {
        Gamepad gamepad = Gamepad.current;

        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
            yield return new WaitForSeconds(rumbleDuration);
            gamepad.PauseHaptics();
        }
    }
}
