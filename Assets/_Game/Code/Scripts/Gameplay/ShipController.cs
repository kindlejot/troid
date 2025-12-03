using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    // Exposed to inspector
    [Header("Spawnable GO references")]
    [SerializeField] private GameObject projectile;

    [Header("Control Configuration")]
    [SerializeField] private float maxRotationSpeed = 180;
    [SerializeField] private float rotationSpeedAcceleration = 720;

    [SerializeField] private float maxForwardVelocity = 8;
    [SerializeField] private float maxReverseVelocity = -6;
    [SerializeField] private float velocityAcceleration = 26;
    [SerializeField] private float velocityDeceleration = 17;

    [SerializeField] private float safeSpawnDistance = 5.0f;

    [SerializeField, Tooltip("RoF for autofire")] private float rateOfFire = 5;

    // Class variables
    private PlayerControls.GameplayActions _gameplayActions;

    private float _currentRotationSpeed = 0;
    private float _currentVelocity = 0;

    private float _projectileDelay;

    // Track Input Action states
    private bool _isShooting;
    private float _currentSteeringInput;
    private float _currentAccelerateInput;

    private void OnEnable()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogError("FATAL: InputManager not found in the scene! Controls can't be assigned.");
            return;
        }

        _gameplayActions = InputManager.Instance.GetGameplayActions();

        _gameplayActions.Shoot.performed += OnShoot;
        _gameplayActions.Shoot.canceled += OnShoot;

        _gameplayActions.Steer.performed += OnSteer;
        _gameplayActions.Steer.canceled += OnSteer;

        _gameplayActions.Accelerate.performed += OnAccelerate;
        _gameplayActions.Accelerate.canceled += OnAccelerate;

        _isShooting = false;
        _currentSteeringInput = 0;
        _currentAccelerateInput = 0;
    }

    private void OnDisable()
    {
        if (_gameplayActions.Accelerate != null)
        {
            _gameplayActions.Shoot.performed -= OnShoot;
            _gameplayActions.Shoot.canceled -= OnShoot;

            _gameplayActions.Steer.performed -= OnSteer;
            _gameplayActions.Steer.canceled -= OnSteer;

            _gameplayActions.Accelerate.performed -= OnAccelerate;
            _gameplayActions.Accelerate.canceled -= OnAccelerate;
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        _isShooting = context.performed;
        _projectileDelay = 0;
    }

    public void OnSteer(InputAction.CallbackContext context) => _currentSteeringInput = context.ReadValue<float>();
    public void OnAccelerate(InputAction.CallbackContext context) => _currentAccelerateInput = context.ReadValue<float>();

    // For object avoidance on spawn
    public bool IsSafeToSpawn (Vector2 position)
    {
        if (((Vector2)transform.position-position).magnitude < safeSpawnDistance)
            return false;

        return true;
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.GetComponent<Obstacle>() != null) {
            Implode ();
        }
    }

    void Implode ()
    {
        FeedbackManager.Instance.PlayImplosionFeedback(transform.position);
        GameManager.Instance.ChangeState (GameState.GameOver);
    }

    public void Reset ()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        _currentRotationSpeed = 0;
        _currentVelocity = 0;
    }

    void Update()
    {
        UpdateRotation ();
        UpdateAcceleration ();
        UpdateShooting ();
    }

    void UpdateRotation ()
    {
        if (_currentSteeringInput < 0) // Rotate counter clockwise
        {
            _currentRotationSpeed -= rotationSpeedAcceleration * Time.deltaTime;
            _currentRotationSpeed = Mathf.Max(-maxRotationSpeed, _currentRotationSpeed);
        }
        else if (_currentSteeringInput > 0) // Rotate clockwise
        {
            _currentRotationSpeed += rotationSpeedAcceleration * Time.deltaTime;
            _currentRotationSpeed = Mathf.Min(maxRotationSpeed, _currentRotationSpeed);
        }
        else
        {
            _currentRotationSpeed = 0;
        }

        if (_currentRotationSpeed != 0)
        {
            transform.Rotate(Vector3.back * Time.deltaTime * _currentRotationSpeed);
        }
    }

    void UpdateAcceleration ()
    {
        if (_currentAccelerateInput > 0) // Accelerate
        {
            _currentVelocity += velocityAcceleration * Time.deltaTime;
            _currentVelocity = Mathf.Min(_currentVelocity, maxForwardVelocity);
        }
        else if (_currentAccelerateInput < 0) // Reverse
        {
            _currentVelocity -= velocityAcceleration * Time.deltaTime;
            _currentVelocity = Mathf.Max(_currentVelocity, maxReverseVelocity);
        }
        else
        {
            if (_currentVelocity != 0) // Decelerate
            {
                float deceleration = Mathf.Min (velocityDeceleration * Time.deltaTime, Mathf.Abs (_currentVelocity));
                _currentVelocity -= _currentVelocity > 0 ? deceleration : -deceleration;
            }
        }

        if (_currentVelocity != 0)
        {
            transform.Translate(Vector3.up * _currentVelocity * Time.deltaTime);
        }
    }

    void LaunchProjectile ()
    {
        GameObject instance = Instantiate(projectile, transform.position, transform.rotation);
        instance.GetComponent<ProjectileController>().Eject(transform.up);
        AudioManager.Instance.PlayShooting();
    }

    void UpdateShooting ()
    {
        if (_isShooting)
        {
            _projectileDelay -= Time.deltaTime;
            if (_projectileDelay <= 0)
            {
                LaunchProjectile();
                _projectileDelay = 1.0f / rateOfFire;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Debug.DrawRay(
            transform.position,
            transform.up * _currentVelocity / maxForwardVelocity, // Cap to 1 unit long
            Color.yellow);
    }
}
