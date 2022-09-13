using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;
    private UIManager uiManager;
    private GameObject _player;

    private void Start()
    {
        uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        if (uiManager == null)
        {
            Debug.Log("UI Manager is NULL");
        }
        
        _player = GameObject.Find("Player");
        if (_player == null)
        {
            Debug.Log("Player is NULL");
        }
    }

    // Update is called once per frame    
    void Update()
    {
        //move down at a speed of 3
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //when we leave the screen, destroy me (this object)
        if(transform.position.y <= -6)
        {
            Destroy(gameObject);
        }

        if (Input.GetKey(KeyCode.C))
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * 2 * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {      
        if (other.tag == "EnemyLaser" && this.tag != "PowerDown")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();           
            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AddHealth();
                        break;
                    case 4:
                        uiManager.AddAmmo();
                        break;
                    case 5:
                        player.AddToSpecialCount();
                        uiManager.updateSpecialCount(player.GetSpecialCount());
                        break;
                    case 6:
                        player.ElectrifiedActive();
                        break;
                }
            }
            
            else
            {
                Debug.Log("UIManager is null");
            }
            Destroy(this.gameObject);
        }
    }
}
