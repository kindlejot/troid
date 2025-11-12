using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public int Damage = 10;
    public float Velocity = 15;
    public float ExpirationTime = 1;

    private Vector3 _direction;
    private float _expirationTimer;
    private bool _ejected = false;

    public void Eject (Vector3 direction)
    {
        this._direction = direction;
        _ejected = true;
        _expirationTimer = ExpirationTime;
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
        if (_ejected) {
            transform.position += _direction * Velocity * Time.deltaTime;
            _expirationTimer -= Time.deltaTime;
            if (_expirationTimer <= 0) {
                Destroy (gameObject);
            }
        }
    }
}
