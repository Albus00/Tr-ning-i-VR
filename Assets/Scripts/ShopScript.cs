//TRÄNING I VR
//Author: Victor Persson
//Date: 2023-04-21
//Last Updated: 2023-04-29 

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

    public GameObject axe;
    public GameObject katana;
    public GameObject mace;
    public GameObject mug;

    private Vector3 spawnPoint;

    void Start(){
        spawnPoint = GameObject.FindWithTag("weaponSpawn").transform.position;
    }

    public void updateShopSigns()
    {
        //TODO if we have time: Redo using arrays. "Find()" isn't performant at all and gets worse
        //                                         the more gameobjects exist in the scene
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
    //TODO (if time exists) - Refactor to common function. See above TODO.
    public void purchaseMug(){
        if(playerMoneyScript.currentMoney >= 100){
            audioSource.PlayOneShot(purchaseSuccessful, audioVolume);
            playerMoneyScript.removeMoney(100);
            Instantiate(mug, spawnPoint, Quaternion.identity);
        }else{
            audioSource.PlayOneShot(notEnoughMoney, audioVolume);
        }
    }
    public void purchaseAxe(){
        if(playerMoneyScript.currentMoney >= 2500){
            audioSource.PlayOneShot(purchaseSuccessful, audioVolume);
            playerMoneyScript.removeMoney(2500);
            Instantiate(axe, spawnPoint, Quaternion.identity);
        }else{
            audioSource.PlayOneShot(notEnoughMoney, audioVolume);
        }
    }
    public void purchaseMace(){
        if(playerMoneyScript.currentMoney >= 2750){
            audioSource.PlayOneShot(purchaseSuccessful, audioVolume);
            playerMoneyScript.removeMoney(2750);
            Instantiate(mace, spawnPoint, Quaternion.identity);
        }else{
            audioSource.PlayOneShot(notEnoughMoney, audioVolume);
        }
    }
    public void purchaseKatana(){
        if(playerMoneyScript.currentMoney >= 3990){
            audioSource.PlayOneShot(purchaseSuccessful, audioVolume);
            playerMoneyScript.removeMoney(3990);
            Instantiate(katana, spawnPoint, Quaternion.identity);
        }else{
            audioSource.PlayOneShot(notEnoughMoney, audioVolume);
        }
    }



}
