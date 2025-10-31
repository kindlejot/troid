using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : Obstacle
{
    Vector3 rotationAxis;
    float angularSpeed;

    void Randomize ()
    {
        // Randomize the spin axis and angular speed
        rotationAxis = Random.onUnitSphere;
        angularSpeed = Random.Range (50, 150);

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
        transform.Rotate (rotationAxis, angularSpeed * Time.deltaTime);
    }
}
