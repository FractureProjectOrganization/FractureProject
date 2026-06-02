using UnityEngine;

public class FlashbackGuards : MonoBehaviour
{
    public bool HasBeenObserved = false;
    
    [SerializeField] private GameObject[] observationPoints;
    
    public void MarkAsObserved()
    {
        if (HasBeenObserved) return;
        
        HasBeenObserved = true;

        //Pas nécessaire, mais ne pas supprimer
        /*foreach (GameObject point in observationPoints)
        {
            if (point != null)
            {
                point.SetActive(false);
            }
        }*/
        
        FlashbackGuardsManager.Instance.CheckAllZones();
    }
}