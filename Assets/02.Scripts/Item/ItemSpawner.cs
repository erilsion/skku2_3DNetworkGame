using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;

    [SerializeField] private float _minSpawnPosition = -30f;
    [SerializeField] private float _maxSpawnPosition = 30f;
    [SerializeField] private float _ySpawnPosition = 8f;

    [SerializeField] private float _itemRespawnCooltime = 5f;
    private float _itemRespawnTimer = 0f;

    private void Awake()
    {
        Instance = this;
        _itemRespawnTimer = 0f;
    }

    private void Update()
    {
        _itemRespawnTimer += Time.deltaTime;
        if (_itemRespawnTimer >= _itemRespawnCooltime)
        {
            ItemSpawn();
            _itemRespawnTimer = 0f;
        }
    }

    public void ItemSpawn()
    {
        transform.position = GetRandomSpawnPoint();
        ItemObjectFactory.Instance.RequestMakeDropScoreItem(transform.position);
    }

    public Vector3 GetRandomSpawnPoint()
    {
        float xSpawnPosition = Random.Range(_minSpawnPosition, _maxSpawnPosition);
        float zSpawnPosition = Random.Range(_minSpawnPosition, _maxSpawnPosition);
        Vector3 spawnPoint = new Vector3(xSpawnPosition, _ySpawnPosition, zSpawnPosition);
        return spawnPoint;
    }
}
