using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundCounter : MonoBehaviour
{
    public TextMeshProUGUI roundText;
    int rCount;

    // Start is called before the first frame update
    void Start()
    {
        rCount = 1;
        roundText.text = rCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        roundText.text = rCount.ToString();
    }

    public void AddRound()
    {
        rCount++;
    }
    public int ReturnCount()
    {
        return rCount;
    }
}
