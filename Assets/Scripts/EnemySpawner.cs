using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnDistance = 10f;

    private Transform player;
    private float timer;

    [SerializeField] private float initialSpawnRate = 1.5f;
    [SerializeField] private float minimumSpawnRate = 0.4f;
    [SerializeField] private float difficultyRamp = 0.02f;

    private float currentSpawnRate;
    private float gameTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentSpawnRate = initialSpawnRate;
    }

    private void Update()
    {
        gameTime += Time.deltaTime;

        // Reducimos el spawn rate con el tiempo
        currentSpawnRate = Mathf.Max(minimumSpawnRate,
                                     initialSpawnRate - gameTime * difficultyRamp);

        timer += Time.deltaTime;

        if (timer >= currentSpawnRate)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 spawnPosition = (Vector2)player.position + randomDirection * spawnDistance;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}