using Steamworks;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance { get; private set; }
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        if (!SteamManager.Initialized) return;

        SteamUserStats.ResetAllStats(true);
    }
    
    public void TriggerAchievement(string achievementID)
    {
        if (!SteamManager.Initialized) return;
        
        SteamUserStats.SetAchievement(achievementID);
        SteamUserStats.StoreStats();
    }

}
