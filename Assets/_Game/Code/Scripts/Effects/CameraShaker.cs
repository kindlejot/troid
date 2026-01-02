using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private Vector3 _initialLocalPosition;
    private Coroutine _currentShakeRoutine;

    private void Awake()
    {
        _initialLocalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        FeedbackManager.OnCameraShakeRequested += TriggerShake;
    }

    private void OnDisable()
    {
        FeedbackManager.OnCameraShakeRequested -= TriggerShake;
    }

    public void TriggerShake (float duration, float magnitude)
    {
        if (_currentShakeRoutine != null)
        {
            StopCoroutine(_currentShakeRoutine);
        }

        _currentShakeRoutine = StartCoroutine (ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine (float duration, float magnitude)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            float damper = Mathf.SmoothStep(magnitude, 0, elapsedTime / duration);
            transform.localPosition = _initialLocalPosition + (Vector3)(Random.insideUnitCircle * damper);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = _initialLocalPosition;
        _currentShakeRoutine = null;
    }
}
