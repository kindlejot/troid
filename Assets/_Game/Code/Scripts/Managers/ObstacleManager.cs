using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] List<LevelData> Levels;

    public int CurrentLevel;

    List<GameObject> obstacles = new List<GameObject> ();

    public void Reset ()
    {
        CurrentLevel = 0;
        while (obstacles.Count > 0) {
            GameObject obj = obstacles[0];
            Destroy (obj);
            obstacles.RemoveAt (0);
        }
    }

    public void NextLevel ()
    {
        if (Levels != null && Levels.Count > CurrentLevel)
        {
            foreach (ObstacleData obstacle in Levels[CurrentLevel].Obstacles)
            {
                Spawn(obstacle);
            }
        }
        else
        {
            // TODO
        }
        CurrentLevel++;
    }

    Vector2 GetRandomPosition ()
    {
        float ortoHeight = Camera.main.orthographicSize;
        float ortoWidth = Camera.main.orthographicSize * Camera.main.aspect;

        Vector2 result;
        do
        {
            result = new Vector2(Random.Range(-ortoWidth, ortoWidth),
                                 Random.Range(-ortoHeight, ortoHeight));
        } while (!GameManager.Instance.Ship.IsSafeToSpawn(result));

        return result;
    }

    void Spawn(ObstacleData data, bool ignoreSafeSpawn = false)
    {
        if (data.Obstacle == null || data.Obstacle.Prefab == null)
        {
            Debug.LogError("Trying to spawn a null Obstacle");
            return;
        }

        Vector2 spawnPosition = GetRandomPosition();
        if (data.Spawner != null &&
            !data.Spawner.Randomize &&
            (ignoreSafeSpawn || 
             GameManager.Instance.Ship.IsSafeToSpawn(data.Spawner.Position)) )
        {
            spawnPosition = data.Spawner.Position;
        }

        GameObject newObstacle = Instantiate(data.Obstacle.Prefab);
        newObstacle.GetComponent<Obstacle>().OnDestruction += Despawn;
        newObstacle.GetComponent<Obstacle>().Init(spawnPosition, data.Obstacle, data.Movement);
        obstacles.Add(newObstacle);
    }

    void Despawn (GameObject reference)
    {
        Obstacle obstacle = reference.GetComponent<Obstacle>();

        if (obstacle == null)
        {
            Debug.LogWarning("Despawning non-obstacle gameobject");
            return;
        }

        if (obstacle is AsteroidController ac)
        {
            if (ac.CanFragment())
            {
                Spawn(ac.GetFragment(true), true);
                Spawn(ac.GetFragment(false), true);
            }
        }

        GameManager.Instance.AddPoints(obstacle.Points);
        reference.GetComponent<Obstacle>().OnDestruction -= Despawn;
        obstacles.Remove(reference);
        Destroy(reference);

        if (obstacles.Count == 0)
        {
            GameManager.Instance.ChangeState(GameState.NextLevel);
        }
    }
}
