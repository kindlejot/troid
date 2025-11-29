using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private AudioSource shootingSFX;
    [SerializeField] private AudioSource obstacleBreakSFX;
    [SerializeField] private AudioSource shipExplodingSFX;

    private void Awake ()
    {
        if (Instance != null && Instance != this) {
            Destroy (gameObject);
        } else {
            Instance = this;
        }
    }

    public void ChangeVolume (float value)
    {
        audioMixer.SetFloat("VolumeMaster", Mathf.Log10(Mathf.Max(value, .01f)) * 20);
    }

    public void ChangeSFXVolume(float value)
    {
        audioMixer.SetFloat("VolumeSFX", Mathf.Log10(Mathf.Max(value, .01f)) * 20);
    }

    public void PlayShooting ()
    {
        shootingSFX.Play ();
    }

    public void PlayObstacleBreak ()
    {
        obstacleBreakSFX.Play ();
    }

    public void PlayShipExploding ()
    {
        shipExplodingSFX.Play ();
    }
}
