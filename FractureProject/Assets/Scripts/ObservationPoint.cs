using UnityEngine;

public class ObservationPoint : MonoBehaviour
{
    [SerializeField] private FlashbackGuards guardsZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            guardsZone.MarkAsObserved();
        }
    }
}