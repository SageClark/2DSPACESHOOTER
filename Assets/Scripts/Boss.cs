using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private float _bossHealth = 1f;
    private float _speed = 2f;
    private float _newSpeed = 0f;
    private float _frequency = 1f;
    private float _amplitude = 8f;
    private float _bossHealthText = 20;

    private bool _bossIsAlive = true;
    private bool _movingDown = true;
    private bool _deathRoutineStarted = false;

    [SerializeField]
    private GameObject _charge;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _bossBar;
    [SerializeField]
    private Sprite _bossHitSprite;
    [SerializeField]
    private Sprite _bossSprite;
    [SerializeField]
    private Text _BossHealthBarText;
    [SerializeField]
    private GameObject _laserBeam;
    [SerializeField]
    private GameObject _bossHealthBar;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _audioClip;

    private SpriteRenderer _spriteRenderer;
    private Player _player;

    private GameObject[] _enemyArray;

    private Vector3 _pos;
    private Vector3 _axis;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCharge());
        StartCoroutine(SpawnEnemies());
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer is null (enemy)");
        }
        
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }

        _bossHealthBar.SetActive(true);
        if (_bossHealthBar == null)
        {
            Debug.LogError("Boss Health Bar is null (enemy)");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_deathRoutineStarted)
        {
            if (transform.position.y > 3.8f)
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
            else if (transform.position.y < 3.8f && _movingDown)
            {
                _pos = transform.position;
                _axis = transform.right;
                _movingDown = false;
            }
            else if (transform.position.y < 3.8f && !_movingDown)
            {
                ZigZag();
            }
        }
        else
        {
            transform.position = transform.position;
        }

        if (_bossHealth < 0.01f && !_deathRoutineStarted)
        {
            DestroyAllEnemies();
            _deathRoutineStarted = true;
            _player.AddHealth();
            _player.AddHealth();
            _player.AddHealth();
            _player.StopPlayerMovement();
            StartCoroutine(BossDeathRoutine());
            
        }
    }

    void ZigZag()
    {
        _pos += Vector3.down * Time.deltaTime * _newSpeed;
        transform.position = _pos + _axis * Mathf.Sin(Time.time * _frequency) * _amplitude;     
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _spriteRenderer.sprite = _bossHitSprite;
            StartCoroutine(BossHitRoutine());
            _bossHealth -= 0.05f;
            _bossHealthText -= 1;
            _BossHealthBarText.text = _bossHealthText + " / 20";
            Destroy(other.gameObject);
        }
    }

    public float GetBossHealth()
    {
        return _bossHealth;
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(4f);
        while (_bossIsAlive && !_deathRoutineStarted)
        {
            Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(2.7f);
        }
    }

    IEnumerator StartCharge()
    {
        yield return new WaitForSeconds(7f);
        while (_bossIsAlive && !_deathRoutineStarted)
        {
            _charge.SetActive(true);
            yield return new WaitForSeconds(2f);
            _charge.SetActive(false);
            _laserBeam.SetActive(true);
            yield return new WaitForSeconds(1f);
            _laserBeam.SetActive(false);
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator BossHitRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.sprite = _bossSprite;
    }

    IEnumerator BossDeathRoutine()
    {

        CameraShake.Shake(6f, 0.5f);
        _audioSource.clip = _audioClip;
        _audioSource.Play();
        Instantiate(_explosionPrefab, new Vector3(3.84f, 2.87f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(_explosionPrefab, new Vector3(-6.19f, 1.13f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(_explosionPrefab, new Vector3(3.13f, -2.38f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(_explosionPrefab, new Vector3(6.19f, -1.13f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(_explosionPrefab, new Vector3(-3.84f, -2.87f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(_explosionPrefab, new Vector3(3.13f, -2.38f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(_explosionPrefab, new Vector3(3.84f, 2.87f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(_explosionPrefab, new Vector3(-6.19f, 1.13f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(_explosionPrefab, new Vector3(3.13f, -2.38f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(_explosionPrefab, new Vector3(6.19f, -1.13f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(_explosionPrefab, new Vector3(-3.84f, -2.87f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(_explosionPrefab, new Vector3(3.13f, -2.38f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        _player.BossIsDead();
        this.gameObject.SetActive(false);
    }

    private void DestroyAllEnemies()
    {
        _enemyArray = GameObject.FindGameObjectsWithTag("Enemy");

        for (var i = 0; i < _enemyArray.Length; i++)
        {
            Destroy(_enemyArray[i]);
        }
    }
}
