using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ДЕТИ RAVE - ИНОГДА:            Band4,   startSyncThreshold 0.4,   threshold 0.3,   BPM 126
// HOTLINE MIAMI - KNOCK KNOCK:   Band6,   startSyncThreshold 0.1,   threshold 0.4,   BPM 110
// Band 0: 0 Hz to 86 Hz
// Band 1: 87 Hz to 258 Hz
// Band 2: 259 Hz to 602 Hz
// Band 3: 603 Hz to 1290 Hz
// Band 4: 1291 Hz to 2666 Hz
// Band 5: 2667 Hz to 5418 Hz
// Band 6: 5419 Hz to 10922 Hz
// Band 7: 10923 Hz to 22050 Hz
[RequireComponent(typeof(AudioSource))]
public class soundDetection : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    public GameObject lightObject;
    public int frequencyBand = 2;
    public float startSyncThreshold = 0.4f;
    public float threshold = 0.3f;
    public float intensityMultiplier = 10f;
    public int beatsPerSpawn;

    private AudioSource audioSource;
    private Light lightComponent;

    private bool startedBeatSyncing;
    private int beatCount;
    private float[] samples;
    private float[] frequencyBands;

    void Start()
    {
        beatsPerSpawn = 30;
        startedBeatSyncing = false;
        beatCount = 0;
        audioSource = GetComponent<AudioSource>();
        lightComponent = lightObject.GetComponent<Light>();

        samples = new float[512];
        frequencyBands = new float[8];
    }

    void Update()
    {
        GetSpectrumData();
        CalculateFrequencyBands();
        ControlLightIntensity();
    }

    void GetSpectrumData()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void CalculateFrequencyBands()
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;
            frequencyBands[i] = average;
        }
    }

    void ControlLightIntensity()
    {
        //Debug.Log(frequencyBands[frequencyBand]);
        if (!startedBeatSyncing)
        {
            if (frequencyBands[frequencyBand] > startSyncThreshold)
            {
                startedBeatSyncing = true;
            }
        }
        if (startedBeatSyncing) // spawn enemies at higher amplitude than dash requirement
        {
            if (frequencyBands[frequencyBand] > threshold)
            {
                beatCount++;
                if (beatCount == beatsPerSpawn)
                {
                    beatCount = 0;
                    int numOfEnemies = Random.Range(1, 4);
                    for (int i = 0; i < numOfEnemies; i++)
                    {
                        float randomX = Random.Range(-5f, 5f);
                        Vector3 spawnPosition = new Vector3(randomX, enemySpawnPoint.position.y, enemySpawnPoint.position.z);
                        //Instantiate(enemyPrefab, spawnPosition, enemySpawnPoint.rotation); //just to stop spawning while testing
                    }
                    beatCount = 0; // dumb, done just in case it it affects spawn rate
                }

                GameObject[] enemies;
                enemies = GameObject.FindGameObjectsWithTag("enemy");
                foreach (GameObject e in enemies)
                {
                    e.GetComponent<BehaviourTest>().BeatReceiver();
                }
                lightComponent.enabled = true;
                lightComponent.intensity = frequencyBands[frequencyBand] * intensityMultiplier;
            }
            else
            {
                lightComponent.enabled = false;
            }
        }
    }
}