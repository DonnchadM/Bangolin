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
    public Dictionary<string, int> powerUpInventory = new Dictionary<string, int>();
    private List<string> powerUpOrder = new List<string>();
    public GameObject coinText;
    private TMP_Text zPowerText;
    private TMP_Text xPowerText;
    private TMP_Text cPowerText;

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
        zPowerText = GameObject.Find("zPower")?.GetComponent<TMP_Text>();
        xPowerText = GameObject.Find("xPower")?.GetComponent<TMP_Text>();
        cPowerText = GameObject.Find("cPower")?.GetComponent<TMP_Text>();
        
        if (coinText != null)
        {
            coinText.GetComponent<TMP_Text>().text = coins.ToString();
        }

        takeCoins(0);
    }

    public int getCoins()
    {
        return coins;
    }

    public void newScene()
    {
        Debug.Log("Jacket");
        canvas = GameObject.Find("Canvas");
        coinText = GameObject.Find("coins");
        zPowerText = GameObject.Find("zPower")?.GetComponent<TMP_Text>();
        xPowerText = GameObject.Find("xPower")?.GetComponent<TMP_Text>();
        cPowerText = GameObject.Find("cPower")?.GetComponent<TMP_Text>();
        if (coinText != null)
        {
            coinText.GetComponent<TMP_Text>().text = coins.ToString();
            takeCoins(0);
        }
        else
        {
            Debug.LogWarning("Coin text object not found in the new scene.");
        }
    }

    public string getPowerPos(int pos)
    {
        if (pos < 0 || pos >= powerUpInventory.Count)
        {
            Debug.LogWarning("Position out of range in powerUpInventory.");
            return null;
        }

        int currentIndex = 0;
        foreach (var key in powerUpInventory.Keys)
        {
            if (currentIndex == pos)
            {
                return key;
            }
            currentIndex++;
        }

        return null;
    }

    public void addCoin(int coinsAdd)
    {
        coins += coinsAdd;
        if (coinText != null)
        {
            coinText.GetComponent<TMP_Text>().text = coins.ToString();
        }
    }

    public void takeCoins(int coinsTake)
    {
        coins -= coinsTake;
        coinText = GameObject.Find("coins");
        if (coinText != null)
        {
            coinText.GetComponent<TMP_Text>().text = coins.ToString();
        }
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
        if (sceneChanging != null)
        {
            sceneChanging.sceneToChange = "Shop";
            sceneChanging.ChangeScene();
        }
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
            powerUpOrder.Add(powerUpName);
        }

    }

    public int getPowerUpQuantity(string powerUpName)
    {
        if (string.IsNullOrEmpty(powerUpName))
        {
            Debug.LogWarning("Power-up name is null or empty.");
            return 0;
        }

        if (powerUpInventory.ContainsKey(powerUpName))
        {
            return powerUpInventory[powerUpName];
        }

        return 0;
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
            return true;
        }
        return false;
    }

    void Start()
    {
        coinText = GameObject.Find("coins");
        sceneChanging = GetComponent<SceneChanger>();
        if (coinText != null)
        {
            coinText.GetComponent<TMP_Text>().text = coins.ToString();
        }
        takeCoins(0);
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
        coinText = GameObject.Find("coins");
        coinText.GetComponent<TMP_Text>().text = coins.ToString();
        zPowerText = GameObject.Find("zPower")?.GetComponent<TMP_Text>();
        zPowerText.text = (getPowerPos(0) + " : " + getPowerUpQuantity(getPowerPos(0)));
        xPowerText = GameObject.Find("xPower")?.GetComponent<TMP_Text>();
        xPowerText.text = (getPowerPos(1) + " : " + getPowerUpQuantity(getPowerPos(1)));
        cPowerText = GameObject.Find("cPower")?.GetComponent<TMP_Text>();
        cPowerText.text = (getPowerPos(2) + " : " + getPowerUpQuantity(getPowerPos(2)));
    }
}
