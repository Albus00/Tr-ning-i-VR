using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;

public class WindowGraph : MonoBehaviour
{
    private playerStatTracker playerStats;
    private RectTransform graphContainer;
    [SerializeField] private Sprite circleSprite;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private moneyAndScoreManager FinalScore;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindWithTag("playerStats").GetComponent<playerStatTracker>();

        playerStats.isntDead = false; //Stop logging immediately

        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();

        FinalScore = GameObject.Find("ScoreManager").GetComponent<moneyAndScoreManager>();

        GameObject.Find("finalScore").GetComponent<TextMeshProUGUI>().text = FinalScore.currentScore.ToString();

        ShowLabels(playerStats.currentHealth);
        ShowGraph(playerStats.isCooldown);
        ShowGraph(playerStats.currentHealth, Color.red);
        ShowGraph(playerStats.currentMoney, Color.green);
        ShowGraph(playerStats.enemiesDefeated, Color.blue);
        ShowGraph(playerStats.currentScore, Color.yellow);
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, Color circleColor){
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = circleColor;
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11,11);
        rectTransform.anchorMin = new Vector2(0,0);
        rectTransform.anchorMax = new Vector2(0,0);
        return gameObject;
    }

    private GameObject CreateRectangle(int startIndex,int endIndex){
        float graphHeight = graphContainer.sizeDelta.y;
        float XtimeScale = 25f;

        GameObject gameObject = new GameObject("coolDownRectangle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        float xPositionStart = XtimeScale/2.0f + startIndex*XtimeScale;
        float xPositionEnd = XtimeScale/2.0f + endIndex*XtimeScale;

        Color transparentRed = new Color(1f,0f,0f,0.015f);

        gameObject.GetComponent<Image>().color = transparentRed;
        rectTransform.anchoredPosition = new Vector2(xPositionStart, graphHeight/2.0f);
        rectTransform.sizeDelta = new Vector2(XtimeScale,graphHeight);
        rectTransform.anchorMin = new Vector2(0,0);
        rectTransform.anchorMax = new Vector2(0,0);
        return gameObject;
    }

    private void ShowLabels(float[] valueList){
        float graphHeight = graphContainer.sizeDelta.y;
        float XtimeScale = 25f;
        
        for(int i = 0; i < valueList.Length; ++i){
            float xPosition = XtimeScale/2.0f + i * XtimeScale;
            
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition,-20f);
            labelX.GetComponent<TextMeshProUGUI>().text = (i).ToString();
        }
    }

    //------For floats:-----//
    private void ShowGraph(float[] valueList, Color graphColor){
        float graphHeight = graphContainer.sizeDelta.y;
        float XtimeScale = 25f;
        float yMaximum = Mathf.Max(valueList);

        GameObject lastCircle = null;
        for(int i = 0; i < valueList.Length; ++i){
            float xPosition = XtimeScale/2.0f + i * XtimeScale;
            float yPosition = (Mathf.Round(valueList[i]) / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), graphColor);
            
            if(lastCircle != null){
                CreateDotConnection(lastCircle.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, graphColor);
            }
            lastCircle = circleGameObject;
        }
    }

    //------For integers:-----//
    private void ShowGraph(int[] valueList, Color graphColor){
        float graphHeight = graphContainer.sizeDelta.y;
        float XtimeScale = 25f;
        float yMaximum = Mathf.Max(valueList);

        GameObject lastCircle = null;
        for(int i = 0; i < valueList.Length; ++i){
            float xPosition = XtimeScale/2.0f + i * XtimeScale;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), graphColor);
            
            if(lastCircle != null){
                CreateDotConnection(lastCircle.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, graphColor);
            }
            lastCircle = circleGameObject;
        }
    }

    //-----For booleans------//
    private void ShowGraph(bool[] valueList){
        int startIndex = 0;
        int endIndex = 0;
        bool lastBool = true;
         for(int i = 0; i < valueList.Length; ++i){
            if(valueList[i] == lastBool){
                lastBool = valueList[i];
            
            }else if(valueList[i] != lastBool){
                if(lastBool){ //om vi nyss passerat ett "true" område
                    endIndex = i-1;
                    GameObject squareGameObject = CreateRectangle(startIndex,endIndex);
                }
                startIndex = i;
            }
        }
    }



    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, Color connectionColor){
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        connectionColor.a = 0.7f;
        gameObject.GetComponent<Image>().color = connectionColor;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0,0);
        rectTransform.anchorMax = new Vector2(0,0);
        rectTransform.sizeDelta = new Vector2(distance,3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0,0, UtilsClass.GetAngleFromVectorFloat(dir));

    }
}
