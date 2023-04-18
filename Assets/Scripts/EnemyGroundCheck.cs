using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    private EnemyBehaviour _enemyBehaviour; // Parent character controller.

    void Awake()
    {
        _enemyBehaviour = transform.parent.gameObject.GetComponent<EnemyBehaviour>(); // Get character controller from parent.
    }

    private void OnTriggerEnter(Collider other)
    {
        _enemyBehaviour.isGrounded = true;
        Debug.Log("ENTER");
    }

    private void OnTriggerExit() 
    {
        _enemyBehaviour.isGrounded = false;
    }
}
