using UnityEngine;

public class FlashEvent : MonoBehaviour
{
    public void Flash()
    {
        SoundManager.PlaySound("Flash");
    }
}
