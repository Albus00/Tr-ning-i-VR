using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{   // want rotation to be x = 4.76, y = -4.677, z = -2.496
    private Vector3 target; // player position
    private bool startMovingTowardPlayer; // set to true when the function CommandToMoveReceiver() is called from the fridge throwing enemy (BehaviourTest)
    public float airSpeed; // how fast the fridge flies
    public float heightAboveTarget; // how far over the target the fridge should go.
    private Vector3 desiredRotation;
    private bool desiredRotationSet;
    public float timeToTarget = 5.0f;
    public float maxRandRotMagn = -2.0f;
    public float minRandRotMagn = -2.0f;
    private Rigidbody fridgeRB;
    public bool hasThrown = false;
    private float gravitys;

    void Start()
    {
        hasThrown = false;
        desiredRotationSet = false;
        desiredRotation = new Vector3(4.76f, -4.677f, -2.496f);
        //airSpeed = 5f;
        //heightAboveTarget = 2f;
        target = GameObject.FindWithTag("Player").transform.position;
        startMovingTowardPlayer = false;
        gravitys = Physics.gravity.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (startMovingTowardPlayer && !(hasThrown))
        {
            float distance = (target - gameObject.transform.position).magnitude;
            
            float V_0x = distance/timeToTarget;
            float V_0y = (gravitys*(timeToTarget)*(timeToTarget)/2.0f)/timeToTarget;

            Vector3 targetDirection = target - transform.position;
            targetDirection.y = 0f;

            float angle = Vector3.Angle(targetDirection, transform.forward);
            transform.rotation = Quaternion.LookRotation(targetDirection);

            Vector3 initialVelocity = transform.forward * V_0x;
            initialVelocity.y = V_0y;

            fridgeRB = GetComponent<Rigidbody>();
            
            fridgeRB.angularVelocity = new Vector3(Random.Range(minRandRotMagn,maxRandRotMagn),Random.Range(minRandRotMagn,maxRandRotMagn),Random.Range(minRandRotMagn,maxRandRotMagn));
            fridgeRB.velocity = initialVelocity;

            hasThrown = true;
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, target.y+heightAboveTarget, target.z), airSpeed * Time.deltaTime);
        }

        if(hasThrown){

        }
    }

    public void CommandToMoveReceiver()
    {
        startMovingTowardPlayer = true; 
    }
}
