using UnityEngine;

public class NextSceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.instance.LoadNextScene();
    }
}
