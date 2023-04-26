using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //    collision.gameObject.transform.root.GetComponent<BehaviourTest>().projectileCollisionDetected(collision);
    //}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        other.gameObject.transform.root.GetComponent<BehaviourTest>().projectileCollisionDetected(other, transform.position);

    }
}
