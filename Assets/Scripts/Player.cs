using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // public or private reference
    // data type (int, float, bool, string)
    // every variable has a name
    // optional value assigned

    [SerializeField]
    private float _speed = 3.5f;


    // SerializeField allows you to access private variables on the unity editor


    // Start is called before the first frame update
    void Start()
    {
            // take the current position = new position (0,0,0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalInput * _speed *  Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        // if player position on the y is greater than 0
        // y position = 0
        // else if position on the y is -3.8f
        // y position = -3.8f

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        // if player on the x > 11
        // x pos = -11
        // else if player on the x is less than -11
        // x pos = 11

    }
}
