using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public bool destroyAfterThrow = true;

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //    collision.gameObject.transform.root.GetComponent<BehaviourTest>().projectileCollisionDetected(collision);
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.name.Contains("mixamorig")){
            other.gameObject.transform.root.GetComponent<BehaviourTest>().projectileCollisionDetected(other, transform.position);
            if(destroyAfterThrow)
            {
                Destroy(this.gameObject, 3f);
            }
            
        }
        //Debug.Log(other.gameObject.name);
        

    }
}
