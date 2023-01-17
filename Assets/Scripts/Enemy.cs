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



    void Start()
    {
        transform.position = new Vector3(Random.Range(-9f,9f), 11, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

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
        CalculateMovement();

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

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.2f)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 11f, 0);
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
            Destroy(this.gameObject, 2.8f);
            _audioSource.Play();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _player.AddScore(10);

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            Destroy(this.gameObject, 2.8f);
            Destroy(GetComponent<Collider2D>());
            _audioSource.Play();
        }

        if (other.tag == "BigShot" )
        {
            
            _player.AddScore(10);

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            Destroy(this.gameObject, 2.8f);
            _audioSource.Play();
        }
            
        
    }
}
