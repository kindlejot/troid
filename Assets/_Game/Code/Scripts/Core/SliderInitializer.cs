using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderInitializer : MonoBehaviour
{
    [SerializeField] private string settingKey = "SettingKey";

    [SerializeField] private TextMeshProUGUI valueDisplay;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        if (slider == null)
        {
            Debug.LogError ($"Slider component is missing from {gameObject.name}");
            return;
        }
        
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(UpdateValueDisplay);

        if (SettingsManager.Instance == null)
        {
            Debug.LogWarning($"SettingsManager not found, the Slider in {gameObject.name} not functional");
            return;
        }

        float savedValue = SettingsManager.Instance.GetSavedSettingValue(settingKey);

        slider.onValueChanged.AddListener((float value) =>
        {
            SettingsManager.Instance.SetSettingValue(settingKey, value);
        });

        slider.value = savedValue;
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
