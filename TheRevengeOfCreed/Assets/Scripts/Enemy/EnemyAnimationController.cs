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
            // Set other animation bools false
            animator.SetBool("isRunning", false);

            // Set action animaiton bool to true
            animator.SetBool("isWalking", true);


        }
    }

    public void Running()
    {
        if (animator != null)
        {
            // Set other animation bools false
            animator.SetBool("isWalking", false);

            // Set action animaiton bool to true
            animator.SetBool("isRunning", true);


        }
    }

    public void Firing()
    {
        if (animator != null)
        {
            // Set other animation bools false
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);

            // Set action animaiton bool to true
            animator.SetBool("isFiring", true);



        }
    }

    public void Dying()
    {
        if (animator != null)
        {
            // Set other animation bools false
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isFiring", false);
            // Set action animaiton bool to true
            animator.SetBool("isDying", true);




        }
    }

}
