using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopButton : MonoBehaviour
{
    private GameObject gameSystem;
    private GameSystem system;
    
    public string powerUpName;
    public int quantity;
    public int price;
    // Start is called before the first frame update
    void Start()
    {
        gameSystem = GameObject.Find("GameSystem");
        system = gameSystem.GetComponent<GameSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void buy(){
        if (system.getCoins() >= price){
            system.addPowerUp(powerUpName,quantity);
            system.takeCoins(price);
        }
    }

}
