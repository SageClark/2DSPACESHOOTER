using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    
    [SerializeField]
    private float _enemySpeed = 2.5f;
    
    private float _frequency = 4.0f;
    private float _amplitude = 1.0f;
    private float _cycleSpeed = 4.0f;
    
    private Rigidbody2D _rigidBody;

    private Player _player;

    private Animator _animator;
    
    private AudioSource _audioSource;

    private float _canFire = -1;
    public float _fireRate = 3;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private GameObject _laserPrefab;
    private bool _hitByLaser = false;

    private GameObject[] enemyArray;
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _enemyPrefab;
    private SpawnManager _spawnManager;

    private Vector3 _pos;
    private Vector3 _axis;

    private bool _enemyDestroyed = false;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
        if (_player == null)       
        {
            Debug.LogError("Player is null (enemy)");
        }
        
        
        _animator = GetComponent<Animator>();

        if(_animator == null)
        {
            Debug.LogError("Animator is Null");
        }

        _rigidBody = GetComponent<Rigidbody2D>();

        if(_rigidBody == null)
        {
            Debug.Log("Enemy Rigidbody is NULL");
        }

        _audioSource = GetComponent<AudioSource>();

        if(_audioSource == null)
        {
            Debug.Log("Enemy AudioSource is NULL");
        }

        _pos = transform.position;
        _axis = transform.right;

    }

    // Update is called once per frame
    void Update()
    {
        //CalculateMovement();
        ZigZag();
        if (Time.time > _canFire && _enemyDestroyed == false)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            if (_hitByLaser == false)
            {
                Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            }

        }
        
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.tag == "Player")
        {
            _enemyDestroyed = true;
            if (_player != null)
            {
                _player.Damage();
            }
            else if (_player == null)
            {
                Debug.Log("the damn player is null");
            }

            Destroy(this._rigidBody);
            _animator.SetTrigger("OnEnemyDeath");
            _amplitude = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
            _spawnManager.enemiesDestroyed++;
            
        }

        if (other.tag == "Laser")
        {
            _enemyDestroyed = true;
            Destroy(this._rigidBody);
            Destroy(other.gameObject);
            
            if(_player != null)
            {
                _player.AddPoints(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _amplitude = 0;
            Destroy(this.gameObject, 2.8f);
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            _spawnManager.enemiesDestroyed++;
        }

        if (other.tag == "Energy")
        {
            Debug.Log("missile contacted");
            _enemyDestroyed = true;
            _enemySpeed = 0;
            _animator.SetTrigger("OnEnemyDeath"); 
            _audioSource.Play();
            Destroy(this.gameObject);
            _spawnManager.enemiesDestroyed++;
        }
    }

    private void ZigZag()
    {
        _pos += Vector3.down * Time.deltaTime * _cycleSpeed;
        transform.position = _pos + _axis * Mathf.Sin(Time.time * _frequency) * _amplitude;
        
        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
    }
}
