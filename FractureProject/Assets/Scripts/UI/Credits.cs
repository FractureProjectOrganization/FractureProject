using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    private void Update()
    {
        if (InputSystem.devices.Any(d => d.wasUpdatedThisFrame && d.allControls.Any(c => c is ButtonControl btn && 
                btn.wasPressedThisFrame)))
        {
            TransitionManager.instance.FadeToBlack();
        }
    }

    public void CreditsEnd()
    {
        TransitionManager.instance.FadeToBlack();
    }
}
