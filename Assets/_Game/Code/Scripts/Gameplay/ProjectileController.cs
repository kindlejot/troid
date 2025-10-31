using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public int Damage = 10;
    public float Velocity = 15;
    public float ExpirationTime = 1;

    Vector3 direction;
    float expirationTimer;
    bool ejected = false;

    public void Eject (Vector3 direction)
    {
        this.direction = direction;
        ejected = true;
        expirationTimer = ExpirationTime;
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.GetComponent<Obstacle>() != null) {
            other.gameObject.GetComponent<Obstacle>().Hit (Damage);
            Destroy (gameObject);
        }
    }

    void Update ()
    {
        if (ejected) {
            transform.position += direction * Velocity * Time.deltaTime;
            expirationTimer -= Time.deltaTime;
            if (expirationTimer <= 0) {
                Destroy (gameObject);
            }
        }
    }

}
