using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRemoval : MonoBehaviour
{
    private float _selfRemovalTimer;

    void Awake ()
    {
        _selfRemovalTimer = GetComponent <ParticleSystem> ().main.duration;
    }

    void Update () 
    {
        _selfRemovalTimer -= Time.deltaTime;
        if (_selfRemovalTimer <= 0) {
            Destroy (gameObject);
        }
    }
}
