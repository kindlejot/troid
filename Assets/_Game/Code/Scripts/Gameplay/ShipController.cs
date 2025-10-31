using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
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
        if (GameManager.Instance.CurrentState == GameState.Pause)
            return;

        UpdateRotation ();
        UpdateAcceleration ();
        UpdateShooting ();
    }

    void UpdateRotation ()
    {
        if (Input.GetAxis ("Horizontal") < 0) { // Rotate counter clockwise
            currentRotationSpeed -= RotationSpeedAcceleration * Time.deltaTime;
            currentRotationSpeed = Mathf.Max (-MaxRotationSpeed, currentRotationSpeed);
        } else if (Input.GetAxis ("Horizontal") > 0) { // Rotate clockwise
            currentRotationSpeed += RotationSpeedAcceleration * Time.deltaTime;
            currentRotationSpeed = Mathf.Min (MaxRotationSpeed, currentRotationSpeed);
        } else {
            currentRotationSpeed = 0;
        }

        if (currentRotationSpeed != 0) {
            transform.Rotate (Vector3.back * Time.deltaTime * currentRotationSpeed);
        }
    }

    void UpdateAcceleration ()
    {
        if (Input.GetAxis("Vertical") > 0) { // Accelerate
            currentVelocity += VelocityAcceleration * Time.deltaTime;
            currentVelocity = Mathf.Min(currentVelocity, MaxForwardVelocity);
        } else if (Input.GetAxis("Vertical") < 0) {  // Reverse
            currentVelocity -= VelocityAcceleration * Time.deltaTime;
            currentVelocity = Mathf.Max(currentVelocity, MaxReverseVelocity);
        } else {
            if (currentVelocity > 0) {
                currentVelocity -= VelocityDeceleration * Time.deltaTime;
                currentVelocity = Mathf.Max (0, currentVelocity);
            }
        }

        if (currentVelocity != 0) {
            transform.Translate (Vector3.up * currentVelocity * Time.deltaTime);
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
        if (Input.GetButtonDown ("Fire1")) {
            LaunchProjectile();
            projectileDelay = 1.0f / RateOfFire;
        }

        if (Input.GetButton("Fire1")) {
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
