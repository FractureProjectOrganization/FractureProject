using UnityEngine;

public class QuitCode : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Quitting the game right now mais nan c'est fou");
        Application.Quit();
    }
}
