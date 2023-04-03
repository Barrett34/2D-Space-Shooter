using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    private bool _canStrafe;
    private int _strafeDirection = 1;





    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(0, 9f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        {
            if (_canStrafe == true)
            {
                Strafe();
            }
            else
            {
                transform.Translate(Vector3.down * _speed / 3 * Time.deltaTime);
            }
            if (transform.position.y <= 2f)
            {
                _canStrafe = true;
            }
        }
    }

    

    public void Strafe()
    {
        if (_canStrafe == true)
        {

            if (transform.position.x <= -8.5f)
            {
                _strafeDirection = 1;
            }
            else if (transform.position.x >= 8.5f)
            {
                _strafeDirection = -1;
            }
            transform.Translate(Vector3.right * _speed * _strafeDirection * Time.deltaTime);

        }
    }
}
