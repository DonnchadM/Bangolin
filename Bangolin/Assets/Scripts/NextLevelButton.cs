using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    private GameObject gameSystem;
    private GameSystem system;
    private SceneChanger sceneChanger;
    // Start is called before the first frame update
    void Start()
    {
        gameSystem = GameObject.Find("GameSystem");
        system = gameSystem.GetComponent<GameSystem>();
        sceneChanger = gameSystem.GetComponent<SceneChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextLevel(){
        sceneChanger.sceneToChange = "nextTestLevel";
        sceneChanger.ChangeScene();
    }
}
