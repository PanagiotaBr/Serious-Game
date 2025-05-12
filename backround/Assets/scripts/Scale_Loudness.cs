using UnityEngine;

public class Scale_Loudness : MonoBehaviour
{
    public AudioSource source;
    public Vector2 minScale;
    public Vector2 maxScale;
    public AudioLoudnessDetection detector;
    public float loudnessSensibility = 100;
    public float threshold = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;
        if (loudness < threshold)
            loudness = 0;
        transform.localScale = Vector2.Lerp(minScale, maxScale,loudness);
    }
}
