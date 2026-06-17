using Steamworks;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    static void Init()
    {
        GameObject obj = new GameObject("AchievementManager");
        instance = obj.AddComponent<AchievementManager>();
        DontDestroyOnLoad(obj);
    }
    
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
    
    public void TriggerMyAchievement(string achievementID)
    {
        Debug.Log("HAHAHAHA");
        
        if (!SteamManager.Initialized) return;
        
        SteamUserStats.SetAchievement(achievementID);
        SteamUserStats.StoreStats();
        
        // Debug.Log ("Achievement: " + SteamUserStats.GetAchievement(achievementID, out ));
        // Debug.Log();
    }

}
