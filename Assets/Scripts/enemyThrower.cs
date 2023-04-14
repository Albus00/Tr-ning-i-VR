using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyThrower : MonoBehaviour
{
    public GameObject target;
    public Rigidbody projectilePrefab;

    public AudioSource audioSourceThrow;
    public AudioClip clip;
    public float throwVolume = 0.3f;

    public float timeToTarget = 5.0f;
    //public float throwFrequency = 4.0f;
    public float currTime = 0.0f;

    void Start(){
        
    }

    void Update(){
        currTime += Time.deltaTime;

        if(timeToTarget == 0.0f) timeToTarget = 0.01f;
        
        transform.LookAt(target.transform);

        transform.position = new Vector3(Mathf.Sin(Time.time)*10.0f,-1.8f,Mathf.Cos(Time.time)*10.0f);

        if(currTime > timeToTarget){
            audioSourceThrow.PlayOneShot(clip, throwVolume);

            float distance = (target.transform.position - gameObject.transform.position).magnitude;
            float gravitys = Physics.gravity.magnitude;

            float V_0x = distance/timeToTarget;
            float V_0y = (gravitys*(timeToTarget)*(timeToTarget)/2.0f)/timeToTarget;
            
            currTime = 0.0f;
            Rigidbody projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            // Calculate the direction to the target
            Vector3 targetDirection = target.transform.position - transform.position;
            targetDirection.y = 0f;

            // Calculate the angle to the target
            float angle = Vector3.Angle(targetDirection, transform.forward);

            // Rotate the projectile towards the target
            projectile.transform.rotation = Quaternion.LookRotation(targetDirection);

            // Calculate the initial velocity of the projectile
            Vector3 initialVelocity = projectile.transform.forward * V_0x;
            initialVelocity.y = V_0y;

            // Set the velocity of the projectile
            projectile.velocity = initialVelocity;
        }
    }
}


