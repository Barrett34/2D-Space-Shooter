using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{
    private Transform target = null;
    private GameObject[] targets;

    private float _distance;
    private float _closestEnemy = Mathf.Infinity;
    private float _speed = 900f;
    private float _rotationSpeed = 700f;

    [SerializeField]
    private Rigidbody2D homingProjectileRigidBody;
   


    // Start is called before the first frame update
    void Start()
    {
        homingProjectileRigidBody = GetComponent<Rigidbody2D>();

        if(homingProjectileRigidBody == null)
        {
            Debug.LogError("The rigidbody is null");
        }

        FindClosestEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        FireMissle();
    }

    private void FindClosestEnemy()
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in targets)
        {
            _distance = (enemy.transform.position - this.transform.position).sqrMagnitude;

            if(_distance < _closestEnemy)
            {
                _closestEnemy = _distance;
                target = enemy.transform;
            }
        }
    }

    private void FireMissle()
    {
        homingProjectileRigidBody.velocity = (transform.up * _speed * Time.deltaTime);

        if (target != null)
        {
            Vector2 direction = (Vector2)target.position - homingProjectileRigidBody.position;
            direction.Normalize();

            float rotationValue = Vector3.Cross(direction, transform.up).z;
            homingProjectileRigidBody.angularVelocity = -rotationValue * _rotationSpeed;
            homingProjectileRigidBody.velocity = transform.up * _speed * Time.deltaTime;
        }
        MissleDestroyed();
    }

    private void MissleDestroyed() {

        if(transform.position.y > 8f || transform.position.y < -8f || transform.position.x > 11f || transform.position.x < -11f)
        {
            Destroy(this.gameObject);
        } else
        {
            StartCoroutine(SelfDestruct());
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }
    

    
}
