using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance { get; private set; }

    [Header ("Explosion effects")]
    [SerializeField] private GameObject obstacleExplosionFXPrefab;
    [SerializeField] private GameObject shipExplosionFXPrefab;

    [Header ("Hit Effects")]
    [SerializeField] private GameObject hitSparkFXPrefab;

    private const float DEFAULT_LIFETIME = 2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayDestructionFeedback(Vector3 position)
    {
        if (obstacleExplosionFXPrefab != null)
        {
            GameObject fx = Instantiate(obstacleExplosionFXPrefab, position, Quaternion.identity);

            float lifeTime = DEFAULT_LIFETIME;

            if (fx.GetComponent<ParticleSystem>() != null)
            {
                lifeTime = fx.GetComponent<ParticleSystem>().main.duration;
            }

            Destroy(fx, lifeTime);
        }

        AudioManager.Instance.PlayObstacleBreak();
    }

    public void PlayImplosionFeedback(Vector3 position)
    {
        if (shipExplosionFXPrefab != null)
        {
            GameObject fx = Instantiate(shipExplosionFXPrefab, position, Quaternion.identity);

            float lifeTime = DEFAULT_LIFETIME;

            if (fx.GetComponent<ParticleSystem>() != null)
            {
                lifeTime = fx.GetComponent<ParticleSystem>().main.duration;
            }

            Destroy(fx, lifeTime);
        }

        AudioManager.Instance.PlayShipExploding();
    }

    public void PlayHitFeedback(Vector3 position, Quaternion rotation)
    {
        if (hitSparkFXPrefab != null)
        {
            GameObject fx = Instantiate(hitSparkFXPrefab, position, rotation);

            float lifeTime = DEFAULT_LIFETIME;

            if (fx.GetComponent<ParticleSystem>() != null)
            {
                lifeTime = fx.GetComponent<ParticleSystem>().main.duration;
            }

            Destroy(fx, lifeTime);
        }

        AudioManager.Instance.PlayObstacleHit();
    }
}
