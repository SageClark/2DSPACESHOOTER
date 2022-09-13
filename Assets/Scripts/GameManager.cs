using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int _lives;

    [SerializeField]
    private bool _isGameOver;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private GameObject _boss;

    [SerializeField]
    private GameObject _spawnManager;

    [SerializeField]
    private GameObject _enemyContainer;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); // level 1 scene
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        _lives = _player.GetPlayerHealth();

        if(_lives == 0 || _player == null)
        {
            Destroy(_boss);
            Destroy(_spawnManager);
            Destroy(_enemyContainer);
        }
    }

    public void GameOver()
    {
        Debug.Log("GameManager::GameOver() called");
        _isGameOver = true;
    }    
}

