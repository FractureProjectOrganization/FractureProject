using UnityEngine;

public class FlashbackGuards : MonoBehaviour
{
    public bool HasBeenObserved = false;
    [SerializeField] private GameObject[] observationPoints;
    
    public void MarkAsObserved()
    {
        if (HasBeenObserved) return;
        HasBeenObserved = true;
        Debug.Log(gameObject.name + " has been observed.");
        
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
