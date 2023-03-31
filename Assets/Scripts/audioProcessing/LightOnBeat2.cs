using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Light))]

public class LightOnBeat2 : MonoBehaviour
{ // TRY TO MAKE A BAND-INDEX TO DISPLAY IF THE SOUND IS SKEWED TOWARDS ONE FREQUENCY REGION
  // FLOAT VALUE THAT GOES FROM 0 -> 7 SHOWING WHAT BANDS ARE MOST ACTIVE

    //https://www.gamedeveloper.com/programming/music-syncing-in-rhythm-games
    //https://www.gamedeveloper.com/audio/coding-to-the-beat---under-the-hood-of-a-rhythm-game-in-unity
    //https://answers.unity.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html
    // TRY TO MAKE THE FUNCTION: EnemyLungeOnBeat() to make the enemy do a quick dash towards player synced with the beat

    // ---------- INSPECTOR VARIABLES ---------- //
    [Range(0, 7)]
    public int band;
    //public int band;
    public float minIntensity, maxIntensity;
    public float lowerActivationLimitBand1;
    [Range(0, 7)]
    public int band2;
    public float lowerActivationLimitBand2;
    private float lightIntensity;
    private Light light;
    public bool fixedPulseRate = false;
    private float pulseRate = 0.545f;
    private float timer2;

    private int beatCount;
    public int beatsPerSpawn;
    public GameObject enemy;
    private float timer;
    public float minBeatInterval;

    // Start is called before the first frame update
    void Start()
    {
        minBeatInterval = 0.1f;
        beatCount = 0;
        beatsPerSpawn = 4;
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        lightIntensity = (AudioP.audioBandbuffer[band] * (maxIntensity - minIntensity)) + minIntensity;
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;

        if (fixedPulseRate)
        {
            if(timer2 >= pulseRate)
            {
                light.intensity = maxIntensity;
                Debug.Log("Pulse");
                timer2 = 0;
            }
            else
            {
                light.intensity = 0;
            }

        }


        if (AudioP.audioBandbuffer[band] >= lowerActivationLimitBand1)
        {
            light.intensity = lightIntensity;
            if (timer >= minBeatInterval)
            {
                beatCount++;
                timer = 0f;
            }
        }
        else
        {
            light.intensity = 0;
        }


        if (beatCount >= beatsPerSpawn)
        {
            Instantiate(enemy);
            beatCount = 0;
        }


        //Debug.Log(beatCount);
    }



    private void SetIntensityTwoBands()
    {
       
        //previousIntensity = lightIntensity;
    }

   
}
