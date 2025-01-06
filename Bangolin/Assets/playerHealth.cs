using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    private int hits = 1;
    public int maximumHits = 2;

    public int ghostTime = 100;
    private bool invincible = false;
    // Start is called before the first frame update
    void Start()
    {
        hits = 1;
    }
    public void addHit(int newHits){
        if (hits + newHits <= maximumHits){
            hits += newHits;
        }else{
            hits = maximumHits;
        }
    }
    public void takeHit(int dam){
        if(!invincible){
            invincible = true;
            StartCoroutine(StartGhostTime());
            Debug.Log(invincible);
            hits -= dam;
            Debug.Log(hits);

        }
    }
    IEnumerator StartGhostTime(){
        yield return new WaitForSeconds(ghostTime);
        invincible = false;
        Debug.Log(invincible);
    }
    // Update is called once per frame
    void Update()
    {
        if (hits <= 0){
            Die();
        }
    }
    void Die(){
        Destroy(this.gameObject);
    }
}
