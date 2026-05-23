using UnityEngine;

public class FlashbackGuards : MonoBehaviour
{
    public bool HasBeenObserved { get; private set; } = false;
    
    [SerializeField] private GameObject[] observationPoints;
    
    public void MarkAsObserved()
    {
        if (HasBeenObserved) return;
        
        HasBeenObserved = true;

        foreach (GameObject point in observationPoints)
        {
            if (point != null)
            {
                point.SetActive(false);
            }
        }
        
        FlashbackGuardsManager.Instance.CheckAllZones();
    }
}