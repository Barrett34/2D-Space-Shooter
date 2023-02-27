using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4f;
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private float _randomSideMovementRange;
    private SpawnManager _spawnManager;
    [SerializeField]
    private int _enemyID;
    private float _rammingSpeed = 6f;
    private float _ramEnemySpeed = 2f;
    private float _targetingDistance = 5f;
    



    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        transform.position = new Vector3(Random.Range(-9f,9f), 11, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _randomSideMovementRange = Random.Range(1f, 10f);

        if (_player == null)
        {
            Debug.LogError("The Player is null.");
        }

        if (_animator == null)
        {
            Debug.LogError("Animator is null");
        }
    }

    void Update()
    {
        EnemyClass();

    }

    public void EnemyClass()
    {
        switch(_enemyID)
        {
            case 0:
                LaserEnemy();
                //LaserEnemyMovement();
                break;
            case 1:
                RamEnemy();
                break;
            default:
                break;

        }
    }

    

    public void LaserEnemy()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        transform.position = new Vector3(Mathf.PingPong(Time.time, _randomSideMovementRange), transform.position.y, transform.position.z);

        if (transform.position.y < -5.2f)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 11f, 0);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9f, 9f), transform.position.y, 0);

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

        }
    }

   

    public void RamEnemy()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);


        if (transform.position.y < -5.2f)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 11f, 0);
        }
        else if (Vector3.Distance(_player.transform.position, transform.position) < _targetingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _rammingSpeed * Time.deltaTime);
        }

    }
              

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.transform.GetComponent<Player>();

        if (other.tag == "Player")
        {
           
            if (player != null)
            {
                player.Damage();
            }

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            _spawnManager.EnemyIsDead();
            Destroy(this.gameObject, 1f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (player != null)
            {
                _player.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            GetComponent<Collider2D>().enabled = false;
            _audioSource.Play();
            _spawnManager.EnemyIsDead();
            Destroy(this.gameObject, 1f);
            
        }

        if (other.tag == "BigShot" )
        {

            if (player != null)
            {
                _player.AddScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            GetComponent<Collider2D>().enabled = false;
            _audioSource.Play();
            _spawnManager.EnemyIsDead();
            Destroy(this.gameObject, 1f);
        }
            
        
    }
}
