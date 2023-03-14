using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject[] _rarePowerups;
    [SerializeField]
    private GameObject[] _frequentPowerup;
    [SerializeField]
    private GameObject[] _enemies;
    [SerializeField]
    private GameObject[] _rareEnemies;
    [SerializeField]
    private bool _stopSpawning = false;
    private int _waveNumber;
    private int _killedEnemies;
    private int _maxEnemies;
    private int _enemiesWaitingToSpawn;
    private UIManager _uiManager;
   
    


    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    public void StartSpawning(int waveNumber)
    {
            if (waveNumber <= 2)
        {
            _stopSpawning = false;
            _killedEnemies = 0;
            _waveNumber = waveNumber;
            _uiManager.DisplayWaveText(_waveNumber);
            _enemiesWaitingToSpawn = _waveNumber + 5;
            _maxEnemies = _waveNumber + 5;
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPowerupRoutine());
            StartCoroutine(SpawnFrequentPowerupRoutine());
        } else if (waveNumber >= 3)
        {
            _stopSpawning = false;
            _killedEnemies = 0;
            _waveNumber = waveNumber;
            _uiManager.DisplayWaveText(_waveNumber);
            _enemiesWaitingToSpawn = _waveNumber + 5;
            _maxEnemies = _waveNumber + 5;
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPowerupRoutine());
            StartCoroutine(SpawnRareEnemyRoutine());
            StartCoroutine(SpawnRarePowerupRoutine());
            StartCoroutine(SpawnFrequentPowerupRoutine());
        }

    }
        
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new  WaitForSeconds(10f);

        while (_stopSpawning == false && _killedEnemies <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomEnemy = Random.Range(0, 2);
            GameObject newEnemy = Instantiate(_enemies[randomEnemy], posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            _enemiesWaitingToSpawn--;
            if(_enemiesWaitingToSpawn == 0)
            {
                _stopSpawning = true;
            }

            yield return new WaitForSeconds(5.0f);
        }
        StartSpawning(_waveNumber + 1);
    }

    IEnumerator SpawnRareEnemyRoutine()
    {
        yield return new WaitForSeconds(10f);

        while (_stopSpawning == false && _killedEnemies <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(-11.5f, 3f, 0f);
            int randomEnemy = Random.Range(0, 2);
            GameObject newRareEnemy = Instantiate(_rareEnemies[randomEnemy], posToSpawn, Quaternion.identity);
            newRareEnemy.transform.parent = _enemyContainer.transform;

            _enemiesWaitingToSpawn--;
            if (_enemiesWaitingToSpawn == 0)
            {
                _stopSpawning = true;
            }

            yield return new WaitForSeconds(10.0f);
        }
        StartSpawning(_waveNumber + 1);
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(5f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 4);
            GameObject newPowerup = Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    IEnumerator SpawnRarePowerupRoutine()
    {
        yield return new WaitForSeconds(10f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 3);
            GameObject newPowerup = Instantiate(_rarePowerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(30f);
        }
    }

    IEnumerator SpawnFrequentPowerupRoutine()
    {
        yield return new WaitForSeconds(5f);

        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newPowerup = Instantiate(_frequentPowerup[0], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(10f);
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void EnemyIsDead()
    {
        _killedEnemies++;
    }
 
}
