using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    private float _enemySpeed = 1.5f;
    private float _frequency = 4.0f;
    private float _amplitude = 1.0f;
    private float _canFire = -1f;
    private float _fireRate = 2f;

    private bool _hitByLaser = false;
    private bool _enemyDestroyed = false;

    private Vector3 _pos;
    private Vector3 _axis;

    private Rigidbody2D _rigidBody;
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    private Transform _enemyTrans;
    private SpawnManager _spawnManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private GameObject _laserPrefab;    
    [SerializeField]
    private GameObject _playerLaser;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Sprite _reverseSprite;
    [SerializeField]
    private GameObject _reverseLaserPrefab;
    [SerializeField]
    private Sprite _forwardSprite;

    void Start()
    {
        _pos = transform.position;
        _axis = transform.right;

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("Player is NULL");
        }
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager is NULL");
        }

        _enemyTrans = GetComponent<Transform>();
        if (_enemyTrans == null)
        {
            Debug.Log("Enemy Transform is NULL");
        }

        _forwardSprite = GetComponent<SpriteRenderer>().sprite;
        if (_forwardSprite == null)
        {
            Debug.Log("Enemy 2 forward Sprite is NULL");
        }
        
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is Null");
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

    private void FixedUpdate()
    {
        int _layerMask = LayerMask.GetMask("Laser");
        Debug.DrawRay(transform.position + new Vector3(-1, -2, 0), Vector3.right * 1.8f, Color.red);
        RaycastHit2D _hit = Physics2D.Raycast(transform.position + new Vector3(-1, -2, 0), Vector3.right, 1.8f, _layerMask);
        if (_hit)
        {
            int rand = Random.Range(0, 3);
            
            if (rand == 0)
            {
                int rand2 = Random.Range(0, 2);
                
                if (rand2 == 1)
                {
                    transform.position = transform.position + new Vector3(-2, 0, 0);
                    Debug.Log("Got em");
                }
                
                else if (rand2 == 0)
                {
                    transform.position = transform.position + new Vector3(2, 0, 0);
                    Debug.Log("Got em");
                }
            }
            
            else if (rand > 0)
            {
                transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
            }
        }
        
        else if (!_hit)
        {
            CalculateMovement();
        }
    }

    void Update()
    {
        if (transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }
        
        if (Time.time > _canFire && _enemyDestroyed == false)
        {
            _canFire = Time.time + _fireRate;
            
            if (_hitByLaser == false && transform.position.y > _player.transform.position.y)
            {
                Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            }
            
            else if (_hitByLaser == false && transform.position.y < _player.transform.position.y)
            {
                Instantiate(_reverseLaserPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _enemySpeed = 0;
            _amplitude = 0;
            _enemyDestroyed = true;
            Destroy(GetComponent<Collider2D>());
            Destroy(this._rigidBody);
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _spawnManager.AddEnemyToDestroyed();
            Debug.Log(_spawnManager.EnemiesDestroyed());

            if (_player != null)
            {
                _player.Damage();
                _player.AddPoints(20);
            }
            
            else if (_player == null)
            {
                Debug.Log("the damn player is null");
            }

            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {            
            _enemySpeed = 0;
            _amplitude = 0;
            _enemyDestroyed = true;
            Destroy(GetComponent<Sprite>());
            Destroy(GetComponent<Collider2D>());
            Destroy(this._rigidBody);
            Destroy(other.gameObject);
            _audioSource.Play();
            _spawnManager.AddEnemyToDestroyed();
            Debug.Log(_spawnManager.EnemiesDestroyed());
            _animator.SetTrigger("OnEnemyDeath");

            if (_player != null)
            {
                _player.AddPoints(20);
            }

            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Energy")
        {
            _enemySpeed = 0;
            _amplitude = 0;
            _enemyDestroyed = true;
            Destroy(GetComponent<Collider2D>());
            Destroy(this._rigidBody);
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _spawnManager.AddEnemyToDestroyed();
            Debug.Log(_spawnManager.EnemiesDestroyed());
           
            if (_player != null)
            {
                _player.AddPoints(20);
            }
           
            Destroy(this.gameObject, 2.8f);
        }
    }

    public void ChangeEnemySpeed(float speed)
    {
        _enemySpeed = speed;
    }

    private void ZigZag()
    {
        _pos += Vector3.down * Time.deltaTime * _enemySpeed;
        transform.position = _pos + _axis * Mathf.Sin(Time.time * _frequency) * _amplitude;

        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
    }
}
