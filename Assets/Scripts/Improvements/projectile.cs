using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public bool destroyAfterThrow = true;
    private Vector3 _previousPosition;
    private Vector3 _positionChange;
    private bool projectileActive;
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log(collision.gameObject.name);
    //    collision.gameObject.transform.root.GetComponent<BehaviourTest>().projectileCollisionDetected(collision);
    //}
    void Start()
    {
        projectileActive = false;
        _previousPosition = transform.position;
    }
    void Update()
    {
        _positionChange = transform.position - _previousPosition;
        _previousPosition = transform.position;
        if(!projectileActive && _positionChange.magnitude > 0.001f)
        {
            projectileActive = true;
        }
        if ( _positionChange.magnitude < 0.001f)
        {
            projectileActive = false;
        }
        // Log the position change to the console
        //Debug.Log($"Position Change: {_positionChange}");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.name.Contains("mixamorig") && projectileActive)
        {
            other.gameObject.transform.root.GetComponent<BehaviourTest>().projectileCollisionDetected(other, transform.position);

        }
        //Debug.Log(other.gameObject.name);
        

    }
}
