using System.Collections;
using UnityEngine;
using TMPro;

public class BonusStage : MonoBehaviour
{
    public int bonusTime = 10;
    public TMP_Text bonusTimer;
    private GameObject gameSystem;
    private GameSystem system;
    private SceneChanger sceneChanger;
    private void Start()
    {
        if (bonusTimer != null)
        {
            bonusTimer.text = bonusTime.ToString();
        }
        StartCoroutine(BonusCountdown());
        gameSystem = GameObject.Find("GameSystem");
        system = gameSystem.GetComponent<GameSystem>();
        sceneChanger = gameSystem.GetComponent<SceneChanger>();
    }

    private IEnumerator BonusCountdown()
    {
        while (bonusTime > 0)
        {
            yield return new WaitForSeconds(1f);
            bonusTime--; 
            if (bonusTimer != null)
            {
                bonusTimer.text = bonusTime.ToString(); 
            }
        }

        bonusEnd(); 
    }

    private void bonusEnd()
    {
        Debug.Log("Bonus stage ended!");
        sceneChanger.sceneToChange = "Shop";
        sceneChanger.ChangeScene();
    }
}
