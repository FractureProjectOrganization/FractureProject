using System;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.instance.LoadMainMenu();
        }
    }

    public void CreditsEnd()
    {
        TransitionManager.instance.FadeToBlack();
    }
}
