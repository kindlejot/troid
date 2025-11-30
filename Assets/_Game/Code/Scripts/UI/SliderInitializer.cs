using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderInitializer : MonoBehaviour
{
    [SerializeField] private string settingKey = "SettingKey";

    [SerializeField] private TextMeshProUGUI valueDisplay;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        if (_slider == null)
        {
            Debug.LogError ($"Slider component is missing from {gameObject.name}");
            return;
        }
        
        _slider.onValueChanged.RemoveAllListeners();
        _slider.onValueChanged.AddListener(UpdateValueDisplay);

        if (SettingsManager.Instance == null)
        {
            Debug.LogWarning($"SettingsManager not found, the Slider in {gameObject.name} not functional");
            return;
        }

        float savedValue = SettingsManager.Instance.GetSavedSettingValue(settingKey);

        _slider.onValueChanged.AddListener((float value) =>
        {
            SettingsManager.Instance.SetSettingValue(settingKey, value);
        });

        _slider.value = savedValue;
    }

    private void UpdateValueDisplay (float value)
    {
        if (valueDisplay != null)
        {
            int percentage = Mathf.RoundToInt(value * 100);
            valueDisplay.text = $"{percentage}%";
        }
    }
}
