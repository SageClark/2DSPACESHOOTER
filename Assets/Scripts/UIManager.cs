using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    
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

    private GameObject _allObjects;

    void Start()
    {
        _allObjects = GameObject.Find("Spawn_Manager");
        _scoreText.text = "" + 0;
    
        if (_gameManager == null)
        {
            Debug.Log("Game Manager is null.");
        }
    }

    public void UpdateScore(int playerScore)
    {     
        _scoreText.text = "" + playerScore;
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
