using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    private PlayerControls.GameplayActions gameplayActions;

    public GameObject Projectile;

    public GameObject ImplosionFX;

    public float MaxRotationSpeed = 180;
    public float RotationSpeedAcceleration = 720;

    public float MaxForwardVelocity = 8;
    public float MaxReverseVelocity = -6;
    public float VelocityAcceleration = 26;
    public float VelocityDeceleration = 17;

    public float SafeSpawnDistance = 5.0f;

    public float RateOfFire = 5; 

    float currentRotationSpeed = 0;
    float currentVelocity = 0;

    float projectileDelay;

    // Track Input Action states
    private bool isShooting;
    private float currentSteeringInput;
    private float currentAccelerateInput;

    private void OnEnable()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogError("FATAL: InputManager not found in the scene! Controls can't be assigned.");
            return;
        }

        gameplayActions = InputManager.Instance.GetGameplayActions();

        gameplayActions.Shoot.performed += OnShoot;
        gameplayActions.Shoot.canceled += OnShoot;

        gameplayActions.Steer.performed += OnSteer;
        gameplayActions.Steer.canceled += OnSteer;

        gameplayActions.Accelerate.performed += OnAccelerate;
        gameplayActions.Accelerate.canceled += OnAccelerate;
    }

    private void OnDisable()
    {
        if (gameplayActions.Accelerate != null)
        {
            gameplayActions.Shoot.performed -= OnShoot;
            gameplayActions.Shoot.canceled -= OnShoot;

            gameplayActions.Steer.performed -= OnSteer;
            gameplayActions.Steer.canceled -= OnSteer;

            gameplayActions.Accelerate.performed -= OnAccelerate;
            gameplayActions.Accelerate.canceled -= OnAccelerate;
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        isShooting = context.performed;
        projectileDelay = 0;
    }

    public void OnSteer(InputAction.CallbackContext context) => currentSteeringInput = context.ReadValue<float>();
    public void OnAccelerate(InputAction.CallbackContext context) => currentAccelerateInput = context.ReadValue<float>();



    // For object avoidance on spawn
    public bool IsSafeToSpawn (Vector2 position)
    {
        if (((Vector2)transform.position-position).magnitude < SafeSpawnDistance)
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
        Instantiate (ImplosionFX, transform.position, Quaternion.identity);
        AudioManager.Instance.PlayShipExploding ();
        GameManager.Instance.ChangeState (GameState.GameOver);
    }

    public void Reset ()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        currentRotationSpeed = 0;
        currentVelocity = 0;
    }

    void Update()
    {
        UpdateRotation ();
        UpdateAcceleration ();
        UpdateShooting ();
    }

    void UpdateRotation ()
    {
        if (currentSteeringInput < 0) // Rotate counter clockwise
        {
            currentRotationSpeed -= RotationSpeedAcceleration * Time.deltaTime;
            currentRotationSpeed = Mathf.Max(-MaxRotationSpeed, currentRotationSpeed);
        }
        else if (currentSteeringInput > 0) // Rotate clockwise
        {
            currentRotationSpeed += RotationSpeedAcceleration * Time.deltaTime;
            currentRotationSpeed = Mathf.Min(MaxRotationSpeed, currentRotationSpeed);
        }
        else
        {
            currentRotationSpeed = 0;
        }

        if (currentRotationSpeed != 0)
        {
            transform.Rotate(Vector3.back * Time.deltaTime * currentRotationSpeed);
        }
    }

    void UpdateAcceleration ()
    {
        if (currentAccelerateInput > 0) // Accelerate
        {
            currentVelocity += VelocityAcceleration * Time.deltaTime;
            currentVelocity = Mathf.Min(currentVelocity, MaxForwardVelocity);
        }
        else if (currentAccelerateInput < 0) // Reverse
        {
            currentVelocity -= VelocityAcceleration * Time.deltaTime;
            currentVelocity = Mathf.Max(currentVelocity, MaxReverseVelocity);
        }
        else
        {
            if (currentVelocity != 0) // Decelerate
            {
                float deceleration = Mathf.Min (VelocityDeceleration * Time.deltaTime, Mathf.Abs (currentVelocity));
                currentVelocity -= currentVelocity > 0 ? deceleration : -deceleration;
            }
        }

        if (currentVelocity != 0)
        {
            transform.Translate(Vector3.up * currentVelocity * Time.deltaTime);
        }
    }

    void LaunchProjectile ()
    {
        GameObject instance = Instantiate(Projectile, transform.position, transform.rotation);
        instance.GetComponent<ProjectileController>().Eject(transform.up);
        AudioManager.Instance.PlayShooting();
    }

    void UpdateShooting ()
    {
        if (isShooting)
        {
            projectileDelay -= Time.deltaTime;
            if (projectileDelay <= 0)
            {
                LaunchProjectile();
                projectileDelay = 1.0f / RateOfFire;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Debug.DrawRay(
            transform.position,
            transform.up * currentVelocity / MaxForwardVelocity, // Cap to 1 unit long
            Color.yellow);
    }
}
