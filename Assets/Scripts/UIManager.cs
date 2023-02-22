using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //handle to text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _restartText;
    private GameManager _gameManager;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _reloadText;
    [SerializeField]
    private Slider _thrusterSlider;
    [SerializeField]
    private Text _waveDisplayText;
   


    // Start is called before the first frame update
    void Start()
    {

        _scoreText.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _reloadText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.Log("Game Manager is null");
        }
    }

    void Update()
    {

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {

        _LivesImg.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();
            
        }
    }


    public void UpdateAmmo(int ammoCount)
    {
        _ammoText.text = "Ammo: " + ammoCount.ToString() + " /15";
        if (ammoCount == 0)
        {
            _reloadText.gameObject.SetActive(true);
        } else
        {
            _reloadText.gameObject.SetActive(false);
        }
    }

    public void DisplayWaveText(int waveNumber)
    {
        _waveDisplayText.text = "Wave: " + waveNumber;
        _waveDisplayText.gameObject.SetActive(true);
        StartCoroutine(WaveDisplayRoutine());
    }

    IEnumerator WaveDisplayRoutine()
    {
        while(_waveDisplayText == true)
        {
            yield return new WaitForSeconds(5f);
            _waveDisplayText.gameObject.SetActive(false);
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        StartCoroutine(GameOverFlickerRoutine());
    }

    public void UpdateThrusterSlider(float boostValue)
    {
        _thrusterSlider.value = boostValue;
    }


    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOver.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOver.gameObject.SetActive(false);
            _restartText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
