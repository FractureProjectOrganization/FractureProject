using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Steamworks;

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
    
    #region Steam Input Convertion
    
    //TODO: ON PAUSE UNTIL AFTER GOLD
    
    // private ushort ToSteamSpeed(float value) => (ushort)(Mathf.Clamp01(value) * ushort.MaxValue);
    //
    // private void TriggerVibration(InputHandle_t inputHandle, ushort usLeftSpeed, ushort usRightSpeed)
    // {
    //     SteamInput.TriggerVibration(inputHandle, usLeftSpeed, usRightSpeed);
    // }
    //
    // private void TriggerVibrationAllControllers(ushort usLeftSpeed, ushort usRightSpeed)
    // {
    //     InputHandle_t[] handles = new InputHandle_t[Constants.STEAM_INPUT_MAX_COUNT];
    //     int count = SteamInput.GetConnectedControllers(handles);
    //
    //     for (int i = 0; i < count; i++)
    //         TriggerVibration(handles[i], usLeftSpeed, usRightSpeed);
    // }
    
    #endregion
    
    #region Interaction
    
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
        // TriggerVibrationAllControllers(ToSteamSpeed(interactionLowFrequency), ToSteamSpeed(interactionHighFrequency));
        
        yield return new WaitForSeconds(interactionRumbleDuration);
        
        gamepad.PauseHaptics();
        // TriggerVibrationAllControllers(0, 0);
    }
    
    #endregion
    
    #region Push
    
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
        // TriggerVibrationAllControllers(ToSteamSpeed(pushLowFrequency), ToSteamSpeed(pushHighFrequency));
    }
    #endregion
}
