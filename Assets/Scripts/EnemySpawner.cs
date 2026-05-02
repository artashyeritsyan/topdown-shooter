using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private Transform player;

    [Header("Borders")]
    [SerializeField] private Transform minBorder;
    [SerializeField] private Transform maxBorder;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;

    [Header("Safe Zone Around Player")]
    [SerializeField] private float safeMinX = -20f;
    [SerializeField] private float safeMaxX = 25f;
    [SerializeField] private float safeMinZ = -25f;
    [SerializeField] private float safeMaxZ = 25f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRandomEnemy();
            timer = 0f;
        }
    }

    private void SpawnRandomEnemy()
    {
        int poolIndex = (Random.value <= 0.8f) ? 0 : 1;

        Vector3 spawnPos = GetSpawnPosition();

        enemyPool.SpawnEnemy(poolIndex, spawnPos);
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 playerPos = player.position;

        float minX = minBorder.position.x;
        float maxX = maxBorder.position.x;
        float minZ = minBorder.position.z;
        float maxZ = maxBorder.position.z;

        // Pick a random side of the safe zone
        int side = Random.Range(0, 4); // 0=left,1=right,2=bottom,3=top

        float x = 0f;
        float z = 0f;

        float extraOffset = Random.Range(1f, 5f); // small distance OUTSIDE safe zone

        switch (side)
        {
            case 0: // LEFT of safe zone
                x = playerPos.x + safeMinX - extraOffset;
                z = Random.Range(playerPos.z + safeMinZ, playerPos.z + safeMaxZ);
                break;

            case 1: // RIGHT
                x = playerPos.x + safeMaxX + extraOffset;
                z = Random.Range(playerPos.z + safeMinZ, playerPos.z + safeMaxZ);
                break;

            case 2: // BOTTOM
                z = playerPos.z + safeMinZ - extraOffset;
                x = Random.Range(playerPos.x + safeMinX, playerPos.x + safeMaxX);
                break;

            case 3: // TOP
                z = playerPos.z + safeMaxZ + extraOffset;
                x = Random.Range(playerPos.x + safeMinX, playerPos.x + safeMaxX);
                break;
        }

        // Clamp to borders
        x = Mathf.Clamp(x, minX, maxX);
        z = Mathf.Clamp(z, minZ, maxZ);

        return new Vector3(x, playerPos.y, z);
    }
}