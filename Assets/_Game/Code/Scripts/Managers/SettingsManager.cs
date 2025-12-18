using UnityEditor.Search;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    const string MASTER_VOL_KEY = "VolumeMaster";
    const string SFX_VOL_KEY    = "VolumeSFX";

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

            // FUTURE: Add new setting types here
            default:
                Debug.LogWarning($"Attempted to set unknown key: {key}");
                return;
        }
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    public float GetSavedSettingValue (string key)
    {
        return PlayerPrefs.GetFloat(key, .75f);
    }
}
