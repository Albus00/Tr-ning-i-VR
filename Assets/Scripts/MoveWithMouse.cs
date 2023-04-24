using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveWithMouse : MonoBehaviour
{
    private Color slow = Color.red;
    private Color fast = Color.green;

    private float threshold = 6.0f;

    private float pos_x = 0;
    private float pos_y = 0;

    private float old_x = 0;
    private float old_y = 0;

    private float speedx = 0;
    private float speedy = 0;

    //Ugly, hacky and downright disrespectful moving average
    private float prev_speedx = 0;
    private float prev_speedy = 0;

    private float prev_prev_speedx = 0;
    private float prev_prev_speedy = 0;

    private float avg3_speedx = 0;
    private float avg3_speedy = 0;

    public TMPro.TMP_Text xpos_display;
    public TMPro.TMP_Text ypos_display;

    public TMPro.TMP_Text xprev_display;
    public TMPro.TMP_Text yprev_display;

    public TMPro.TMP_Text xspeed_display;
    public TMPro.TMP_Text yspeed_display;

    public TMPro.TMP_Text xspeedavg3_display;
    public TMPro.TMP_Text yspeedavg3_display;
     
     
     // Use this for initialization
     void Start () {
        Application.targetFrameRate = 92;

     }
     
     // Update is called once per frame
     void Update () {         
        Vector3 mouse = Camera.main.ScreenToViewportPoint(Input.mousePosition);
         
         //FIND CURRENT POSITION
        pos_x = mouse.x;
        pos_y = mouse.y;

        transform.position = new Vector3(mouse.x, mouse.y, transform.position.z);

        // (NEW-OLD)/TIME = Velocity     (thats basic physics baybeee)
        speedx = (pos_x-old_x)/Time.deltaTime;
        speedy = (pos_y-old_y)/Time.deltaTime;

        UpdateText();

        if(Mathf.Abs(speedx)>=threshold){
            Debug.Log(Mathf.Abs(speedx));
        }
        if(Mathf.Abs(speedy)>=threshold){
            Debug.Log(Mathf.Abs(speedy));
        }

        //Ugliest piece of sh{} code, please redo with arrays or something
        //Moving average over 3 frames
        prev_prev_speedx = prev_speedx;
        prev_prev_speedy = prev_speedy;

        prev_speedx = speedx;
        prev_speedy = speedy;

        avg3_speedx = Mathf.Abs((speedx + prev_speedx + prev_prev_speedx)/3.0f);
        avg3_speedy = Mathf.Abs((speedy + prev_speedy + prev_prev_speedy)/3.0f);

        //UPDATE OLD POSITION
        old_x = pos_x;
        old_y = pos_y;

     }

     void UpdateText(){
        xpos_display.text = pos_x.ToString();  
        ypos_display.text = pos_y.ToString();        

        xprev_display.text = old_x.ToString();
        yprev_display.text = old_y.ToString();

        xspeed_display.text = Mathf.Abs(speedx).ToString();
        yspeed_display.text = Mathf.Abs(speedy).ToString();

        xspeedavg3_display.text = Mathf.Abs(avg3_speedx).ToString();
        yspeedavg3_display.text = Mathf.Abs(avg3_speedy).ToString();   
     }
}
