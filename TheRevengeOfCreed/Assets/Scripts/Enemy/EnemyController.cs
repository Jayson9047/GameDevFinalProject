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
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _animation.Walking();

        if (DetectPlayer() == true)
        {
            Debug.Log("I see player");
        }

    }

    bool DetectPlayer()
    {
        bool detected = false;
        Vector3 rayPosition = transform.position;
        rayPosition.y = 1;
        var ray = new Ray(rayPosition, this.transform.forward);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                detected = true;
            }
        }

        return detected;
    }

}
