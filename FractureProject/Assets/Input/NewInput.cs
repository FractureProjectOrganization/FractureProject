using UnityEngine;

public static class NewInput
{
    private static PlayerControls _controls;
    private static PlayerControls Controls
    {
        get
        {
            if (_controls == null)
            {
                _controls = new PlayerControls();
                _controls.Enable();
            }
            return _controls;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetState()
    {
        if (_controls != null)
        {
            _controls.Disable();
            _controls = null;
        }
    }

    public static Vector2 GetMovement() => Controls.Player.Move.ReadValue<Vector2>();
    public static float GetAxisHorizontal() => Controls.Player.Move.ReadValue<Vector2>().x;
    public static float GetAxisVertical() => Controls.Player.Move.ReadValue<Vector2>().y;

    public static bool GetInteractDown() => Controls.Player.Interact.WasPressedThisFrame();
    public static bool GetInteract() => Controls.Player.Interact.IsPressed();
    public static bool GetInteractUp() => Controls.Player.Interact.WasReleasedThisFrame();

    public static bool GetAlbumDown() => Controls.Player.Album.WasPressedThisFrame();
    public static bool GetAlbum() => Controls.Player.Album.IsPressed();
    public static bool GetAlbumUp() => Controls.Player.Album.WasReleasedThisFrame();

    public static bool GetPauseDown() => Controls.Player.Pause.WasPressedThisFrame();
    public static bool GetPause() => Controls.Player.Pause.IsPressed();
    public static bool GetPauseUp() => Controls.Player.Pause.WasReleasedThisFrame();

    public static bool GetBackDown() => Controls.Player.Back.WasPressedThisFrame();
    public static bool GetBack() => Controls.Player.Back.IsPressed();
    public static bool GetBackUp() => Controls.Player.Back.WasReleasedThisFrame();

    public static bool GetSkipDown() => Controls.Player.Skip.WasPressedThisFrame();
    public static bool GetSkip() => Controls.Player.Skip.IsPressed();
    public static bool GetSkipUp() => Controls.Player.Skip.WasReleasedThisFrame();

}
