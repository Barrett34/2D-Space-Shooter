using System.Collections;
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
    [SerializeField]
    private GameObject _enemyShield;
    [SerializeField]
    private bool _isEnemyShieldActive = false;
    private int _enemyShieldLevel;
    private int _chanceForEnemyShield;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private float _randomSideMovementRange;
    private SpawnManager _spawnManager;
    [SerializeField]
    private int _enemyID;
    private float _rammingSpeed = 6f;
    private float _targetingDistance = 5f;
    [SerializeField]
    private float _rayDistance = 4.0f;
    [SerializeField]
    private float _rayCastRad = 3f;

    



    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _randomSideMovementRange = Random.Range(-10f, 10f);
        _enemyShield.SetActive(false);
        EnemyShieldChance();

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
                break;
            case 1:
                RamEnemy();
                break;
            case 2:
                BomberEnemy();
                break;
            case 3:
                SmartEnemy();
                break;
            case 4:
                PowerupDestroyEnemy();
                    break;
            case 5:
                DodgingEnemy();
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

    public void BomberEnemy()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);

        if (transform.position.x > 13.3f)
        {
            transform.position = new Vector3(Random.Range(-12f, -11.5f), Random.Range(2f, 3f), 0);
        }

        if (Time.time > _canFire)
        {
            Vector3 offset = new Vector3(.15f, -1f, 0);
            _fireRate = Random.Range(1.4f, 3f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
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

    public void SmartEnemy()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.2f)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 11f, 0);
        }

        RearLaserShotCast();
    }

    public void PowerupDestroyEnemy()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.2f)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 11f, 0);
        }

        DestroyPowerupShot();
    }

    public void DodgingEnemy()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.2f)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 11f, 0);
        }

        DodgeLaser();
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
            _audioSource.Play();
            _spawnManager.EnemyIsDead();
            Destroy(this.gameObject, 1f);
            _speed = 0f;

        }

        if (other.tag == "Laser" && _isEnemyShieldActive == false)
        {
            
            _player.AddScore(10);
            Destroy(other.gameObject);
            _animator.SetTrigger("OnEnemyDeath");
            GetComponent<Collider2D>().enabled = false;
            _audioSource.Play();
            _spawnManager.EnemyIsDead();
            Destroy(this.gameObject, 1f);
            _speed = 0f;

        }

        if (other.tag == "Laser" && _isEnemyShieldActive == true)
        {
            Destroy(other.gameObject);
            _enemyShieldLevel--;
            _enemyShield.SetActive(false);
            _isEnemyShieldActive = false;
            return;
        }



        if (other.tag == "BigShot")
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

    private void EnemyShieldActive()
    {
        _enemyShieldLevel = 1;
        _isEnemyShieldActive = true;
        _enemyShield.SetActive(true);
        Debug.Log("Shield Active is TRUE");
        
    }

    private void EnemyShieldChance()
    {
        _chanceForEnemyShield = Random.Range(0, 3);

        if (_chanceForEnemyShield == 0 && _enemyID == 0)
        {
            EnemyShieldActive();
        } 
    }

    private void RearLaserShotCast()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _rayCastRad, Vector2.up, _rayDistance, LayerMask.GetMask("Player"));

        if(hit.collider != null)
        {
            if (hit.collider.CompareTag("Player") && Time.time > _canFire)
            {
                FireRearLaserShot();
            }
        }
    }

    private void FireRearLaserShot()
    {
        if (Time.time > _canFire)
        {
            Vector3 offset = new Vector3(0f, .75f, 0f);
            _fireRate = Random.Range(1.5f, 3f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + offset, Quaternion.Euler(transform.position.x, transform.position.y, 175.6f));
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            Debug.DrawRay(transform.position, Vector2.up, Color.blue);

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void DestroyPowerupShot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, LayerMask.GetMask("Powerup"));
        Debug.DrawRay(transform.position, Vector3.down * 10f, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Powerup") && Time.time > _canFire)
            {
                Debug.DrawRay(transform.position, Vector2.down * 8f, Color.green);
                FirePowerupShot();
            }
        }
    }

    private void FirePowerupShot()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(.75f, 1f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void DodgeLaser()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 2f, Vector2.down, 10f);
        Debug.DrawRay(transform.position, Vector3.down * 10f, Color.red);


        float moveRandomSide = Random.Range(-1, 4);

        if (hit.collider != null)
        {
            bool _canDodge = true;

            if (hit.collider.CompareTag("Laser") && _canDodge == true)
            {
                transform.position = new Vector3(transform.position.x - moveRandomSide, transform.position.y, transform.position.z);
                _canDodge = false;
            }
        }
    }

}
