using UnityEngine;
using TMPro;
using System.Collections;

public class BreathingManager : MonoBehaviour
{
    [Header("Breathing Targets")]
    [SerializeField] GameObject[] targets;

    [Header("Breathing Timing")]
    [SerializeField] float inhaleDuration = 4f;
    [SerializeField] float holdDuration = 3f;
    [SerializeField] float exhaleDuration = 6f;
    [SerializeField] float scaleMultiplier = 1.1f;
    [SerializeField] bool pulsing = true;

    [Header("Breathing Text Display")]
    [SerializeField] TMP_Text breathingPhaseText;
    [SerializeField] CanvasGroup textCanvasGroup;
    [SerializeField] float fadeDuration = 0.5f;

    private enum Phase { Inhale, Hold, Exhale }
    private Phase currentPhase = Phase.Inhale;
    private float timer = 0f;

    private Vector3[] originalScales;
    private Vector3[] inhaleScales;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        // Prepare scale arrays
        originalScales = new Vector3[targets.Length];
        inhaleScales = new Vector3[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            originalScales[i] = targets[i].transform.localScale;
            inhaleScales[i] = originalScales[i] * scaleMultiplier;
        }

        // Set initial text
        UpdateBreathingText(currentPhase);
    }

    private void Update()
    {
        if (!pulsing) return;

        timer += Time.deltaTime;
        float t = 0f;

        switch (currentPhase)
        {
            case Phase.Inhale:
                t = Mathf.Clamp01(timer / inhaleDuration);
                ApplyScale(originalScales, inhaleScales, t);
                if (timer >= inhaleDuration) NextPhase(Phase.Hold);
                break;

            case Phase.Hold:
                ApplyScale(inhaleScales, inhaleScales, 0f);
                if (timer >= holdDuration) NextPhase(Phase.Exhale);
                break;

            case Phase.Exhale:
                t = Mathf.Clamp01(timer / exhaleDuration);
                ApplyScale(inhaleScales, originalScales, t);
                if (timer >= exhaleDuration) NextPhase(Phase.Inhale);
                break;
        }
    }

    private void ApplyScale(Vector3[] from, Vector3[] to, float t)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].transform.localScale = Vector3.Lerp(from[i], to[i], t);
        }
    }

    private void NextPhase(Phase next)
    {
        currentPhase = next;
        timer = 0f;
        UpdateBreathingText(currentPhase);
    }

    private void UpdateBreathingText(Phase phase)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        string newText = "";

        switch (phase)
        {
            case Phase.Inhale:
                newText = "<size=28><b>Inhale</b></size>\n<size=20>from your nose</size>";
                break;
            case Phase.Hold:
                newText = "<size=28><b>Hold</b></size>";
                break;
            case Phase.Exhale:
                newText = "<size=28><b>Exhale</b></size>\n<size=20>through your mouth</size>";
                break;
        }

        fadeCoroutine = StartCoroutine(FadeText(newText));
    }

    private IEnumerator FadeText(string newText)
    {
        // Fade out
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            textCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        breathingPhaseText.text = newText;

        // Fade in
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            textCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        textCanvasGroup.alpha = 1f;
    }
}
