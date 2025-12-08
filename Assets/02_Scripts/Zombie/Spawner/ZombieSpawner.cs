using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn Setting")]
    public float _spwanInterval = 3f;
    public int _maxZombieCount = 50;

    private int _currentCount = 0;

    [Header("Spawn Area")]
    public Transform[] _spawnPoints;
    public float _spawnRadius = 50f;

    [Header("Player Avoid Setting")]
    public Transform _player;
    public float _safeGap = 10f;

    private void Start()
    {
        SpawnInitiate();
        StartCoroutine(SpawnRoutine());
    }

    private void SpawnInitiate()
    {
        for (int i = 0; i < _maxZombieCount; i++)
        {
            TrySpawnZombie();
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spwanInterval);

            if (_currentCount >= _maxZombieCount)
                continue;

            TrySpawnZombie();
        }
    }

    private void TrySpawnZombie()
    {
        if (_spawnPoints.Length == 0)
        {
            Debug.Log("No Area");
            return;
        }

        Transform area = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        const int maxAttempts = 10; // Nav체크시도

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 randomPos = GetRandomNavPosition(area.position);

            if (randomPos == Vector3.zero)
                continue; // Nav 위가 아니면 다시

            if (Vector3.Distance(randomPos, _player.position) < _safeGap)
                continue; // 가까우면 다시

            Spawn(randomPos);
            return;
        }

        Debug.Log("Not Found NavArea");
    }

    private void Spawn(Vector3 pos)
    {
        GameObject zombie = ZombiePool.Instance.SpawnZombie(pos);
        _currentCount++;

        zombie.GetComponent<ZombieController>().OnDeadCallback = () =>
        {
            _currentCount--;
        };
    }

    private Vector3 GetRandomNavPosition(Vector3 center)
    {
        Vector2 circle = Random.insideUnitCircle * _spawnRadius;

        Vector3 randomPos = new Vector3(
            center.x + circle.x, center.y, center.z + circle.y);

        NavMeshHit hit;

        //실패시 그 주변 탐색
        if (NavMesh.SamplePosition(randomPos, out hit, 3f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero; // Nav 위 아니면 실패
    }
}
