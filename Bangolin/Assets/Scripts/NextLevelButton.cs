using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    private GameObject gameSystem;
    private GameSystem system;
    private SceneChanger sceneChanger;

    // List of level names
    public List<string> levelNames;

    // Start is called before the first frame update
    void Start()
    {
        gameSystem = GameObject.Find("GameSystem");
        system = gameSystem.GetComponent<GameSystem>();
        sceneChanger = gameSystem.GetComponent<SceneChanger>();

        // Example: Initialize the list of level names (or set this in the Inspector)
        levelNames = new List<string> { "SampleScene", "nextTestLevel","bigGap","climb","descend",};
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void nextLevel()
    {
        if (levelNames.Count > 0)
        {
            // Select a random level from the list
            int randomIndex = Random.Range(0, levelNames.Count);
            string randomLevel = levelNames[randomIndex];

            // Change to the selected random level
            sceneChanger.sceneToChange = randomLevel;
            sceneChanger.ChangeScene();
        }
        else
        {
            Debug.LogWarning("No levels in the list to choose from!");
        }
    }
}
