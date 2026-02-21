using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawner", menuName = "GameData/SpawnConfig")]
public class SpawnConfig : ScriptableObject
{
    [Tooltip("Object spawn position, if available")]
    public Vector2 Position;

    [Tooltip("Randomize position (overrides Position)")]
    public bool Randomize;
}
