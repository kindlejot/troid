using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRemoval : MonoBehaviour
{
    float selfRemovalTimer;

    void Awake ()
    {
        selfRemovalTimer = GetComponent <ParticleSystem> ().main.duration;
    }

    void Update () 
    {
        selfRemovalTimer -= Time.deltaTime;
        if (selfRemovalTimer <= 0) {
            Destroy (gameObject);
        }
    }
}
