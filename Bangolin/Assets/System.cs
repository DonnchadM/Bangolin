using UnityEngine;

public class GameSystem : MonoBehaviour
{
    private int coins = 0;
    private int extraLives = 2;
    private static GameSystem instance;
    private int levelsBeat = 0;
    private SceneChanger sceneChanging;

    void Awake()
    {
        
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public int getCoins(){
        return coins;
    }
    public void addCoin(int coinsAdd){
        coins += coinsAdd;
    }
    public int getExtraLives(){
        return extraLives;
    }
    public void addExtraLives(int extraLivesAdd){
        extraLives += extraLivesAdd;
    }
    public void beatLevel(){
        levelsBeat++;
        sceneChanging.sceneToChange = "Shop";
        sceneChanging.ChangeScene();
    }
    // Start is called before the first frame update
    void Start()
    {
        sceneChanging = GetComponent<SceneChanger>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
