using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{

    private float _speed = 5;

    private GameObject _enemy;
    private SpawnManager _spawnManager;

    void Start()
    {
        StartCoroutine(TimeLimit());
    }

    void Update()
    {       
        _enemy = GameObject.FindWithTag("Enemy");
        
        if (_enemy != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _enemy.transform.position, _speed * Time.deltaTime);
        }
        else if (_enemy == null)
        {
            Debug.Log("Enemy is NULL");
        }
    }
    IEnumerator TimeLimit()
    {
        yield return new WaitForSeconds(20);
        Destroy(this.gameObject);
    }
}
