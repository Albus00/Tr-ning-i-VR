using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// https://forum.unity.com/threads/what-is-spectrum-data-audio-getspectrumdata.204060/

[RequireComponent(typeof(AudioSource))]
public class AudioP : MonoBehaviour
{   
    [SerializeField] bool useEqualLoudnessScale = true;
    private AudioSource audioSource;
    public static float[] samples = new float[512];
    public static float[] freqBand = new float[8];
    public static float[] bandbuffer = new float[8];
    float[] bufferdecrase = new float[8];
    public float changeRate = .5f;


    //Light
    float[] freqBandHighest = new float[8];
    public static float[] audioBand = new float[8];
    public static float[] audioBandbuffer = new float[8];

    private float[] equalLoudnessScale8 = new float[8];


    //
    public static float Amplitude, AmplitudeBuffer;
    float AmplitudeHighest;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        equalLoudnessScale8 = EqualLoudnessScale8();
        string microphonename = Microphone.devices[0];
        //audioSource.clip = Microphone.Start(microphonename, true, 10, 44100);
        audioSource.loop = true;
        //while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();
    }


    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        if(useEqualLoudnessScale) equalLoudnessFilter();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    void GetAmplitude()
    {
        float CurrentAmplitude = 0;
        float CurrentAmplitudeBuffer = 0;
        for(int i = 0; i < 8; i++)
        {
            CurrentAmplitude += audioBand[i];
            CurrentAmplitudeBuffer += audioBandbuffer[i];
        }
        if(CurrentAmplitude> AmplitudeHighest)
        {
            AmplitudeHighest = CurrentAmplitude;
        }
        Amplitude = CurrentAmplitude / AmplitudeHighest;

        AmplitudeBuffer = CurrentAmplitudeBuffer / AmplitudeHighest;
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman); // Kolla om en annan windowing metod kanske funkar bättre
    }

    //light
    void CreateAudioBands()
    {
        for (int i = 0; i < 8; ++i)
        {
            if (freqBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBand[i];
            }
            audioBand[i] = (freqBand[i] / freqBandHighest[i]);
            audioBandbuffer[i] = (bandbuffer[i] / freqBandHighest[i]);
        }

    }

    /*
     22050 / 512 = 43 Hz per sampel

     8 Bands:
        20-60hz
        250-500hz
        500-2000hz
        2000-4000hz
        4000-6000hz
        6000-20000hz
     */

    float[] freqSpans;
    void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7) { sampleCount += 2; }
            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }
            average /= count;
            freqBand[i] = average * 10;
        }
    }

    void BandBuffer()
    {
        for (int i = 0; i < 8; ++i)
        {
            if (freqBand[i] > bandbuffer[i])
            {
                bandbuffer[i] = freqBand[i];
                bufferdecrase[i] = 0.005f;
            }
            if (freqBand[i] < bandbuffer[i])
            {
                bandbuffer[i] -= bufferdecrase[i];
                bufferdecrase[i] *= 1.2f;
            }
        }
    }

    float[] EqualLoudnessScale8()
    {
        List<float> freqSpans = new List<float>();
        for (int i = 0; i < 8; i++)
        {
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            freqSpans.Add(sampleCount * 43);
        }
        freqSpans.Add(Mathf.Infinity);
        float[] result = new float[8];
        int band = 0;
        float sum = 0;
        int count = 0;
        for (int i = 0; i < EqualLoudness.freq.Length - 1; i++)
        {
            float test = EqualLoudness.freq[i];
            if (EqualLoudness.freq[i] >= freqSpans[band])
            {
                if (EqualLoudness.freq[i] <= freqSpans[band+1])
                {
                    sum += EqualLoudness.phon70[i];
                    count++;
                }
                if (EqualLoudness.freq[i] >= freqSpans[band + 1])
                {
                    result[band] = sum / count;
                    sum = count = 0;
                    band++;
                    i--;
                }
            }
        }
        if (band != result.Length) result[band]=sum/count;

        //Normolize output
        float max = 0;
        foreach(float s in result)
        {
            if (max < s) max = s;
        }
        for(int i = 0; i < result.Length-1; i++)
        {
            result[i] /= max;
        }
        return result;
    }
    void equalLoudnessFilter()
    {
        for (int i = 0; i < 8; i++)
        {
            freqBand[i] /= equalLoudnessScale8[i];
        }
    }
    void BandBuffer2()
    {
        for (int i = 0; i < 8; ++i)
        {
            bandbuffer[i] = Mathf.Lerp(bandbuffer[i], samples[i], changeRate * Time.deltaTime);
        }
    }
}
