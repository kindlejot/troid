using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Obstacle : MonoBehaviour
{
    public int Health = 10;
    public Vector3 Direction;

    public UnityAction<GameObject> OnDestruction;

    protected int health;

    public virtual void Init (Vector2 position, Vector2 direction)
    {
        health = Health;
        transform.position = position;
        this.Direction = direction;
    }

    protected abstract void Movement ();

    public virtual void Hit (int damage, Vector3 impactPoint)
    {
        health -= damage;

        if (health <= 0) {
            FeedbackManager.Instance.PlayDestructionFeedback(transform.position);
            Destruct ();
        } else
        {
            FeedbackManager.Instance.PlayHitFeedback(impactPoint, Quaternion.LookRotation(impactPoint - transform.position));
        }
    }

    protected virtual void Destruct()
    {
        OnDestruction?.Invoke (gameObject);
    }

    void Update () {
        Movement ();
    }
}
