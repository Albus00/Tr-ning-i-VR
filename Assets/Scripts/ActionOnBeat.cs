using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
// KOLLA FMOD PÅ UNITY STORE
[RequireComponent(typeof(Light))]
public class ActionOnBeat : MonoBehaviour
{ // TRY TO MAKE A BAND-INDEX TO DISPLAY IF THE SOUND IS SKEWED TOWARDS ONE FREQUENCY REGION
  // FLOAT VALUE THAT GOES FROM 0 -> 7 SHOWING WHAT BANDS ARE MOST ACTIVE


    // TRY TO MAKE THE FUNCTION: EnemyLungeOnBeat() to make the enemy do a quick dash towards player synced with the beat

    public GameObject enemy;

    // ---------- INSPECTOR VARIABLES ---------- //
    [Range(0, 7)]
    public int band;
    //public int band;
    public float minIntensity, maxIntensity;
    public float lowerActivationLimitBand1;
    public bool usesTwoBands = false;
    [Range(0, 7)]
    public int band2;
    public float lowerActivationLimitBand2;

    private float lightIntensity;
    private float previousIntensity;
    private float previousAmplitude;
    private float momentum;
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

        Debug.Log(AudioP.audioBandbuffer[band]);


        if (!usesTwoBands)
        {
            SetIntensity();
        }
        else
        {
            SetIntensityTwoBands();
        }



        //Debug.Log("band1: " + AudioP.audioBandbuffer[band] + "Band2: " + AudioP.audioBandbuffer[band2]);
    }


    private void SetIntensity()
    {

        if (AudioP.audioBandbuffer[band] >= lowerActivationLimitBand1)
        {

            light.intensity = lightIntensity;
            //enemy.GetComponent<BehaviourTest>().beatReciever();
        }
        //else
        //{
        //    if (lightIntensity > minIntensity)
        //    {
        //        light.intensity = previousIntensity / 3;
        //    }
        //    else
        //    {
        //        light.intensity = minIntensity;
        //    }
        //    //light.intensity = minIntensity;
        //    //light.intensity = previousIntensity / 3;
        //}
        //previousIntensity= lightIntensity;
    }

    private void SetIntensityTwoBands()
    {
        if (AudioP.audioBandbuffer[band] >= lowerActivationLimitBand1 && AudioP.audioBandbuffer[band2] >= lowerActivationLimitBand2)
        {
            light.intensity = lightIntensity;
        }
        //else
        //{
        //    if(lightIntensity > minIntensity)
        //    {
        //        light.intensity = previousIntensity / 3; 
        //    }
        //    else
        //    {
        //        light.intensity = minIntensity;
        //    }
        //    //light.intensity = minIntensity;
        //    //light.intensity = previousIntensity / 3;
        //}
        //previousIntensity = lightIntensity;
    }

    private void findLoudestFreq()
    {

    }
}