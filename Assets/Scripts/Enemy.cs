using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 11, 0);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -5.2 )
        {
            transform.position = new Vector3(Random.Range(-9, 9), 11, 0);
        }
    }
}
