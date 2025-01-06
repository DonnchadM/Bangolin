using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToChange;
    private GameSystem system;
    public void ChangeScene()
    {
        GameObject systemObj = GameObject.Find("GameSystem");
        if(systemObj != null){
            system = systemObj.GetComponent<GameSystem>();
        }
        if (!string.IsNullOrEmpty(sceneToChange))
        {
            if(systemObj != null){
                system.newScene();
            }
            SceneManager.LoadScene(sceneToChange);
        }
        else
        {
            Debug.LogWarning("Scene name is not set.");
        }
    }
}
