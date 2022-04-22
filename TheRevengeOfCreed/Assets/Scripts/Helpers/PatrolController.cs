using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatrolState { Turning, Walking, Idle, Following, Attacking}
public class PatrolController : MonoBehaviour
{
    
    public Transform[] points;
    int current;
    public float speed;
    public bool isPatrolling = false;
    public float RotationSpeed;
    public PatrolState patrolState = PatrolState.Idle;

    public float distance = 8f;
    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    // Start is called before the first frame update
    void Start()
    {
        current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPatrolling)
        {
            Patrol();
        }

        if (DetectPlayer() == true)
        {
            // Follow Player
           
            isPatrolling = false;

        }
    }

    
    void MoveToPlayer(Transform target)
    {
        RotateToPosition(target);
        var _distanceToTarget = Vector3.Distance(transform.position, target.position);


        // Move to shooting range
        if (_distanceToTarget > distance)
        {
            Vector3.Distance(transform.position, target.position);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * 2 * Time.deltaTime);
            patrolState = PatrolState.Following;
        } else
        {
            patrolState = PatrolState.Attacking;
        }

    }
    bool DetectPlayer()
    {
        bool detected = false;
        var rayPosition = new Vector3(transform.position.x, 1, transform.position.z) ;
     
        var ray = new Ray(rayPosition, _direction);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                detected = true;
                MoveToPlayer(hit.transform);
            }
        }

        return detected;
    }

    void Patrol()
    {
        if (transform.rotation != _lookRotation)
        {
            patrolState = PatrolState.Turning;
            RotateToPosition(points[current]);
        } else
        {
            if (transform.position != points[current].position)
            {
                transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);
                patrolState = PatrolState.Walking;
            }
            else
            {
                current = (current + 1) % points.Length;
                RotateToPosition(points[current]);
            }
        }

    }

    void RotateToPosition(Transform target)
    { 

        //find the vector pointing from our position to the target
        _direction = (target.position - transform.position).normalized;

        //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
  

    }
}
