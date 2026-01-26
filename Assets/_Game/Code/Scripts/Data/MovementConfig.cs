using UnityEngine;

[CreateAssetMenu(fileName = "NewMovement", menuName = "GameData/MovementConfig")]
public class MovementConfig : ScriptableObject
{
    [Header("Linear movement")]
    [Tooltip ("The base movement vector (normalized)")]
    public Vector2 Direction = new Vector2(1, 1).normalized;

    [Tooltip("The constant speed of the object")]
    public float Velocity = 1f;

    [Header("Rotational movement")]
    [Tooltip("If enabled, object will rotate based on animation curve below")]
    public bool IsTurning = false;

    [Tooltip("The Z-axis rotation curve (0 to 1 for time, mapped in degrees)")]
    public AnimationCurve AngleCurve = AnimationCurve.Linear(0, 0, 1, 360);

    [Tooltip("The duration in seconds for one full loop of the Angle Curve.")]
    public float TurningDuration = 1f;

    private void OnValidate()
    {
        if (TurningDuration <= 0) TurningDuration = .1f;
    }
}
