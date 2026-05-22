using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlashbackGuardsManager : MonoBehaviour
{
    [System.Serializable]

    public class FlashbackGuard
    {
        public List<GameObject> Guards;
        public List<GameObject> ObservationSpots;
    }
}
