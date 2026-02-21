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
    [SerializeField] private AudioSource obstacleHitSFX;
    [SerializeField] private AudioSource shipExplodingSFX;

    private void Awake ()
    {
        if (Instance != null && Instance != this) {
            Destroy (gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start()
    {
        if (SettingsManager.Instance != null)
        {
            ChangeVolume(SettingsManager.Instance.GetSavedSettingValue(SettingsManager.MASTER_VOL_KEY));
            ChangeSFXVolume(SettingsManager.Instance.GetSavedSettingValue(SettingsManager.SFX_VOL_KEY));
        }
    }

    private void OnEnable()
    {
        SettingsManager.OnSettingChanged += HandleSettingsChanged;
    }

    private void OnDisable()
    {
        SettingsManager.OnSettingChanged -= HandleSettingsChanged;   
    }

    private void HandleSettingsChanged(string key, float value)
    {
        if (key == SettingsManager.MASTER_VOL_KEY)
        {
            ChangeVolume(value);
        }
        
        if (key == SettingsManager.SFX_VOL_KEY)
        {
            ChangeSFXVolume(value);
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

    public void PlayObstacleHit()
    {
        obstacleHitSFX.pitch = Random.Range(0.9f, 1.1f);
        obstacleHitSFX.Play();
    }

    public void PlayShipExploding ()
    {
        shipExplodingSFX.Play ();
    }
}
