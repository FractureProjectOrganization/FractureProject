using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private InputSystem_Actions _actions; // generated C# class

    // Expose actions cleanly
    public InputAction Interact => _actions.Player.Interact;
    public InputAction Album => _actions.Player.Album;
    public InputAction UIConfirm => _actions.UI.Submit;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _actions = new InputSystem_Actions();
    }

    void OnEnable() => _actions.Enable();
    void OnDisable() => _actions.Disable();
}
