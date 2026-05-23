using UnityEngine;

public class FlashbackGuardsManager : MonoBehaviour
{
    public static FlashbackGuardsManager Instance;

    [SerializeField] private FlashbackGuards[] guardsZones;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CheckAllZones()
    {
        foreach (FlashbackGuards zone in guardsZones)
        {
            if (!zone.HasBeenObserved) return;
        }
        
        TransitionManager.instance.FadeToBlack();
    }
}