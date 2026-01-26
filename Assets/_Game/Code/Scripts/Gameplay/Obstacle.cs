using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Obstacle : MonoBehaviour
{
    public UnityAction<GameObject> OnDestruction;

    public int Points => _points;

    protected int _health;
    protected int _points;

    protected MovementConfig _movementConfig;
    protected float _timer;

    Coroutine _hitFlashCoroutine;

    public virtual void Init (Vector2 position, ObstacleConfig config, MovementConfig movement = null)
    {
        _health = config.Health;
        _points = config.Points;
        _movementConfig = movement;
        transform.position = position;
    }

    protected virtual void Movement ()
    {
        if (_movementConfig == null)
        {
            return;
        }

        Vector3 direction = (Vector3)_movementConfig.Direction.normalized;
        if (_movementConfig.IsTurning)
        {
            float duration = _movementConfig.TurningDuration;
            float angle = _movementConfig.AngleCurve.Evaluate((_timer % duration) / duration);
            direction = Quaternion.Euler(0, 0, angle) * direction;
        }
        transform.position += direction * _movementConfig.Velocity * Time.deltaTime;
    }

    public virtual void Hit (int damage, Vector3 impactPoint)
    {
        _health -= damage;

        if (_health <= 0) {
            FeedbackManager.Instance.PlayDestructionFeedback(transform.position);
            Destruct ();
        } else {
            if (_hitFlashCoroutine != null) {
                StopCoroutine(_hitFlashCoroutine);
            }
            _hitFlashCoroutine = StartCoroutine(GetComponent<MeshRenderer>().HitFlash());
            FeedbackManager.Instance.PlayHitFeedback(impactPoint, Quaternion.LookRotation(impactPoint - transform.position));
        }
    }

    protected virtual void Destruct()
    {
        OnDestruction?.Invoke (gameObject);
    }

    void Update () {
        _timer += Time.deltaTime;
        Movement ();
    }
}
