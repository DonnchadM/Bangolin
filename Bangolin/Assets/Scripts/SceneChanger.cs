using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToChange;

    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneToChange))
        {
            SceneManager.LoadScene(sceneToChange);
        }
        else
        {
            Debug.LogWarning("Scene name is not set.");
        }
    }
}
