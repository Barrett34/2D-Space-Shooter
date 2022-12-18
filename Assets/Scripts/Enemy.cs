using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _speed = 4f;

    private Player _player;


    void Start()
    {
        transform.position = new Vector3(Random.Range(-9f,9f), 11, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -5.2f )
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 11f, 0);
        }
         
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _player.AddScore(10);
            Destroy(this.gameObject);
        }
    }
}
