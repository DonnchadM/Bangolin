using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    private GameSystem gameSystem;
    private SceneChanger sceneChanger;

    public List<string> levelNames;
    public List<string> bonusLevelNames;

    void Start()
    {
        gameSystem = FindObjectOfType<GameSystem>();
        sceneChanger = gameSystem.GetComponent<SceneChanger>();

        levelNames = new List<string> { "SampleScene", "nextTestLevel", "bigGap", "climb", "descend" };
        bonusLevelNames = new List<string> { "Bonus1" };
    }

    public void NextLevel()
    {
        Debug.Log("crap");
        gameSystem.IncrementNextLevelButtonPressCount();

        int pressCount = gameSystem.GetNextLevelButtonPressCount();

        if (pressCount % 3 == 0) 
        {
            if (bonusLevelNames.Count > 0)
            {
                int randomIndex = Random.Range(0, bonusLevelNames.Count);
                string randomBonusLevel = bonusLevelNames[randomIndex];

                sceneChanger.sceneToChange = randomBonusLevel;
                sceneChanger.ChangeScene();
            }
            else
            {
                Debug.LogWarning("No bonus levels in the list to choose from!");
            }
        }
        else 
        {
            if (levelNames.Count > 0)
            {
                int randomIndex = Random.Range(0, levelNames.Count);
                string randomLevel = levelNames[randomIndex];

                sceneChanger.sceneToChange = randomLevel;
                sceneChanger.ChangeScene();
            }
            else
            {
                Debug.LogWarning("No levels in the list to choose from!");
            }
        }
    }
}
