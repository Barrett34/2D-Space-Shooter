using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = .3f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, _rotationSpeed, Space.Self);
    }

    //check for laser collision (Trigger)
    // instantiate explosion at the position of the asteroid (us)
    // destroy the explosion after 3 secs

    private void OnTriggerEnter2D(Collider2D collision)
   {
       if (collision.tag == "Laser")
       {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, .2f);
       }
    }


}
