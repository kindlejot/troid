using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : Obstacle
{
    private Vector3 _rotationAxis;
    private float _angularSpeed;

    void Randomize ()
    {
        // Randomize the spin axis and angular speed
        _rotationAxis = Random.onUnitSphere;
        _angularSpeed = Random.Range (50, 150);

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
        transform.Rotate (_rotationAxis, _angularSpeed * Time.deltaTime);
    }
}
