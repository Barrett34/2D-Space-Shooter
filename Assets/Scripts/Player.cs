using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private int _shieldLevel;
    [SerializeField]
    private Color _shieldColor;
    [SerializeField]
    private SpriteRenderer _shieldRenderer;
    [SerializeField]
    private int _ammoCount = 15;
    



    private UIManager _uiManager;

    private Color _green = new Color(255, 186, 0);
    private Color _red = new Color(255, 4, 4);

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is null");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        _uiManager.UpdateAmmo(_ammoCount);
    }

    void Update()
    {
        CalculateMovement();
        

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
        

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed =  7f;
        }
        else
        {
            _speed = 3.5f;
        }
    }

    void FireLaser()
    {
        if (_ammoCount > 0)
        {
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);

            Vector3 offset = new Vector3(0, 1.05f, 0);
            _canFire = Time.time + _fireRate;

            if (Input.GetKey(KeyCode.Space) && _isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            }

            _audioSource.Play();
        }
        
    }

    public void Damage()
    {
        

        if (_isShieldActive == true)
        {
            _shieldLevel--;

            if (_shieldLevel == 2)
            {
                _shieldColor.a = .55f;
                _shieldRenderer.color = _shieldColor;
                return;
            }
            else if (_shieldLevel == 1)
            {
                _shieldColor.a = .25f;
                _shieldRenderer.color = _shieldColor;
                return;
            }
            else if (_shieldLevel <= 0)
            {
                _isShieldActive = false;
                _shield.SetActive(false);
                return;
            }
        }

        _lives--;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);


        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed = _speed * _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ShieldActive()
    {
        _shieldLevel = 3;
        _shieldColor.a = 1f;
        _shieldRenderer.color = _shieldColor;
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    public void AmmoReload()
    {
        _uiManager.UpdateAmmo(15);
        _ammoCount = 15;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void AddPlayerLife()
    {
        if (_lives == 3)
        {
            return;
        } else if (_lives == 2)
        {
            _lives += 1;
            _rightEngine.SetActive(false);
            _uiManager.UpdateLives(_lives);
        } else if (_lives == 1)
        {
            _lives += 1;
            _leftEngine.SetActive(false);
            _uiManager.UpdateLives(_lives);
        }
    }

    IEnumerator TripleShotPowerDownRoutine()
    { 
            yield return new WaitForSeconds(5.0f);
            _isTripleShotActive = false;
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

   

}
