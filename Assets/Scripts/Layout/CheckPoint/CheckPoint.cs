using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Animator animator;
    private bool _isActivated = false; 

    private void Start()
    {
        animator = GetComponent<Animator>();  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_isActivated)
        {
            animator.SetTrigger("CheckActive");
            CheckPointSystem.instance.LastCheckPoint(gameObject);
            _isActivated = true; 
        }
    }

    private void OnDrawGizmos()
    {
       
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}

