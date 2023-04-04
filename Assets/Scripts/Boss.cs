using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    private bool _canStrafe;
    private int _strafeDirection = 1;
    private float _canFire = -1f;
    private float _fireRate = 3f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _laserSpreadShotPrefab;
    private Player _player;
    [SerializeField]
    private int _bossHealthPoints = 100;





    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        transform.position = new Vector3(0, 9f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        {
            if (_canStrafe == true)
            {
                Strafe();
            }
            else
            {
                transform.Translate(Vector3.down * _speed / 3 * Time.deltaTime);
            }
            if (transform.position.y <= 2f)
            {
                _canStrafe = true;
            }
        }
        FireTripleLaserShot();
        FireSpreadLaserShot();
        DestroyBoss();
    }

    public void FireTripleLaserShot()
    {
        if (Time.time > _canFire)
        {
            Vector3 offset = new Vector3(0, -2.0f, 0);
            _fireRate = Random.Range(3f, 4f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

        }
        Debug.Log("Triple Shot Fired");
    }

    public void FireSpreadLaserShot()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(.75f, 1.25f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserSpreadShotPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

        }
        Debug.Log("Spread Shot Fired");
    }

    

    public void Strafe()
    {
        if (_canStrafe == true)
        {

            if (transform.position.x <= -8.5f)
            {
                _strafeDirection = 1;
            }
            else if (transform.position.x >= 8.5f)
            {
                _strafeDirection = -1;
            }
            transform.Translate(Vector3.right * _speed * _strafeDirection * Time.deltaTime);

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
        }

        if (other.tag == "Laser")
        {
            _player.AddScore(10);
            Destroy(other.gameObject);
            _bossHealthPoints--;
        }

        if(other.tag == "Laser" && _player._isHomingMissleActive == true)
        {
            {
                _player.AddScore(10);
                Destroy(other.gameObject);
                _bossHealthPoints = _bossHealthPoints - 5;
            }
        }

        if (other.tag == "BigShot")
        {

            if (player != null)
            {
                _player.AddScore(10);
                _bossHealthPoints = _bossHealthPoints - 10;
            }
        }

    }

    public void DestroyBoss()
    {
        if (_bossHealthPoints <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
