using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saucer : MonoBehaviour
{
    private float _canFire = -1;
    private float _fireRate = 1;

    private bool _enemyDestroyed = false;
   
    private SpawnManager _spawnManager;
    private Rigidbody2D _rigidBody;
    private Player _player;
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private GameObject _swooshPrefab;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SaucerDestroyRoutine());

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager is NULL");
        }
        
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _rigidBody = GetComponent<Rigidbody2D>();
        if (_rigidBody == null)
        {
            Debug.Log("Enemy Rigidbody is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.Log("Enemy AudioSource is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {       
        if (Time.time > _canFire && _enemyDestroyed == false)
        {
            Instantiate(_swooshPrefab, transform.position, Quaternion.identity);
            _canFire = Time.time + _fireRate;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            _enemyDestroyed = true;
            Destroy(this._rigidBody);
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);
            _audioSource.Play();
            _spawnManager.AddEnemyToDestroyed();
            Debug.Log(_spawnManager.EnemiesDestroyed());

            if (_player != null)
            {
                _player.AddPoints(30);
                _player.Damage();
            }
            
            else if (_player == null)
            {
                Debug.Log("the damn player is null");
            }

            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            _enemyDestroyed = true;
            Destroy(this._rigidBody);
            Destroy(other.gameObject);
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<Collider2D>());
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _audioSource.Play();
            _spawnManager.AddEnemyToDestroyed();
            
            if (_player != null)
            {
                _player.AddPoints(30);
            }
            
            Destroy(this.gameObject);
        }

        if (other.tag == "Energy")
        {
            _enemyDestroyed = true;
            Destroy(this._rigidBody);
            Destroy(other.gameObject);
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<Collider2D>());
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _audioSource.Play();
            _spawnManager.AddEnemyToDestroyed();

            if (_player != null)
            {
                _player.AddPoints(30);
            }

            Destroy(this.gameObject);
        }
    }

    IEnumerator SaucerDestroyRoutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
