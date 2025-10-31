using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Obstacle : MonoBehaviour
{
    public int Health = 10;

    public UnityAction<GameObject> OnDestruction;

    protected int health;
    public Vector3 Direction;

    public virtual void Init (Vector2 position, Vector2 direction)
    {
        health = Health;
        transform.position = position;
        this.Direction = direction;
    }

    protected abstract void Movement ();

    public virtual void Hit (int damage)
    {
        health -= damage;

        if (health <= 0) {
            Destruct ();
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
