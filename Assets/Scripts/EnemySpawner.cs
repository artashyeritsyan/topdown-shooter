using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private Transform player;

    [Header("Borders")]
    [SerializeField] private Transform minBorder; // bottom-left
    [SerializeField] private Transform maxBorder; // top-right

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

        Vector3 spawnPos = GetSpawnFromBorder();

        enemyPool.SpawnEnemy(poolIndex, spawnPos);
    }

    private Vector3 GetSpawnFromBorder()
    {
        Vector3 playerPos = player.position;

        float minX = minBorder.position.x;
        float maxX = maxBorder.position.x;
        float minZ = minBorder.position.z;
        float maxZ = maxBorder.position.z;

        int side = Random.Range(0, 4); // 0=left, 1=right, 2=bottom, 3=top

        float x = 0f;
        float z = 0f;

        switch (side)
        {
            case 0: // LEFT
                x = minX;
                z = GetValidZ(playerPos, minZ, maxZ);
                break;

            case 1: // RIGHT
                x = maxX;
                z = GetValidZ(playerPos, minZ, maxZ);
                break;

            case 2: // BOTTOM
                z = minZ;
                x = GetValidX(playerPos, minX, maxX);
                break;

            case 3: // TOP
                z = maxZ;
                x = GetValidX(playerPos, minX, maxX);
                break;
        }

        return new Vector3(x, playerPos.y, z);
    }

    private float GetValidX(Vector3 playerPos, float minX, float maxX)
    {
        float forbiddenMin = playerPos.x + safeMinX;
        float forbiddenMax = playerPos.x + safeMaxX;

        return GetOutsideRange(forbiddenMin, forbiddenMax, minX, maxX);
    }

    private float GetValidZ(Vector3 playerPos, float minZ, float maxZ)
    {
        float forbiddenMin = playerPos.z + safeMinZ;
        float forbiddenMax = playerPos.z + safeMaxZ;

        return GetOutsideRange(forbiddenMin, forbiddenMax, minZ, maxZ);
    }

    private float GetOutsideRange(float forbiddenMin, float forbiddenMax, float borderMin, float borderMax)
    {
        bool useLower = Random.value < 0.5f;

        if (useLower && forbiddenMin > borderMin)
            return Random.Range(borderMin, forbiddenMin);

        if (!useLower && forbiddenMax < borderMax)
            return Random.Range(forbiddenMax, borderMax);

        // fallback if one side is invalid
        return Random.Range(borderMin, borderMax);
    }
}