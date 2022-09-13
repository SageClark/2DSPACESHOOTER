using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float _playerMovementSpeed = 3.5f;

    private int enemiesDestroyed = 0;
    private int _waveCount = 1;

    private bool _stopSpawning = false;
    private bool _shieldActive = false;

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemy2Prefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private GameObject _speedPrefab;
    [SerializeField]
    private GameObject _saucerPrefab;
    [SerializeField]
    private GameObject _cloudPrefab;
    [SerializeField]
    private GameObject _bossHealthBar;
    [SerializeField]
    private GameObject _boss;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private GameObject _playerObject;
    [SerializeField]
    private GameObject _wave1Text;
    [SerializeField]
    private GameObject _wave2Text;
    [SerializeField]
    private GameObject _wave3Text;
    [SerializeField]
    private GameObject _wave4Text;

    void Update()
    {
        CheckEnemyCount();
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnSpecialRoutine());
        StartCoroutine(SpawnAmmoRoutine());
    }

    void CheckEnemyCount()
    {
        if (enemiesDestroyed == 20 && _waveCount == 1)
        {
            _waveCount = 2;
            StartCoroutine(SpawnEnemyRoutineWave2());
            StartCoroutine(SpawnPowerDownRoutine());
        }
        else if (enemiesDestroyed == 40 && _waveCount == 2)
        {
            _waveCount = 3;
            StartCoroutine(SpawnEnemyRoutineWave3());
            StartCoroutine(SpawnSaucerRoutine());
            StartCoroutine(SpawnPowerDownRoutine());
        }
        else if (enemiesDestroyed == 60 && _waveCount == 3)
        {
            _waveCount = 4;
            StartCoroutine(SpawnEnemyRoutineWave4());
        }
    }
    
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);
        _wave1Text.SetActive(true);
        yield return new WaitForSeconds(3f);
        _wave1Text.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
        while (_stopSpawning == false && _waveCount == 1)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2f);
        }
    }   
    
    IEnumerator SpawnEnemyRoutineWave2()
    {
        yield return new WaitForSeconds(1.5f);
        _wave2Text.SetActive(true);
        yield return new WaitForSeconds(3);
        _wave2Text.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        
        while (_stopSpawning == false && _waveCount == 2)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(1.8f);

            Vector3 posToSpawn2 = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy2 = Instantiate(_enemy2Prefab, posToSpawn2, Quaternion.identity);
            newEnemy2.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(6f);
        }
    }

    IEnumerator SpawnEnemyRoutineWave3()
    {
        yield return new WaitForSeconds(1.5f);
        _wave3Text.SetActive(true);
        yield return new WaitForSeconds(3);
        _wave3Text.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        
        while (_stopSpawning == false && _waveCount == 3)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2.8f);

            Vector3 posToSpawn2 = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy2 = Instantiate(_enemy2Prefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator SpawnEnemyRoutineWave4()
    {
        while (_playerObject.transform.position.x != 0.56f && _playerObject.transform.position.y != -1.5625f)
        {
            _playerObject.transform.position = Vector3.MoveTowards(_playerObject.transform.position, new Vector3(0.56f, -1.5625f, 0), _playerMovementSpeed * Time.deltaTime);
        }

        yield return new WaitForSeconds(1.5f);
        _wave4Text.SetActive(true);
        _player.StopPlayerMovement();
        
        yield return new WaitForSeconds(3);
        _wave4Text.SetActive(false);
        _boss.SetActive(true);
        
        yield return new WaitForSeconds(1.5f);
        _bossHealthBar.SetActive(true);
        
        yield return new WaitForSeconds(1.4f);
        _player.StartPlayerMovement();
    }

    IEnumerator SpawnSaucerRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false && _waveCount == 3)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f),Random.Range(1.5f, 6f), 0);
            GameObject newEnemy = Instantiate(_cloudPrefab, posToSpawn, Quaternion.identity);
            
            yield return new WaitForSeconds(1.2f);
            Destroy(newEnemy);
            GameObject newEnemy2 = Instantiate(_saucerPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            newEnemy2.transform.parent = _enemyContainer.transform;
            
            yield return new WaitForSeconds(3f);            
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        float randSec = Random.Range(3, 7);
        yield return new WaitForSeconds(randSec);
        
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 4);
            Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnAmmoRoutine()
    {

        while (_stopSpawning == false)
        {
            float randSec = Random.Range(30, 46);
            yield return new WaitForSeconds(randSec);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            Instantiate(powerups[4], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerDownRoutine()
    {
        float randSec = Random.Range(3, 10);
        yield return new WaitForSeconds(randSec);
        
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            Instantiate(powerups[6], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnSpecialRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(30f);
            
            int canSpecialSpawn = Random.Range(0, 6);
            
            if (canSpecialSpawn >= 3)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                Instantiate(powerups[5], posToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(5.0f);
            }           
        }
    }

    public bool isShieldActive()
    {
        return _shieldActive;
    }
    
    public int GetWaveNumber()
    {
        return _waveCount;
    }

    public int EnemiesDestroyed()
    {
        return enemiesDestroyed;
    }

    public void AddEnemyToDestroyed()
    {
        enemiesDestroyed++;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;      
    }

    public void SetWaveToFirst()
    {
        _waveCount = 1;
    }
}
