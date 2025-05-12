using UnityEngine;

public class BreathingAnimation : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    [SerializeField] float inhaleDuration = 4f;
    [SerializeField] float holdDuration = 3f;
    [SerializeField] float exhaleDuration = 6f;
    [SerializeField] float scaleMultiplier = 1.1f;
    [SerializeField] bool pulsing = true;

    private enum BreathingPhase { Inhale, Hold, Exhale }
    private BreathingPhase currentPhase = BreathingPhase.Inhale;

    private float timer = 0f;
    private Vector3 originalScale;
    private Vector3 inhaleScale;

    private void Awake()
    {
        if (!targetObject)
            targetObject = this.gameObject;

        originalScale = targetObject.transform.localScale;
        inhaleScale = originalScale * scaleMultiplier;
    }

    private void Update()
    {
        if (!pulsing) return;

        timer += Time.deltaTime;

        switch (currentPhase)
        {
            case BreathingPhase.Inhale:
                float inhaleLerp = Mathf.Clamp01(timer / inhaleDuration);
                targetObject.transform.localScale = Vector3.Lerp(originalScale, inhaleScale, inhaleLerp);
                if (timer >= inhaleDuration)
                {
                    currentPhase = BreathingPhase.Hold;
                    timer = 0f;
                }
                break;

            case BreathingPhase.Hold:
                targetObject.transform.localScale = inhaleScale;
                if (timer >= holdDuration)
                {
                    currentPhase = BreathingPhase.Exhale;
                    timer = 0f;
                }
                break;

            case BreathingPhase.Exhale:
                float exhaleLerp = Mathf.Clamp01(timer / exhaleDuration);
                targetObject.transform.localScale = Vector3.Lerp(inhaleScale, originalScale, exhaleLerp);
                if (timer >= exhaleDuration)
                {
                    currentPhase = BreathingPhase.Inhale;
                    timer = 0f;
                }
                break;
        }
    }
}
