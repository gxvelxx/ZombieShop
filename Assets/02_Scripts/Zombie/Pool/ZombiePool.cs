using UnityEngine;
using UnityEngine.Pool;

public class ZombiePool : MonoBehaviour
{
    public static ZombiePool Instance;

    public GameObject _zombiePrefab;
    private ObjectPool<GameObject> _pool;



    private void Awake()
    {
        Instance = this;

        _pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                //좀비 생성
                var obj = Instantiate(_zombiePrefab);
                obj.transform.SetParent(this.transform);
                obj.SetActive(false);

                obj.GetComponent<ZombieController>().SetPool(_pool);
                return obj;
            },
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj)
        );
    }    

    public GameObject SpawnZombie(Vector3 position)
    {
        GameObject zombie = _pool.Get();
        zombie.transform.position = position;
        zombie.transform.rotation = Quaternion.identity;
        return zombie;
    }
}
