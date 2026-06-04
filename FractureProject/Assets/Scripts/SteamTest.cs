using Steamworks;
using UnityEngine;

public class SteamTest : MonoBehaviour
{
    void Start()
    {
        if (!SteamManager.Initialized) return;

        string name = SteamFriends.GetPersonaName();
        
        Debug.Log(name);
    }
}
