using UnityEngine;

public class CameraSingleton : MonoBehaviour
{
    public static CameraSingleton Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) // No camera still exists
        {
            Instance = this; // Become singleton
            DontDestroyOnLoad(gameObject); // Avoid being destroyed on unload
        }
        else  // Instance found, therefore camera already exists
        {
            Destroy(gameObject); // Destroy this copy
        }
    }
}
