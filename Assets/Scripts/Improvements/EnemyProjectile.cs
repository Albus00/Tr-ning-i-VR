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
    void Start()
    {
        desiredRotationSet = false;
        desiredRotation = new Vector3(4.76f, -4.677f, -2.496f);
        //airSpeed = 5f;
        //heightAboveTarget = 2f;
        target = GameObject.FindWithTag("Player").transform.position;
        startMovingTowardPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startMovingTowardPlayer)
        {
            if (!desiredRotationSet)
            {
                transform.eulerAngles = desiredRotation;
                desiredRotationSet = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x, target.y+heightAboveTarget, target.z), airSpeed * Time.deltaTime);
        }
    }

    public void CommandToMoveReceiver()
    {
        startMovingTowardPlayer = true; 
    }
}
