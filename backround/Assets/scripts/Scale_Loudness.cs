using UnityEngine;

public class Scale_Loudness : MonoBehaviour
{
    // public AudioSource source;
    public Vector3 minScale;
    public Vector3 maxScale;
    public AudioLoudnessDetection detector;
    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    public float ypos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ypos = transform.position.y;
    }


    // Update is called once per frame
    public void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;
        if (loudness > threshold)
        {
            transform.localScale = maxScale;
            transform.position = new Vector3(transform.position.x,-2, transform.position.z);
            // Debug.Log("Scaling UP");
        }
        else
        {
            transform.localScale = minScale;
            transform.position = new Vector3(transform.position.x, ypos, transform.position.z);
            // Debug.Log("Scalin/g DOWN");
        }
        // transform.localScale = Vector2.Lerp(minScale, maxScale, loudness);
        Debug.Log("Loudness: " + loudness);
        // Debug.Log("Scale: " + transform.localScale);
    }
}
