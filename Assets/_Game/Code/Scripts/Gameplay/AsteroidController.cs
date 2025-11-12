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
        get => _fragmentDepth;
        set {
            _fragmentDepth = value;
            float scale = Mathf.Pow (.8f, _fragmentDepth);
            transform.localScale = new Vector3 (scale, scale, scale);
        }
    }

    private int _fragmentDepth = 0;

    private float _velocity;

    private Vector3 _rotationAxis;
    private float _angularSpeed;

    void Randomize ()
    {
        // Randomize the travel direction and velocity
        _velocity = Random.Range (MinVelocity, MaxVelocity);

        // Randomize the spin axis and angular speed
        _rotationAxis = Random.onUnitSphere;
        _angularSpeed = Random.Range (10, 100);

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
        transform.position += Direction * _velocity * Time.deltaTime;
        transform.Rotate (_rotationAxis, _angularSpeed * Time.deltaTime);
    }
}
