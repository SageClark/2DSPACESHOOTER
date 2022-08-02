using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _specialText;

    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _ammoText;
    
    [SerializeField]
    private Image _LivesImg;

    [SerializeField]
    private Image _shieldsImg;
    
    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Sprite[] _shieldSprites;
   
    [SerializeField]
    private Text _gameOverText;
    
    [SerializeField]
    private Text _restartText;
    
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private Button _playAgainButton;

    [SerializeField]
    private Player playerScript;

    public Slider slider;

    private GameObject _allObjects;

    public int ammoCount = 20;


    void Start()
    {
        _allObjects = GameObject.Find("Spawn_Manager");
        slider = GameObject.Find("ThrusterSlider").GetComponent<Slider>();

        _scoreText.text = "" + 0;
        _ammoText.text = "AMMO: " + 20;
        _specialText.text = "SPECIAL: " + 0;

        if (_gameManager == null)
        {
            Debug.Log("Game Manager is null.");
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            slider.value--;
        }
    }

    public void AddAmmo()
    {
        ammoCount += 10;
        UpdateAmmo(ammoCount);
        Debug.Log("Ammo Added");
    }

    public void updateSpecialCount(int specialCount)
    {
        _specialText.text = "SPECIAL: " + specialCount;
        if (specialCount == 0)
        {
            _specialText.color = Color.red;
        }
        else if (specialCount > 0)
        {
            _specialText.color = Color.green;
        }
    }

    public void UpdateAmmo(int ammoCount)
    {
        _ammoText.text = "AMMO: " + ammoCount;
        
        if(ammoCount == 0)
        {
            _ammoText.color = Color.red;
        }
        if (ammoCount <= 10 && ammoCount > 0)
        {
            _ammoText.color = Color.yellow;
        }
        else if (ammoCount > 10)
        {
            _ammoText.color = Color.green;
        }
        Debug.Log(ammoCount);
    }

    public void UpdateScore(int playerScore)
    {     
        _scoreText.text = " " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateShield(int currentShields)
    {
        _shieldsImg.sprite = _shieldSprites[currentShields];
    }

    void GameOverSequence()
    {

        Destroy(_allObjects);
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverTextFlicker());
        _restartText.gameObject.SetActive(true);


    }
    
    IEnumerator GameOverTextFlicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(true);
        }
    }
}
