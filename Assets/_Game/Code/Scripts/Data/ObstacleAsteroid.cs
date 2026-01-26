using UnityEngine;

[CreateAssetMenu(fileName = "NewAsteroid", menuName = "GameData/Obstacles/ObstacleAsteroid")]
public class ObstacleAsteroid : ObstacleConfig
{
    public int Fragments = 3;
    public float RelativeSize = 1.0f;
}
