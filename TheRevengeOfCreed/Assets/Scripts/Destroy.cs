using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject bulletCollsion; 
    // Start is called before the first frame update
    void Start()
    {
        Destroy(bulletCollsion, 3);
    }

}
