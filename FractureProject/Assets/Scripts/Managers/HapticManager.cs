using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HapticManager : MonoBehaviour
{
    #region Initialize
    
    public static HapticManager instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    static void Init()
    {
        GameObject obj = new GameObject("Haptic Manager");
        instance = obj.AddComponent<HapticManager>();
        DontDestroyOnLoad(obj);
    }
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    
    #endregion
    
    [Header("Interaction Vibration Settings")]
    [Range(0f, 1f), Tooltip("Vibration lourde")]
    public float interactionLowFrequency = 0.1f;
    [Range(0f, 1f), Tooltip("Vibration légere")]
    public float interactionHighFrequency = 0.6f;
    public float interactionRumbleDuration = 0.08f;
    
    public IEnumerator InteractionFeedback()
    {
        Gamepad gamepad = Gamepad.current;

        if (gamepad == null) yield break;
        
        gamepad.SetMotorSpeeds(interactionLowFrequency, interactionHighFrequency);
        yield return new WaitForSeconds(interactionRumbleDuration);
        gamepad.PauseHaptics();
    }
    
    [Space]
    
    [Header("Push Vibration Settings")]
    [Range(0f, 1f), Tooltip("Vibration lourde")]
    public float pushLowFrequency = 0.6f;
    [Range(0f, 1f), Tooltip("Vibration légere")]
    public float pushHighFrequency = 0.35f;
    
    public IEnumerator Push()
    {
        Gamepad gamepad = Gamepad.current;

        if (gamepad == null) yield break;
        
        gamepad.SetMotorSpeeds(pushLowFrequency, pushHighFrequency);
    }
}
