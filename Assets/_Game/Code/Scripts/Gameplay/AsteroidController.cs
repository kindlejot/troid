using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : Obstacle
{
    public int MaxFragments = 4;

    public float MinVelocity = 1;
    public float MaxVelocity = 3;

    public int FragmentDepth
    {
        get => fragmentDepth;
        set {
            fragmentDepth = value;
            float scale = Mathf.Pow (.8f, fragmentDepth);
            transform.localScale = new Vector3 (scale, scale, scale);
        }
    }

    int fragmentDepth = 0;

    float velocity;

    Vector3 rotationAxis;
    float angularSpeed;

    void Randomize ()
    {
        // Randomize the travel direction and velocity
        velocity = Random.Range (MinVelocity, MaxVelocity);

        // Randomize the spin axis and angular speed
        rotationAxis = Random.onUnitSphere;
        angularSpeed = Random.Range (10, 100);

        // Randomize starting position
        transform.rotation = Random.rotation;
    }

    public override void Init(Vector2 position, Vector2 direction)
    {
        base.Init (position, direction);
        Randomize();
    }

    protected override void Destruct()
    {
        base.Destruct ();
    }

    protected override void Movement()
    {
        transform.position += Direction * velocity * Time.deltaTime;
        transform.Rotate (rotationAxis, angularSpeed * Time.deltaTime);
    }
}
