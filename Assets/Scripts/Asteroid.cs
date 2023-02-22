using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = .3f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    void Update()
    {
        transform.Rotate(0f, 0f, _rotationSpeed, Space.Self);
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
   {
       if (collision.tag == "Laser")
       {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            int waveNumber = 1;
            _uiManager.DisplayWaveText(waveNumber);
            _spawnManager.StartSpawning(waveNumber);
            Destroy(this.gameObject, .2f);
       }
    }


}
