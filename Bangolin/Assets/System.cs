using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSystem : MonoBehaviour
{
    private int coins = 0;
    private int extraLives = 2;
    private static GameSystem instance;
    private int levelsBeat = 0;
    private SceneChanger sceneChanging;
    private GameObject canvas;
    // Dictionary to store power-ups and their quantities
    private Dictionary<string, int> powerUpInventory = new Dictionary<string, int>();
    public GameObject coinText;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        canvas = GameObject.Find("Canvas");
        coinText = GameObject.Find("coins");
    }

    public int getCoins()
    {
        return coins;
        coinText.GetComponent<TMP_Text>().text = coins.ToString();

    }

    public void addCoin(int coinsAdd)
    {
        coins += coinsAdd;
        coinText.GetComponent<TMP_Text>().text = coins.ToString();
    }

    public void takeCoins(int coinsTake){
        coins -= coinsTake;
        coinText = GameObject.Find("coins");
        coinText.GetComponent<TMP_Text>().text = coins.ToString();
    }

    public int getExtraLives()
    {
        return extraLives;
    }

    public void addExtraLives(int extraLivesAdd)
    {
        extraLives += extraLivesAdd;
    }

    public void beatLevel()
    {
        levelsBeat++;
        sceneChanging.sceneToChange = "Shop";
        sceneChanging.ChangeScene();
    }

    public void addPowerUp(string powerUpName, int quantity = 1)
    {
        if (powerUpInventory.ContainsKey(powerUpName))
        {
            powerUpInventory[powerUpName] += quantity;
        }
        else
        {
            powerUpInventory[powerUpName] = quantity;
        }
    }

    public int getPowerUpQuantity(string powerUpName)
    {
        if (powerUpInventory.ContainsKey(powerUpName))
        {
            return powerUpInventory[powerUpName];
        }
        return 0; // Return 0 if the power-up is not in the inventory
    }

    public bool usePowerUp(string powerUpName)
    {
        if (powerUpInventory.ContainsKey(powerUpName) && powerUpInventory[powerUpName] > 0)
        {
            powerUpInventory[powerUpName]--;
            if (powerUpInventory[powerUpName] == 0)
            {
                powerUpInventory.Remove(powerUpName);
            }
            return true; // Indicate that the power-up was successfully used
        }
        return false; // Indicate that the power-up is not available or has no quantity left
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneChanging = GetComponent<SceneChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnityEditor.EditorWindow.focusedWindow.maximized = !UnityEditor.EditorWindow.focusedWindow.maximized;
            }   
        #endif
        //coinText.GetComponent<TMP_Text>().text = coins.ToString();
    }
}
