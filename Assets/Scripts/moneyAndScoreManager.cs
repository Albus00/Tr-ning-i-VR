//TRÄNING I VR
//Author: Victor Persson
//Date: 2023-04-21
//Author:
//Date: 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class moneyAndScoreManager : MonoBehaviour
{
    public ShopScript shop;
    public int currentMoney;
    public int prevMoney;

    public int currentScore;

    private TMPro.TMP_Text storeMoneyDisplay;
    // Start is called before the first frame update
    void Start()
    {
        currentMoney = 0; //Start Value for Money
        currentScore = 0; //Start Value for Score
        GameObject.Find("moneyDisplay").GetComponent<TextMeshProUGUI>().SetText("$"+currentMoney.ToString());
        shop.updateShopSigns();
    }

    // Update is called once per frame
    void Update()
    {
        // ------DEBUG-----
        if(Input.GetKeyDown("k")){
            //DEBUG: Simulate an enemy being defeated for $100
            giveMoney(100, false);
        }
        if(Input.GetKeyDown("b")){
            //DEBUG: Simulate an object being purchased for $450
            shop.purchaseObject();
        }
        if(Input.GetKeyDown("h")){
            //DEBUG: Set the money avaliable to exactly 450
            currentMoney = 450;
        }
        // -----End of debug stuff-----

        if(currentMoney <= 0){
            currentMoney = 0;
        }

        //lazy update shop dis
        if(prevMoney != currentMoney){
            GameObject.Find("moneyDisplay").GetComponent<TextMeshProUGUI>().SetText("$"+currentMoney.ToString());

            shop.updateShopSigns();
        }

        prevMoney = currentMoney;    
    }

    //Called when defeating enemies
    public void giveMoney(int giveAmount, bool doublePoints)
    {
        if(doublePoints){
            currentMoney += giveAmount*2;
        }else{
            currentMoney += giveAmount;
        }
    }

    //Called during purchases
    public void removeMoney(int cost)
    {
        currentMoney -= cost;
    }

    //Called when defeating enemies and moving a lot
    public void giveScore(int score){
        currentScore += score;
    }
}
