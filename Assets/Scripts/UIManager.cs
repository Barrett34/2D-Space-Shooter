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


    // Start is called before the first frame update
    void Start()
    {

        _scoreText.text = "Score: " + 0;
        _gameOver.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    void Update()
    {
        RestartGame();
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
            StartCoroutine(GameOverFlickerRoutine());
            
        }
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

    public void RestartGame()
    {
        if (Input.GetKey("r"))
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
