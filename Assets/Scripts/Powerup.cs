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
    [SerializeField]
    private int _powerupID;
    [SerializeField]
    private AudioClip _clip;

    void Start()
    {
        transform.position = new Vector3(Random.Range(-9f, 9f), 15f, 0f);
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)

                AudioSource.PlayClipAtPoint(_clip, transform.position);

            {
                switch(_powerupID)
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
                    default:
                        break;
                }
            }
            
            Destroy(this.gameObject);
            
        }
    }
}
