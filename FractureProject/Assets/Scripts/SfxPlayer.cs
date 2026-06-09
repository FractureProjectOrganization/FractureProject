using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    public string clip;
    public void PlaySfx()
    {
        SoundManager.PlaySound(clip);
    }
}
