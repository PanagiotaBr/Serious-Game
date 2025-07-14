using UnityEngine;
using System.Collections;


public class AudioLoudnessDetection : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;
    
    void Start()
    {
        if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            StartCoroutine(RequestMicPermission());
        }
        else
        {
            MicrophoneToAudioClip();
        }
    }

    IEnumerator RequestMicPermission()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            MicrophoneToAudioClip();
        }
    }


    public void MicrophoneToAudioClip()
    {
    string microphoneName = Microphone.devices[0];
    microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    StartCoroutine(WaitForMicPosition());
    }

    private IEnumerator WaitForMicPosition()
    {
    yield return new WaitUntil(() => Microphone.GetPosition(Microphone.devices[0]) > 0);
    Debug.Log("Microphone ready!");
    }

    public float GetLoudnessFromMicrophone()
    {
        if (microphoneClip == null || !Microphone.IsRecording(Microphone.devices[0]))
            return 0;

        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
            return 0; 

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData,startPosition);

        //compute loudness
        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }
}
