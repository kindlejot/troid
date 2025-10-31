using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private void Awake ()
    {
        if (Instance != null && Instance != this) {
            Destroy (gameObject);
        } else {
            Instance = this;
        }
    }

    public AudioSource ShootingSFX;
    public AudioSource ObstacleBreakSFX;
    public AudioSource ShipExplodingSFX;

    public void PlayShooting ()
    {
        ShootingSFX.Play ();
    }

    public void PlayObstacleBreak ()
    {
        ObstacleBreakSFX.Play ();
    }

    public void PlayShipExploding ()
    {
        ShipExplodingSFX.Play ();
    }
}
