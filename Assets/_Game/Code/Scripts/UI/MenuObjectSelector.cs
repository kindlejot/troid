using UnityEngine;
using UnityEngine.EventSystems;

public class MenuObjectSelector : MonoBehaviour
{
    private EventSystem _eventSystem;

    private void Start()
    {
        _eventSystem = GetComponent<EventSystem>();
        if (_eventSystem ==  null)
        {
            Debug.LogError("MenuObjectSelector should be assigned to EventSystem, such component not found!");
        }
    }

    void Update()
    {
        if (_eventSystem.currentSelectedGameObject == null || !_eventSystem.currentSelectedGameObject.activeInHierarchy)
        {
            _eventSystem.SetSelectedGameObject(MenuManager.Instance.GetFirstSelectableGameObject());
        }
    }
}
