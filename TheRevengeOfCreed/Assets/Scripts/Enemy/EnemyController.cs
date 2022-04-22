using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    EnemyAnimationController _animation;
    PatrolController _patrol;
    Vector3 initialPosition;    // Return to this position after losing enemy - chasing, try targeting first

    public int health = 100;
    public int damageReceieved = 25;

    // Start is called before the first frame update - Do initial setup of enemy
    void Start()
    {
        _animation = GetComponent<EnemyAnimationController>();
        _patrol = GetComponent<PatrolController>();
        initialPosition = transform.position;
       

    }

    // Update is called once per frame
    void Update()
    {
        if (_patrol.patrolState == PatrolState.Walking)
        {
            _animation.Walking();
        }

        if (_patrol.patrolState == PatrolState.Following)
        {

            _animation.Running();
        }

        if (_patrol.patrolState == PatrolState.Attacking)
        {
            _animation.Firing();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Determine if colliding with bullet
        if (collision.gameObject.tag.ToLower() == "bullet")
        {
            health = health - damageReceieved;
            if (health <= 0)
            {
                _patrol.isPatrolling = false;
                _patrol.patrolState = PatrolState.Idle;
                _animation.Dying();
            }

        }
    }

    public void Death()
    {
        Destroy(gameObject);
        
    }



}
