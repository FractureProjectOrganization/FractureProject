using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettingSlider : MonoBehaviour
{
    private Slider slider;
    public AudioMixerGroup mixerGroup;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void UpdateVolume()
    {
        float percent = 1-((slider.value) / slider.maxValue);
        float exp = Mathf.Exp(2.13f*percent-2)-0.135f;
        mixerGroup.audioMixer.SetFloat(mixerGroup.name,exp *(-80f));
    }
}
