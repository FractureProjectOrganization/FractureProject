using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessTransition : MonoBehaviour
{
    public Light[] bforeLights, afterLights;
    public Volume bforeVolume, afterVolume;
    
    private ColorLookup colorLookup;

    private List<float> bforeLightsSavedWeight;
    private List<float> afterLightsSavedWeight;
    private float bforeVolumeWeight, afterVolumeWeight;
    public Transform start, end, debugObj;
    
    [Tooltip("Essayez de faire en sorte que ca match le temps de dezoom")]
    public float transitionDuration = 1f;
    
    private float transitionTime = 0f;
    private Vector2 vec0Cached;
    
    private bool isFadingOut = false;
    
    private Player player;

    void Start()
    {
        player = Player.instance;
        bforeLightsSavedWeight = new List<float>();
        afterLightsSavedWeight = new List<float>();
        foreach(Light l in bforeLights) bforeLightsSavedWeight.Add(l.intensity);
        foreach (Light l in afterLights)
        {
            afterLightsSavedWeight.Add(l.intensity);
            l.intensity = 0;
        }

        bforeVolumeWeight = bforeVolume.weight;
        afterVolumeWeight = afterVolume.weight;

        afterVolume.weight = 0;
        
        vec0Cached = new Vector2(end.position.x - start.position.x, end.position.z - start.position.z);
    }

    void Update()
    {
        Vector2 vec0 = new Vector2(end.position.x - start.position.x, end.position.z - start.position.z);
        Vector2 vec = vec0.normalized;
        float bh = ((player.transform.position.x-start.position.x)*vec.x + (player.transform.position.z-start.position.z)*vec.y)/Mathf.Sqrt(vec.x*vec.x + vec.y*vec.y);

        for (int i =0;i< bforeLights.Length; i++)
        {
            Light l = bforeLights[i];
            l.intensity = Mathf.Lerp(0,bforeLightsSavedWeight[i], 1-bh*2/vec0.magnitude);
        }
        bforeVolume.weight = Mathf.Lerp(0,bforeVolumeWeight, 1-bh/vec0.magnitude);
        
        for (int i =0;i< afterLights.Length; i++)
        {
            Light l = afterLights[i];
            l.intensity = Mathf.Lerp(0, afterLightsSavedWeight[i], bh*2/vec0.magnitude);
        }

        afterVolume.weight = Mathf.Lerp(0, afterVolumeWeight, bh / vec0.magnitude);
        afterVolume.profile.TryGet<ColorLookup>(out colorLookup);

        ClampedFloatParameter parameter =
            new ClampedFloatParameter(Mathf.Lerp(0, afterVolumeWeight, bh / vec0.magnitude), 0, 1, false);
        if (colorLookup) colorLookup.contribution = parameter;
        
        if (isFadingOut) FadeOutPostProcess();
    }

    private void FadeOutPostProcess()
    {
        transitionTime += Time.deltaTime;
        float bh = Mathf.Lerp(vec0Cached.magnitude, 0f, transitionTime / transitionDuration);

        for (int i = 0; i < bforeLights.Length; i++)
        {
            Light l = bforeLights[i];
            l.intensity = Mathf.Lerp(0, bforeLightsSavedWeight[i], 1 - bh * 2 / vec0Cached.magnitude);
        }
        bforeVolume.weight = Mathf.Lerp(0, bforeVolumeWeight, 1 - bh / vec0Cached.magnitude);

        for (int i = 0; i < afterLights.Length; i++)
        {
            Light l = afterLights[i];
            l.intensity = Mathf.Lerp(0, afterLightsSavedWeight[i], bh * 2 / vec0Cached.magnitude);
        }

        afterVolume.weight = Mathf.Lerp(0, afterVolumeWeight, bh / vec0Cached.magnitude);
        afterVolume.profile.TryGet<ColorLookup>(out colorLookup);

        ClampedFloatParameter parameter =
            new ClampedFloatParameter(Mathf.Lerp(0, afterVolumeWeight, bh / vec0Cached.magnitude), 0, 1, false);
        if (colorLookup) colorLookup.contribution = parameter;
    }
    
    public void StartFadeOut()
    {
        transitionTime = 0f;
        isFadingOut = true;
    }
}