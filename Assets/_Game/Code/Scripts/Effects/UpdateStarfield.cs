using UnityEngine;

public class UpdateStarfield : MonoBehaviour
{
    [SerializeField] private Material starfieldMaterial;

    [SerializeField] private BorderGuard shipBorderGuard;

    private bool _shaderFound = false;

    void Start()
    {
        if (starfieldMaterial == null || !starfieldMaterial.shader.name.Contains("Starfield"))
        {
            Debug.LogError("Material not found or the material doesn't contain right shader");
            return;
        }
        _shaderFound = true;
    }

    void Update()
    {
        if (_shaderFound)
        {
            starfieldMaterial.SetVector("_Position", shipBorderGuard.ObjectAccumulatedPosition);
        }
    }
}
