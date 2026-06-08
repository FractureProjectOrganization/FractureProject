using UnityEngine;

public class Credits : MonoBehaviour
{
    private void Update()
    {
        if (NewInput.GetSkipDown())
        {
            TransitionManager.instance.FadeToBlack();
        }
    }

    public void CreditsEnd()
    {
        TransitionManager.instance.FadeToBlack();
    }
}