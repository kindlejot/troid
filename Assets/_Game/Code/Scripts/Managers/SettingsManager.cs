using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public static Action<string, float> OnSettingChanged;

    public const string MASTER_VOL_KEY = "VolumeMaster";
    public const string SFX_VOL_KEY    = "VolumeSFX";
    public const string AUTO_STEER_KEY = "AutoSteer";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSettings ()
    {
        SetSettingValue (MASTER_VOL_KEY, GetSavedSettingValue (MASTER_VOL_KEY));
        SetSettingValue (SFX_VOL_KEY, GetSavedSettingValue(SFX_VOL_KEY));
        SetSettingValue (AUTO_STEER_KEY, GetSavedSettingValue(AUTO_STEER_KEY));
    }

    public void SetSettingValue (string key, float value)
    {
        switch (key)
        {
            case MASTER_VOL_KEY:
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.ChangeVolume(value);
                }
                break;

            case SFX_VOL_KEY:
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.ChangeSFXVolume(value);
                }
                break;

            case AUTO_STEER_KEY:
                break;

            // FUTURE: Add new setting types here
            default:
                Debug.LogWarning($"Attempted to set unknown key: {key}");
                return;
        }
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();

        OnSettingChanged?.Invoke(key, value);
    }

    public float GetSavedSettingValue (string key)
    {
        return PlayerPrefs.GetFloat(key, .75f);
    }
}
