using UnityEngine;

public abstract class ObstacleConfig : ScriptableObject
{
    public GameObject Prefab;

    public int Health = 10;
    public int Points = 50;
}
