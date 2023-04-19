using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
// KOLLA FMOD PÅ UNITY STORE
[RequireComponent (typeof (Light))]
public class LightOnAudio : MonoBehaviour
{ // TRY TO MAKE A BAND-INDEX TO DISPLAY IF THE SOUND IS SKEWED TOWARDS ONE FREQUENCY REGION
  // FLOAT VALUE THAT GOES FROM 0 -> 7 SHOWING WHAT BANDS ARE MOST ACTIVE


    // TRY TO MAKE THE FUNCTION: EnemyLungeOnBeat() to make the enemy do a quick dash towards player synced with the beat



    // ---------- INSPECTOR VARIABLES ---------- //
    [Range(0, 7)] 
    public int band; 
    //public int band;
    public float minIntensity, maxIntensity;
    public float lowerActivationLimitBand1;
   
    private float lightIntensity;
    private Light light;
    
    // Start is called before the first frame update
    void Start()
    {
        
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(band > 7){ band = 7; } // array out of bounds fix
        //if (band < 0){ band = 0; } // array out of bounds fix
        lightIntensity = (AudioP.audioBandbuffer[band] * (maxIntensity - minIntensity)) + minIntensity;
        SetIntensity();
    }


    private void SetIntensity()
    {

        if (AudioP.audioBandbuffer[band] >= lowerActivationLimitBand1)
        {
            
            light.intensity = lightIntensity;
           
        }
        else
        {
            light.intensity = 0f;
            
        }
       
    }

}
