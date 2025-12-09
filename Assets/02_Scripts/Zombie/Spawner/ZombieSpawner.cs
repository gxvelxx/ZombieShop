using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Spawn Setting")]
    public float _spawnInterval = 3f;
    public int _worldMaxZombie = 120;

    private int _worldCurrentCount = 0;

    [Header("Spawn Area")]
    public SpawnPoint[] _spawnPoints;

    [Header("Player Avoid Setting")]
    public Transform _player;
    public float _safeGap = 10f;    

    /// <summary>
    /// 게임매니저가 스포너 초기화
    /// </summary>
    public void InitializeSpawner()
    {        
        foreach (var spawnPoint in _spawnPoints)
        {
            if (spawnPoint != null && spawnPoint.Data != null)
                spawnPoint.Data._currentCount = 0;
        }

        _worldCurrentCount = 0;

        SpawnInitiate();
        StartCoroutine(SpawnRoutine());
    }

    private void SpawnInitiate()
    {
        for (int i = 0; i < _worldMaxZombie; i++)
        {
            TrySpawnZombie();
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (GameManager.Instance.Playing)
        {
            yield return new WaitForSeconds(_spawnInterval);

            if (_worldCurrentCount < _worldMaxZombie)
                TrySpawnZombie();
        }
    }

    /// <summary>
    /// NavMesh 확인 후 스폰처리
    /// </summary>
    private void TrySpawnZombie()
    {
        if (_spawnPoints.Length == 0)
        {
            Debug.Log("No Area");
            return;
        }

        SpawnPoint area = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        SpawnPointData data = area.Data;
        
        if (data._currentCount >= data._maxCount) // 해당 포인트가 꽉 차있으면 패스
            return;

        const int maxAttempts = 10; // Nav체크시도

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 randomPos = GetRandomNavPosition(area.transform.position, data._radius);

            if (randomPos == Vector3.zero)
                continue; // Nav 위가 아니면 다시

            if (Vector3.Distance(randomPos, _player.position) < _safeGap)
                continue; // 가까우면 다시

            Spawn(randomPos, data);
            return;
        }

        Debug.Log("Not Found NavArea");
    }

    private void Spawn(Vector3 pos, SpawnPointData data)
    {
        GameObject zombie = ZombiePool.Instance.SpawnZombie(pos);
        _worldCurrentCount++;
        data._currentCount++;

        ZombieController controller = zombie.GetComponent<ZombieController>();
        controller.InitializeAtSpawn(pos);

        controller.OnDeadCallback = () =>
        {            
            _worldCurrentCount--;
            data._currentCount--;
        };
    }

    private Vector3 GetRandomNavPosition(Vector3 center, float radius)
    {
        Vector2 circle = Random.insideUnitCircle * radius;

        Vector3 randomPos = new Vector3(
            center.x + circle.x, center.y, center.z + circle.y);

        NavMeshHit hit;

        //실패시 스폰할 위치 주변 탐색
        if (NavMesh.SamplePosition(randomPos, out hit, 3f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero; // Nav 위 아니면 실패
    }

    private void OnDrawGizmos()
    {
        if (_spawnPoints == null)
            return;

        Gizmos.color = Color.cyan;

        foreach (var point in _spawnPoints)
        {
            if (point == null) continue;
            if (point.Data == null) continue;

            // 스폰 반경을 원으로 표시
            Gizmos.DrawWireSphere(point.transform.position, point.Data._radius);
        }
    }
}
