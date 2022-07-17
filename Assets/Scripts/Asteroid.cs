using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 3.0f;
    [SerializeField]
    public GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private Scroller _scroller;


    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _scroller     = GameObject.Find("Plane").GetComponent<Scroller>();    
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {          
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.5f);
            _spawnManager.StartSpawning();
            Destroy(other.gameObject, 0.5f);
            _scroller.enabled = true;
        }
    }         
}
