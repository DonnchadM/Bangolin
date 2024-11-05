using UnityEngine;

public class GameSystem : MonoBehaviour
{
    private int coins = 0;
    private int extraLives = 2;
    private static GameSystem instance;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
