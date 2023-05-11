using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [0] ДЕТИ RAVE - ИНОГДА:                       Band4,   startSyncThreshold 0.4,   threshold 0.3,   BPM 126
// [1] HOTLINE MIAMI - KNOCK KNOCK:              Band6,   startSyncThreshold 0.1,   threshold 0.4,   BPM 110
// [2] LE CASTLE VANIA - JOHN WICK MODE:         Band0,   startSyncThreshold 0.3,   threshold 0.2,   BPM 124
// [3] SMACK, DJS FROM MARS - IT DOESN'T MATTER: ??
// [4] MIKE WILLIAMS - HERE FOR YOU:             ??       
// [5] SLAMTYPE - OLD SCHOOL VIBE:               ??

// Band 0: 0 Hz     to 86 Hz
// Band 1: 87 Hz    to 258 Hz
// Band 2: 259 Hz   to 602 Hz
// Band 3: 603 Hz   to 1290 Hz
// Band 4: 1291 Hz  to 2666 Hz
// Band 5: 2667 Hz  to 5418 Hz
// Band 6: 5419 Hz  to 10922 Hz
// Band 7: 10923 Hz to 22050 Hz
[RequireComponent(typeof(AudioSource))]
public class soundDetection : MonoBehaviour
{
    //Save information for each song manually in the inspector. Make sure the indices match
    public string[] musicTitle;
    public string[] musicArtist;
    public int[] musicFrequencyBands;
    public float[] musicThresholds;
    public float[] musicStartSyncThresholds;
    public int[] musicBPMs;
    public AudioClip[] songMP3;
    private AudioClip lastMusicPlayed;
    private int startSongIndex;

    public GameObject enemyHandler; // reference to enemyHandler
    public GameObject lightObject;
    public int frequencyBand = 6; // frequency band currently being looked at
    public float startSyncThreshold = 0.4f; // amplitude that needs to be exceded to start counting beats (avoid intros, etc)
    public float threshold = 0.3f; // amplitude required for +1 beatcount
    public float intensityMultiplier = 10f; // intensity for the light
    public int beatsPerSpawn; // size "beatCount" needs to reach before an enemy is spawned.
    public int BPM;

    private AudioSource audioSource;
    private Light lightComponent;

    private bool startedBeatSyncing;
    private int beatCount; // counts beats.
    private float[] samples; // not important, dont touch
    private float[] frequencyBands; // array of the 8 frequency bands

    void Start()
    {
        startSongIndex = Random.Range(0, 6);
        beatsPerSpawn = 30;
        startedBeatSyncing = false;
        beatCount = 0;
        audioSource = GetComponent<AudioSource>();
        lightComponent = lightObject.GetComponent<Light>();

        samples = new float[512];
        frequencyBands = new float[8];
        
        //Select startup song
        audioSource.clip = songMP3[startSongIndex];
        lastMusicPlayed = audioSource.clip;
        frequencyBand = musicFrequencyBands[startSongIndex];
        threshold = musicThresholds[startSongIndex];
        startSyncThreshold = musicStartSyncThresholds[startSongIndex];
        BPM = musicBPMs[startSongIndex];
        audioSource.Play();

        //Compensate for volume settings
        threshold = threshold*audioSource.volume;
        startSyncThreshold = startSyncThreshold*audioSource.volume;
    }

    void Update()
    {
        GetSpectrumData();
        CalculateFrequencyBands();
        ControlLightIntensity();

        //------- Continual random songs (no repeats) -------//
        if(!(audioSource.isPlaying)){ 
            PlayRandomSong();
        }
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
        
        
            if (frequencyBands[frequencyBand] > startSyncThreshold)
            {
                startedBeatSyncing = true;
            }
        
        if (startedBeatSyncing) // spawn enemies at higher amplitude than dash requirement
        {
            if (frequencyBands[frequencyBand] > threshold)
            {
                beatCount++;
                if (beatCount == beatsPerSpawn)
                {
                    
                    enemyHandler.GetComponent<EnemyHandler>().SpawnEnemy();
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

    void PlayRandomSong(){
        while(audioSource.clip == lastMusicPlayed){ //Don't repeat
                int randomIndex = Random.Range(0 , songMP3.Length);

                audioSource.clip = songMP3[randomIndex];
                frequencyBand = musicFrequencyBands[randomIndex];
                threshold = musicThresholds[randomIndex] * audioSource.volume;
                startSyncThreshold = musicStartSyncThresholds[randomIndex] * audioSource.volume;
                BPM = musicBPMs[randomIndex];
            }

            lastMusicPlayed = audioSource.clip;
            audioSource.Play();
    }
}