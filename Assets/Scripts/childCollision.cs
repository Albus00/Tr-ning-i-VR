using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class childCollision : MonoBehaviour
{
    public GameObject parentObject; // Ändrad från private
    private float forceMultiplier = 5f;
    private Rigidbody rb;
    //public GameObject thisObject; // debugging
    private GameObject thisObject;
    private void Start()
    {
        //parentObject = GameObject.FindWithTag("enemy"); // Det här är problemet, Första kollisionen funkar men inte följande kollisioner för att alla kloner har taggen "enemy"

        rb = GetComponent<Rigidbody>(); 
        thisObject = gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Vector3 direction = other.transform.position - transform.position;
        Vector3 collisionDirection = (other.transform.position - thisObject.transform.position).normalized;
        parentObject.GetComponent<pathing>().CollisionDetected(thisObject, collisionDirection);

        //Debug.Log(gameObject.name);
        //rb.AddForce(direction.normalized*forceMultiplier);
    }
}
