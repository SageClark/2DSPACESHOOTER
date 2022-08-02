using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private GameObject _speedPrefab;

    private bool _stopSpawning = false;
    private Asteroid _asteroid;
    
    void Start()
    {

        
    }
    
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnSpecialRoutine());
    }

    void Update()
    {
        
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false )
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2f);
        }       
    }   
    
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 5);
            Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnSpecialRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(1f);
            int canSpecialSpawn = Random.Range(0, 6);
            if (canSpecialSpawn >= 3)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
                Instantiate(powerups[5], posToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(5.0f);
            }           
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;      
    }
}
