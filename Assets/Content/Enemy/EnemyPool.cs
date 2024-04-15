using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public Enemy enemyPrefab = null;
    public int enemyCount = 10;
    [Range(0.0f, 1.0f)]
    public float enemySpawnSize = 0.9f;

    private void Awake()
    {
        for (int i = 0; i < enemyCount; ++i)
        {
            CreateEnemy();
        }
    }

    private void CreateEnemy()
    {
        // Get Orthographic Camera bounds in World Space.
        Vector2 boundsOrigin = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        Vector2 boundsSize = Camera.main.orthographicSize * 2.0f * new Vector2(aspectRatio, 1.0f) * enemySpawnSize;
        Rect bounds = new Rect(boundsOrigin - (boundsSize / 2.0f), boundsSize);

        // Spawn Enemies within bounds.
        Vector3 enemyPosition = bounds.position;
        enemyPosition.x += Random.Range(0.0f, 1.0f) * bounds.width;
        enemyPosition.y += Random.Range(0.0f, 1.0f) * bounds.height;
        Enemy enemy = Instantiate(enemyPrefab);
        enemy.transform.position = enemyPosition;
        enemy.destroyed.AddListener(OnEnemyDestroyed);
    }

    private void OnEnemyDestroyed()
    {
        CreateEnemy();
    }
}
