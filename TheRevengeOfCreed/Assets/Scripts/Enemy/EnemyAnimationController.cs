using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Walking()
    {
        if (animator != null)
        {
            // Set action animaiton bool to true
            animator.SetBool("isWalking", true);

            // Set other animation bools false
        }
    }
}
