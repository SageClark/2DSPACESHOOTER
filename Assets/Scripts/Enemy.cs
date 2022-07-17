using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    
    [SerializeField]
    private float _enemySpeed = 2.5f;
    
    private Rigidbody2D _rigidBody;

   
    private Player _player;

    private Animator _animator;
    
    private AudioSource _audioSource;

    private float _canFire = -1;
    private float _fireRate = 3;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private GameObject _laserPrefab;
    private bool _hitByLaser = false;
    


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        
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


    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
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
        if (transform.position.y < -5.4)
        {
            float _randomXPosition = Random.Range(-9.4f, 9.4f);
            transform.position = new Vector3(_randomXPosition, 7.4f, 0);
           
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.tag == "Player")
        {           
            
            if(_player != null)
            {
                _player.Damage();
            }
            else if (_player == null)
            {
                Debug.Log("the damn player is null");
            }

            Destroy(this._rigidBody);
            _animator.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
            
        }

        if (other.tag == "Laser")
        {
            _hitByLaser = true;
            Destroy(this._rigidBody);
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddPoints(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            Destroy(this.gameObject, 2.8f);
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
        }
    }

}
