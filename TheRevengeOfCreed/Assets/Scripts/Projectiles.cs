using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{

    private Rigidbody bulletRigid;
    [SerializeField] private float speed = 30f;


    private void Awake()
    {
        bulletRigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
        bulletRigid.velocity = transform.forward * speed;
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if(other.gameObject.CompareTag("Enemy"))
        {
            //hit
        }
        else
        {

        }
        Destroy(gameObject);
    }
}
