using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3f;
    //create id for power up
    // 0 = triple shot
    // 1 = speed
    // 2 = shields
    // 3 = ammo
    //4 = 1 UP
    [SerializeField]
    private int _powerupID;
    [SerializeField]
    private AudioClip _clip;
    private Player _player;

    void Start()
    {
        transform.position = new Vector3(Random.Range(-9f, 9f), 15f, 0f);
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, 5 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)

                AudioSource.PlayClipAtPoint(_clip, transform.position);

            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoReload();
                        break;
                    case 4:
                        player.AddPlayerLife();
                        break;
                    case 5:
                        player.BigShotActive();
                        break;
                    case 6:
                        player.SpeedDecreaserActive();
                        break;
                    case 7:
                        player.HomingMissleActive();
                        break;
                    default:
                        break;
                }
            }

            Destroy(this.gameObject);
        }   
    }
}
