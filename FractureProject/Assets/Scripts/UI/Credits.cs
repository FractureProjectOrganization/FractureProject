using System;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKey)
        {
            TransitionManager.instance.FadeToBlack();
        }
    }

    public void CreditsEnd()
    {
        TransitionManager.instance.FadeToBlack();
    }
}
