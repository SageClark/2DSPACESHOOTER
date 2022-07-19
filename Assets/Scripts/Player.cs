using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    public int _score;
    [SerializeField]
    private int _numOfShields = 0;

    [SerializeField]
    private float _speed = 0.5f;
    [SerializeField]
    private float _shiftSpeed = 2f;
    private float _speedMultiplier = 2;
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    public float horizontalInput;
    public float verticalInput;

    public bool _isTripleShotActive = false;
    public bool _isSpeedActive = false;
    public bool _isShieldActive = false;
    
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    public GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _shieldsImg;
    [SerializeField]
    private GameObject _leftEngineDamaged;
    [SerializeField]
    private GameObject _rightEngineDamaged;
    [SerializeField]
    private GameObject _playerController;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    public Joystick joystick;
    
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;




    // Start is called before the first frame update
    void Start()
    {
        
        transform.position = new Vector3(0.05f, -2.27f, 0.0f);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();


        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is NULL");
        }
        
        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKey(KeyCode.K) && Time.time > _canFire)
        {
            FireLaser();
        }
        
    }


    public void CalculateMovement()
    {
        // set controls for up, down, left, right. back and forth locked on zero.
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        // add speed boost when left shift key is held
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(direction * _shiftSpeed * Time.deltaTime);
        }

        transform.Translate(direction * _speed * Time.deltaTime);
        
        // clamp y pos between -3.8f and 0, wrap x pos from one side of screen to other side:
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    public void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();


        //instantiate 3 lasers (triple shot prefab)


    }

    public void Damage()
    {
        if (_numOfShields > 0)
        {
            _numOfShields--;
            _uiManager.UpdateShield(_numOfShields);
            if (_numOfShields < 1)
            {
                _shieldsImg.SetActive(false);
                _shieldVisualizer.SetActive(false);
            }
                     
        }
        else
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
        }
        
        if(_lives == 2)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _leftEngineDamaged.SetActive(true);
        }
        else if (_lives == 1)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _rightEngineDamaged.SetActive(true);
        }
        else if (_lives < 1)
        {
            Destroy(this.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);          
            _spawnManager.OnPlayerDeath();
            
        }

    }

    public void AddPoints(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());

    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedActive()
    {
        _isSpeedActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());

    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        _isSpeedActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        
        _shieldVisualizer.SetActive(true);
        _numOfShields = 3;
        _shieldsImg.SetActive(true);
        _uiManager.UpdateShield(3);
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isShieldActive = false;
        _shieldVisualizer.SetActive(false);
        _shieldsImg.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            
            Damage();
            
        }
    }
}


