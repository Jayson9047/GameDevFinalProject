using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{

    
    [SerializeField] private float speed = 30f;
    [SerializeField] private Transform vfxHitRegular;
    [SerializeField] private Transform vfxHitGreen;

    private Rigidbody bulletRigid;
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
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(vfxHitRegular, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
