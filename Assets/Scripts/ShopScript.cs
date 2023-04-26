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

public class ShopScript : MonoBehaviour
{
    public moneyAndScoreManager playerMoneyScript;

    public AudioSource audioSource;
    public AudioClip purchaseSuccessful;
    public AudioClip notEnoughMoney;
    public float audioVolume = 0.5f;

    private Color buyableColor = Color.yellow;
    private Color tooExpensiveColor = Color.red;
    private Color dynamicBuyable = Color.white;

    public Image staticBorder;
    public Image dynamicBorder;
    private int price;

    public void updateShopSigns()
    {
        //TODO: Redo using arrays. "Find()" isn't a very good way of going about this. But it works for now.
        foreach(Transform child in GameObject.Find("Products").transform){
            int.TryParse(child.Find("price").GetComponent<TextMeshProUGUI>().text, out price);
            dynamicBorder = child.Find("dynBorder").GetComponent<Image>();

            float adequateMoneyRatio = (float)playerMoneyScript.currentMoney/(float)price;

            if(playerMoneyScript.currentMoney >= price){
                staticBorder = child.GetComponent<Image>();
                staticBorder.color = buyableColor;
                dynamicBorder.fillAmount = 1.0f;
                dynamicBorder.color = Color.green;
            }else{
                staticBorder = child.GetComponent<Image>(); 
                staticBorder.color = tooExpensiveColor;
                dynamicBorder.fillAmount = adequateMoneyRatio;
                dynamicBorder.color = dynamicBuyable;
            }
        }
    }

    public void purchaseObject(){
        //TODO: 
        // 1. Find name of object grabbed 
        //      bla bla (GameObject.Find("Products").transform) bla bla
        // 2. Find price of object grabbed 
        //      int.TryParse(child.Find("price".GetComponent<TextMeshProUGUI>().text, out price))
        // 3. Give the player / spawn the object purchased if the funds are avaliable
        if(playerMoneyScript.currentMoney >= 450){
            Debug.Log("Purchase successful!");
            audioSource.PlayOneShot(purchaseSuccessful, audioVolume);
            playerMoneyScript.removeMoney(450);
            //TODO: [pseudocode]: player.giveWeapon(nameOfWeapon);
        }else{
            Debug.Log("Not enough money!");
            audioSource.PlayOneShot(notEnoughMoney, audioVolume);
        }
    }
}
