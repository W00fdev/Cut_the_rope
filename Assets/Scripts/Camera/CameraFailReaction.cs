using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraFailReaction : MonoBehaviour
{
    public CanvasGroup RedEffect;

    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 1f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    private float currentDuration;
    private Vector3 originalPos;

    private void Awake()
    {
        originalPos = transform.position;
        currentDuration = shakeDuration;
    }

    public void Play()
    {
        currentDuration = shakeDuration;
        camTransform.localPosition = originalPos;
        DOTween.Sequence()
            .Append(RedEffect.DOFade(.3f, shakeDuration / 2f))
            .Append(RedEffect.DOFade(0f, shakeDuration / 2f));
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        while (currentDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            currentDuration -= Time.deltaTime * decreaseFactor;
            yield return null;
        }
        camTransform.localPosition = originalPos;
    }
}