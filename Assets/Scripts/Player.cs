using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2.5f;
    private float _speedDecreaser = 1.0f;
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
    [SerializeField]
    private GameObject _homingMisslePrefab;
    private bool _isSpeedBoostActive = false;
    private bool _isBigShotActive = false;
    private bool _isShieldActive = false;
    private bool _isSpeedDecreaserActive = false;
    public bool _isHomingMissleActive = false;
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
    [SerializeField]
    private GameObject _bigShot;
    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private float _boostValue = 100f;
    [SerializeField]
    private float _reboostSpeed;
    private bool isThrusterActive;
    [SerializeField]
    private Camera _camera;


    



    private UIManager _uiManager;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();

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

        if (_camera == null)
        {
            Debug.LogError("The Camera is null");
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

        if (Input.GetKey(KeyCode.LeftShift) && _boostValue <= 0)
        {
            StopCoroutine(ActivateBoostRefuel());
            _thruster.SetActive(false);
            _speed = 3.5f;
        }

        if (Input.GetKey(KeyCode.LeftShift) && _boostValue > 0)
        {
            if (_isSpeedBoostActive)
            {
                StopCoroutine(ActivateBoostRefuel());
                ActivateThruster();
                _speed = 10f;
            }
            else if (_isSpeedDecreaserActive)
            {
                _thruster.SetActive(false);
                _speed = _speedDecreaser;
            }
            else
            {
                StopCoroutine(ActivateBoostRefuel());
                ActivateThruster();
                _speed = 8f;
            }
        
        } 
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isThrusterActive = false;

            if (_isSpeedBoostActive)
            {
                _thruster.SetActive(false);
                StartCoroutine(ActivateBoostRefuel());
            }
            else if (_isSpeedDecreaserActive)
            {
                _thruster.SetActive(false);
                _speed = _speedDecreaser;
            }
            else
            {
                _thruster.SetActive(false);
                StartCoroutine(ActivateBoostRefuel());
                _speed = 3.5f;
            }
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
            else if (Input.GetKey(KeyCode.Space) && _isHomingMissleActive == true)
            {
                Instantiate(_homingMisslePrefab, transform.position, Quaternion.identity);
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
            _camera.ShakeCamera();
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
            _camera.ShakeCamera();
        }

        _uiManager.UpdateLives(_lives);


        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _camera.ShakeCamera();
            Destroy(this.gameObject);
            
        }
    }

    public void HomingMissleActive()
    {
        _isHomingMissleActive = true;
        StartCoroutine(HomingMisslePowerDownRoutine());

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

    public void SpeedDecreaserActive()
    {
        _isSpeedDecreaserActive = true;
        _speed = _speedDecreaser;
        StartCoroutine(SpeedDecreaserRoutine());
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

    public void BigShotActive()
    {
        _isBigShotActive = true;
        _bigShot.SetActive(true);
        StartCoroutine(BigShotPowerDownRoutine());
    }

    public void ActivateThruster()
    {
        isThrusterActive = true;

        if(_boostValue > 0)
        {
            _thruster.SetActive(true);
            _boostValue -= 15 * 2 * Time.deltaTime;
            _uiManager.UpdateThrusterSlider(_boostValue);
        }
        else if (_boostValue <= 0)
        {
            _thruster.SetActive(false);
            _boostValue = 0.0f;
            _uiManager.UpdateThrusterSlider(_boostValue);
        }
    }

    IEnumerator ActivateBoostRefuel()
    {
        while(_boostValue != 100 && isThrusterActive == false)
        {
            yield return new WaitForSeconds(.1f);
            _boostValue += 30 * _reboostSpeed * Time.deltaTime;
            _uiManager.UpdateThrusterSlider(_boostValue);

            if(_boostValue >= 100)
            {
                _boostValue = 100;

                _uiManager.UpdateThrusterSlider(_boostValue);

                break;
            }

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

    IEnumerator SpeedDecreaserRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedDecreaserActive = false;
        _speed = 3.5f;
    }

    IEnumerator BigShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        _isBigShotActive = false;
        _bigShot.SetActive(false);
       
    }

    IEnumerator HomingMisslePowerDownRoutine()
    {
        yield return new WaitForSeconds(45f);
        _isHomingMissleActive = false;
    }
   

}
