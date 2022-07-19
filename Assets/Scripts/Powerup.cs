using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    //ID for Powerups
    //0 = Triple shot
    //1 = speed
    //2 = shields
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;
    private UIManager _uiManager;


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

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                //if powerUP is 0
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
                }

            }
            Destroy(this.gameObject);
        }
    }

    //only be collectable by the player (use tags)
    //on collected, destroy
}
