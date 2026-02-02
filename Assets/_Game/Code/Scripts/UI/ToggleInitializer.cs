using UnityEngine;
using UnityEngine.UI;

public class ToggleInitializer : MonoBehaviour
{
    [SerializeField] private string settingKey = "SettingKey";

    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }

    private void Start()
    {
        if (_toggle == null)
        {
            Debug.LogError($"Toggle component is missing from {gameObject.name}");
            return;
        }

        _toggle.onValueChanged.RemoveAllListeners();

        if (SettingsManager.Instance == null)
        {
            Debug.LogWarning($"SettingsManager not found, the Toggle in {gameObject.name} not functional");
            return;
        }

        float savedValue = SettingsManager.Instance.GetSavedSettingValue(settingKey);

        _toggle.onValueChanged.AddListener((bool value) =>
        {
            SettingsManager.Instance.SetSettingValue(settingKey, value ? 1.0f : 0.0f);
        });

        _toggle.isOn = savedValue > 0.5f;
    }
}
