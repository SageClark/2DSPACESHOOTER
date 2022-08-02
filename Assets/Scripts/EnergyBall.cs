using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    
    private float _speed = 5;

    private GameObject _enemy;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeLimit());
    }

    // Update is called once per frame
    void Update()
    {
        _enemy = GameObject.FindWithTag("Enemy");
        transform.position = Vector3.MoveTowards(transform.position, _enemy.transform.position, _speed * Time.deltaTime);
        
    }

    IEnumerator TimeLimit()
    {
        yield return new WaitForSeconds(20);
        Destroy(this.gameObject);
    }
}
