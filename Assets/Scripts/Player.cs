using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private int _lives = 3;
    public int _score;
    private int _numOfShields = 0;
    private int _specialCount;

    private float _speed = 3.5f;
    [SerializeField]
    private float _shiftSpeed = 2f;
    private float _speedMultiplier = 2;
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    public float horizontalInput;
    public float verticalInput;
    private float _dimmerWaitTime;

    public bool _isTripleShotActive = false;
    public bool _isSpeedActive = false;
    public bool _isShieldActive = false;
    private bool _canMove = true;
    private bool _bossIsAlive = true;
    private bool _victoryNotStarted = true;
    private bool _startDimming = false;

    private ColorGrading _colorGrading;

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
    [SerializeField]
    private GameObject _energyBallPrefab;
    [SerializeField]
    private Image _bar;
    [SerializeField]
    private GameObject _electrifiedPrefab;
    [SerializeField]
    private PostProcessVolume _processingVolume;
    [SerializeField]
    private GameObject _tractorBeam;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    public Joystick joystick;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    public AudioClip _laserEmptyClip;
    [SerializeField]
    public AudioSource _audioSource;

    [SerializeField]
    private GameObject _victoryText1;
    [SerializeField]
    private GameObject _victoryText2;
    [SerializeField]
    private GameObject _victoryText3;




    // Start is called before the first frame update
    void Start()
    {
        
        transform.position = new Vector3(0.05f, -2.27f, 0.0f);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is NULL");
        }

        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        _processingVolume.profile.TryGetSettings(out _colorGrading);
        _dimmerWaitTime = Time.time;
    }

    void Update()
    {
        if (!_bossIsAlive && _victoryNotStarted)
        {
            _victoryNotStarted = false;
            StartCoroutine(VictoryRoutine());
        }

        if (_canMove == true)
        {
            CalculateMovement();

            if (Input.GetKey(KeyCode.K) && Time.time > _canFire)
            {
                FireLaser();
            }
            if (Input.GetKeyDown(KeyCode.O) && Time.time > _canFire && _specialCount > 0 && _spawnManager.EnemiesDestroyed() != 20)
            {
                FireEnergyBall();
                _specialCount--;
                _uiManager.updateSpecialCount(_specialCount);
            }
            if (Input.GetKey(KeyCode.C))
            {
                GameObject _powerUp = GameObject.FindGameObjectWithTag("PowerUp");
                _tractorBeam.SetActive(true);
                _powerUp.transform.position = Vector3.MoveTowards(transform.position, this.transform.position, _speed * _speedMultiplier * Time.deltaTime);
            }
        }
        if (_startDimming)
        {
            if (_colorGrading.contrast.value > -100 || _colorGrading.saturation.value > -100)
            {
                if (Time.time > _dimmerWaitTime)
                {
                    _dimmerWaitTime = Time.time + 0.05f;
                    _colorGrading.contrast.value = _colorGrading.contrast.value--;
                    _colorGrading.contrast.value = _colorGrading.saturation.value--;
                }
            }
            else
            {
                StartCoroutine(BackToMainMenu());
            }
        }
    }

    public void AddToSpecialCount()
    {
        _specialCount++;
    }

    public int GetSpecialCount()
    {
        return _specialCount;
    }

    public int GetPlayerHealth()
    {
        return _lives;
    }
   
    public void CalculateMovement()
    {
        // set controls for up, down, left, right. back and forth locked on zero.
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        // add speed boost when left shift key is held
        if (Input.GetKey(KeyCode.LeftShift) && _bar.fillAmount > 0.1f)
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

        if (_isTripleShotActive == true && _uiManager.GetAmmoCount() > 0)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _audioSource.clip = _laserSoundClip;
            _audioSource.Play();
            _uiManager.ReduceAmmoCount();
            _uiManager.UpdateAmmo(_uiManager.GetAmmoCount());
        }
        else if (_isTripleShotActive == false && _uiManager.GetAmmoCount() > 0)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            _audioSource.clip = _laserSoundClip;
            _audioSource.Play();
            _uiManager.ReduceAmmoCount();
            _uiManager.UpdateAmmo(_uiManager.GetAmmoCount());
        }

        else if (_isTripleShotActive == false && _uiManager.GetAmmoCount() < 1)
        {
            _uiManager.UpdateAmmo(_uiManager.GetAmmoCount());
            _audioSource.clip = _laserEmptyClip;
            _audioSource.Play();
        }
    }

    public void FireEnergyBall()
    {     
        Instantiate(_energyBallPrefab, transform.position + new Vector3(0, 1.1f, 0), Quaternion.identity);
    }
    
    public void Damage()
    {
        CameraShake.Shake(0.10f, 0.5f);

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

    public void StopPlayerMovement()
    {
        _canMove = false;
    }

    public void StartPlayerMovement()
    {
        _canMove = true;
    }

    public void AddHealth()
    {
        //if player is not over the 3 lives cap, add 1 life
        if (_lives <= 2)
        {
            _lives++;
        }
        //if health is being raised from 1 to 2, deactivate exploded effect on right thruster
        if (_lives == 2)
        {
            _rightEngineDamaged.SetActive(false);
        }
        // if health is being raise from 2 to 3, deactivate exploded effect on right thruster
        else if (_lives == 3)
        {
            _leftEngineDamaged.SetActive(false);
        }
        
        _uiManager.UpdateLives(_lives);
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

    public void ElectrifiedActive()
    {
        _canMove = false;
        _electrifiedPrefab.SetActive(true);
        StartCoroutine(Electrified());
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

    public IEnumerator Electrified()
    {
        yield return new WaitForSeconds(2);
        _electrifiedPrefab.SetActive(false);
        _canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);          
            Damage();          
        }
        
        if (other.CompareTag("Boss"))
        {
            Damage();
        }
    }

    public void BossIsDead()
    {
        _bossIsAlive = false;
    }
    
    public void StartVictory()
    {
        StartCoroutine(VictoryRoutine());
    }

    public void StartDimming()
    {
        while (_colorGrading.contrast.value > -100 || _colorGrading.hueShift.value > -180 || _colorGrading.saturation.value > -100)
        {
            if (Time.time > _dimmerWaitTime)
            {
                _dimmerWaitTime = Time.time + 0.1f;
                _colorGrading.contrast.value = _colorGrading.contrast.value--;
                _colorGrading.contrast.value = _colorGrading.hueShift.value--;
                _colorGrading.contrast.value = _colorGrading.saturation.value--;
            }
        }
    }
    
    public IEnumerator VictoryRoutine()
    {
        StartPlayerMovement();
        _victoryText1.SetActive(true);
        yield return new WaitForSeconds(2f);
        _victoryText1.SetActive(false);
        yield return new WaitForSeconds(1f);
        _victoryText2.SetActive(true);
        yield return new WaitForSeconds(2f);
        _victoryText2.SetActive(false);
        yield return new WaitForSeconds(1f);
        _victoryText3.SetActive(true);
        yield return new WaitForSeconds(2f);
        _victoryText3.SetActive(false);
        _startDimming = true;
    }

    private IEnumerator BackToMainMenu()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}


