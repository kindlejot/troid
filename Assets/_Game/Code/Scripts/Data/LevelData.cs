using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct ObstacleData
{
    public ObstacleConfig Obstacle;
    public SpawnConfig Spawner;
    public MovementConfig Movement;
}

[CreateAssetMenu(fileName = "NewLevel", menuName = "GameData/LevelData")]
public class LevelData : ScriptableObject
{
    public List<ObstacleData> Obstacles;
}
