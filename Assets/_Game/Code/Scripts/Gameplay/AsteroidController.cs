using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : Obstacle
{
    private Vector3 _rotationAxis;
    private float _angularSpeed;

    private ObstacleAsteroid _config;

    public bool CanFragment()
    {
        return _config != null && _config.Fragments > 1;
    }

    public ObstacleData GetFragment (bool lhs)
    {
        ObstacleData result = new ObstacleData();

        ObstacleAsteroid newConfig = Instantiate(_config);
        newConfig.Fragments--;
        newConfig.RelativeSize *= .75f;
        newConfig.Health = newConfig.Fragments * 10;
        result.Obstacle = newConfig;

        SpawnConfig spawner = ScriptableObject.CreateInstance<SpawnConfig>();
        spawner.Position = transform.position;
        result.Spawner = spawner;

        MovementConfig movementConfig = ScriptableObject.CreateInstance<MovementConfig>();
        if (_movementConfig != null && _movementConfig.Velocity != 0)
        {
            movementConfig.Direction = Vector3.Cross(_movementConfig.Direction, lhs ? Vector3.forward : Vector3.back);
            movementConfig.Direction = Quaternion.Euler(0, 0, Random.Range(-30, 30)) * movementConfig.Direction;
            movementConfig.Velocity = _movementConfig.Velocity;
        }
        else
        {
            movementConfig.Direction = Random.insideUnitCircle;
            movementConfig.Velocity = 1;
        }
        result.Movement = movementConfig;
        return result;
    }

    void Randomize ()
    {
        // Randomize the spin axis and angular speed
        _rotationAxis = Random.onUnitSphere;
        _angularSpeed = Random.Range (10, 100);

        // Randomize starting position
        transform.rotation = Random.rotation;
    }

    public override void Init(Vector2 position, ObstacleConfig config, MovementConfig movement = null)
    {
        if (config is ObstacleAsteroid oa)
        {
            _config = oa;
            transform.localScale = new Vector3(oa.RelativeSize, oa.RelativeSize, oa.RelativeSize);
        }

        base.Init(position, config, movement);
        Randomize();
    }

    protected override void Destruct()
    {
        base.Destruct ();
    }

    protected override void Movement()
    {
        base.Movement();
        transform.Rotate (_rotationAxis, _angularSpeed * Time.deltaTime);
    }
}
