using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    EnemyAnimationController _animation;
    PatrolController _patrol;
    Vector3 initialPosition;    // Return to this position after losing enemy - chasing, try targeting first

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



}
