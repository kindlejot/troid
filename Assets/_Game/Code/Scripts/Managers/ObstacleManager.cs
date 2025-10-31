using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObstacleType {
    Asteroid,
    Mine,
    FlyingSaucer
}

public class ObstacleManager : MonoBehaviour
{
    public GameObject ObstacleAsteroid;
    public GameObject ObstacleMine;

    public GameObject ExplosionFX;

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
        CurrentLevel++;
        for (int i=0;i<CurrentLevel;i++) {
            Spawn (ObstacleType.Mine);
        }

        if (CurrentLevel >= 3)
        {
            for (int i=0; i<CurrentLevel/3;i++)
            {
                Spawn (ObstacleType.Asteroid);
            }
        }

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

    Vector2 GetRandomDirection ()
    {
        return Random.insideUnitCircle.normalized;
    }

    void Spawn (ObstacleType type, Vector2? position = null, Vector2? direction = null, int fragmentDepth = 0) 
    {
        switch (type) {
            case ObstacleType.Asteroid: {
                    GameObject newObstacle = Instantiate(ObstacleAsteroid);
                    newObstacle.GetComponent<AsteroidController>().OnDestruction += Despawn;
                    newObstacle.GetComponent<AsteroidController>().Init(position ?? GetRandomPosition(), direction ?? GetRandomDirection());
                    newObstacle.GetComponent<AsteroidController>().FragmentDepth = fragmentDepth;
                    obstacles.Add(newObstacle);
                }
                break;

            case ObstacleType.Mine: {
                    GameObject newObstacle = Instantiate(ObstacleMine);
                    newObstacle.GetComponent<MineController>().OnDestruction += Despawn;
                    newObstacle.GetComponent<MineController>().Init(position ?? GetRandomPosition(), Vector2.zero);
                    obstacles.Add(newObstacle);
                }
                break;

            case ObstacleType.FlyingSaucer:

            default:
                throw new System.NotImplementedException();
                // break;
        }
    }

    void Despawn (GameObject reference)
    {
        Instantiate (ExplosionFX, reference.transform.position, Quaternion.identity);
        AudioManager.Instance.PlayObstacleBreak ();

        if (reference.GetComponent<AsteroidController>() != null) {
            int depth = reference.GetComponent<AsteroidController>().FragmentDepth;

            if (depth < reference.GetComponent<AsteroidController>().MaxFragments) {
                depth++;
                Vector2 position = reference.transform.position;
                Vector3 direction = reference.GetComponent<Obstacle>().Direction;

                Vector3 leftSideDir = Vector3.Cross (direction, Vector3.forward);
                Vector3 rightSideDir = Vector3.Cross (direction, Vector3.back);

                leftSideDir = Quaternion.Euler (0,0,Random.Range (-30,30)) * leftSideDir;
                rightSideDir = Quaternion.Euler (0,0,Random.Range (-30,30)) * rightSideDir;

                Spawn (ObstacleType.Asteroid, position, leftSideDir, depth);
                Spawn (ObstacleType.Asteroid, position, rightSideDir, depth);
            }
        }
        reference.GetComponent<Obstacle>().OnDestruction -= Despawn;
        obstacles.Remove (reference);
        Destroy (reference);

        GameManager.Instance.AddPoints (100);

        if (obstacles.Count==0) {
            GameManager.Instance.ChangeState (GameState.NextLevel);
        }
    }
    
}
