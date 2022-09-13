using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int _rand;    
    
    private float _enemySpeed = 0f;   
    private float _frequency = 1.5f;
    private float _amplitude = 1.5f;    
    private float _canFire = -1;
    private float _fireRate = 5;
    private float _distance;
    private float _attackRange = 4f;
    private float _ramMultiplier = 2f;
    private float _nowShoot = 0f;

    private bool _enemyDestroyed = false;
    private bool _isShieldActive;
    private bool _hitByLaser = false;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _shield;
    
    private SpawnManager _spawnManager;
    private Player _playerScript;
    private GameObject _playerObject;

    private Rigidbody2D _rigidBody;
    private Collider2D _collider;

    private Animator _animator;

    private AudioSource _audioSource;

    private Vector3 _pos;
    private Vector3 _axis;

    // Start is called before the first frame update
    void Start()
    {
        //NULL check objects
        _playerObject = GameObject.Find("Player");
        if (_playerObject == null)
        {
            Debug.LogError("Player is NULL");
        }

        _playerScript = GameObject.Find("Player").GetComponent<Player>();
        if (_playerScript == null)
        {
            Debug.LogError("Player is NULL");
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.LogError("Animator is NULL");
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

        //Check wave number
        int _wavenumber = _spawnManager.GetWaveNumber();

        if (_wavenumber == 1)
        {
            _enemySpeed = 2f;
        }
        else if (_wavenumber == 2 || _wavenumber == 3)
        {
            _enemySpeed = 3f;
        }
        else if (_wavenumber == 4)
        {
            _enemySpeed = 3.3f;
        }
        
        // Get position for movement
        _pos = transform.position;
        _axis = transform.right;

        // Set shield to active ot inactive at instatiation
        _rand = Random.Range(0, 6);

        if (_rand > 2)
        {
            _shield.SetActive(true);
            _isShieldActive = true;
        }
        else
        {
            _shield.SetActive(false);
            _isShieldActive = false;
        }
    }

    private void FixedUpdate()
    {
        int _layerMask = LayerMask.GetMask("PowerUp");
        Debug.DrawRay(transform.position, -Vector2.up * 10f, Color.red);
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, -Vector2.up, 10f, _layerMask);
        
        if (_hit && Time.time > _nowShoot)
        {
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);

            float _timeToWait = 10;
            _nowShoot = Time.time + _timeToWait;
        }
    }
    
    void Update()
    {
        _distance = Vector3.Distance(_playerObject.transform.position, this.transform.position);

        if(_distance <= _attackRange)
        {
            _playerObject = GameObject.Find("Player");
            transform.position =  Vector3.MoveTowards(transform.position, _playerObject.transform.position, _enemySpeed * _ramMultiplier * Time.deltaTime);
            _pos = transform.position;
            _axis = transform.right;
        }
        
        else
        {
            ZigZag(); // this is the normal movement pattern
        }
        
        if (Time.time > _canFire && _enemyDestroyed == false)
        {
            float fireTime = _fireRate;
            _canFire = Time.time + fireTime;
            
            if (_hitByLaser == false)
            {
                Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            }
        }        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _shield.SetActive(false);
            _playerScript.Damage();
            Destroy(this._collider);
            Destroy(this._rigidBody);
            _enemyDestroyed = true;
            _amplitude = 0;
            _enemySpeed = 0;
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _spawnManager.AddEnemyToDestroyed();
            Debug.Log(_spawnManager.EnemiesDestroyed());
            Destroy(this.gameObject, 2.6f);     
        }

        if (other.tag == "Laser")
        {
            if (_isShieldActive == true)
            {
                _shield.SetActive(false);
                _isShieldActive = false;
                Destroy(other.gameObject);
            }
            
            else if (_isShieldActive == false)
            {
                if (_playerScript != null)
                {
                    _playerScript.AddPoints(10);
                }

                _enemyDestroyed = true;
                _enemySpeed = 0;
                _amplitude = 0;
                Destroy(GetComponent<Collider2D>());
                Destroy(this._rigidBody);
                Destroy(other.gameObject);
                _animator.SetTrigger("OnEnemyDeath");
                _audioSource.Play();
                _spawnManager.AddEnemyToDestroyed();
                Debug.Log(_spawnManager.EnemiesDestroyed());
                Destroy(this.gameObject, 2.8f);
            }
        }

        if (other.tag == "Energy")
        {
            if (_isShieldActive == true)
            {
                _shield.SetActive(false);
                _isShieldActive = false;
                _enemyDestroyed = true;
                _enemySpeed = 0;
                _amplitude = 0;
                Destroy(GetComponent<Collider2D>());
                Destroy(this._rigidBody);
                _animator.SetTrigger("OnEnemyDeath");
                _audioSource.Play();
                _spawnManager.AddEnemyToDestroyed();
                Debug.Log(_spawnManager.EnemiesDestroyed());
                Destroy(this.gameObject, 2.8f);
            }
            
            else if (_isShieldActive == false)
            {
                _enemyDestroyed = true;
                _enemySpeed = 0;
                _amplitude = 0;
                Destroy(GetComponent<Collider2D>());
                Destroy(this._rigidBody);
                _animator.SetTrigger("OnEnemyDeath");
                _audioSource.Play();
                _spawnManager.AddEnemyToDestroyed();
                Debug.Log(_spawnManager.EnemiesDestroyed());
                Destroy(this.gameObject, 2.8f);
            }
        }
    }

    private void ZigZag()
    {
        _pos += Vector3.down * _enemySpeed * Time.deltaTime;
        transform.position = _pos + _axis * Mathf.Sin(Time.time * _frequency) * _amplitude;
        
        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
    }
}
