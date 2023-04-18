using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ActionOnBeat : MonoBehaviour // F�r tr�tt f�r den h�r skiten
{
    //https://www.gamedeveloper.com/programming/music-syncing-in-rhythm-games
    //https://www.gamedeveloper.com/audio/coding-to-the-beat---under-the-hood-of-a-rhythm-game-in-unity
    //https://answers.unity.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html
    // ------------ Audio Processing ------------ //
    [Range(0, 7)]
    public int band;
    public float lowerActivationLimit;
    // --- from 2nd link --- //
    private float songPosition;
    private float secPerBeat;
    private float dspSongTime;
    public float songBpm;
    public float songPositionInBeats;



    // ------------ Boolean flags ------------ //
    private bool allowBeatCounterIncrement;
    private bool doAction;
   

    // ------------ Timers ------------ //
    private float timer;
    private float beatTimestamp;
    private float beatDetectionPaddingTime; // dont record another beat within 0.1s after the most recent one

    // ------------ Counters ------------ //
    private int beatCount;

    // ------------ testing ------------ //
    private Transform target;
    private float dashTimer = 0.5f;
    private bool dashing;

    void Start()
    {
        dspSongTime = (float)AudioSettings.dspTime;
        secPerBeat = 60f / songBpm;

        target = GameObject.FindWithTag("Player").transform;
        dashing = false;
        
        allowBeatCounterIncrement = true;
        lowerActivationLimit = 0.5f;
        beatCount= 0;
        beatDetectionPaddingTime = 0.1f;
        doAction= false;
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime);
        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;
        // replace "Input.GetKeyDown(KeyCode.Space)" for beat detection
        if (Input.GetKeyDown(KeyCode.Space) && timer >= dashTimer)
        {
            StartCoroutine(Do_Wait_Reset());
        }
        if (dashing)
        {
            Dash();
        }
        Debug.Log(dashing);
    }



    IEnumerator Do_Wait_Reset()
    {
        dashing = true;
        yield return new WaitForSeconds(0.2f);
        dashing = false;
    }
    private void Dash()
    {
        timer = 0;
        gameObject.transform.LookAt(target);
        //Vector3 targetDirection = target.position - gameObject.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), 10f * Time.deltaTime);
        //Debug.Log("It worked! How?...");
    }




    private void IncrementBeatCounter() // counts beats with amplitude above the threshold set by lowerActivationLimit
    {
        beatTimestamp = Time.time;
        beatCount++;
        allowBeatCounterIncrement = false;
    }

    private void BeatCheck()
    {
        if(AudioP.audioBandbuffer[band] >= lowerActivationLimit && allowBeatCounterIncrement)
        {
            IncrementBeatCounter();
            beatTimestamp = 0;
        }
    }
}
