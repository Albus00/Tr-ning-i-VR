//Traning i vr
//Klas Nordquist
//Victor Persson

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{   
    private Vector3 target; // player position
    private bool startMovingTowardPlayer; // set to true when the function CommandToMoveReceiver() is called from the fridge throwing enemy (BehaviourTest)
    public float heightAboveTarget; // how far over the target the fridge should go.
    public float timeToTarget = 5.0f;
    public float maxRandRotMagn = -2.0f;
    public float minRandRotMagn = -2.0f;
    private Rigidbody fridgeRB;
    public bool hasThrown = false;
    private float gravitys;
    private float theImpulse;

    public AudioClip[] groundHitSound;
    private AudioClip ground;
    public AudioClip fridgeRattle;

    private AudioSource fridgeAudio;

    public HealthManager playerHealth;


    void Start()
    {
        hasThrown = false;
        target = GameObject.FindWithTag("Player").transform.position;
        startMovingTowardPlayer = false;
        gravitys = Physics.gravity.magnitude;
        fridgeAudio = GetComponent<AudioSource>();
        playerHealth = GameObject.FindWithTag("HealthHandler").GetComponent<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startMovingTowardPlayer && !(hasThrown))
        {
            Vector3 adjustedTarget = target + new Vector3(0, 10f, 0);
            float distance = (adjustedTarget - gameObject.transform.position).magnitude;
            float V_0x = distance/timeToTarget;
            float V_0y = (gravitys*(timeToTarget)*(timeToTarget)/2.0f)/timeToTarget;

            Vector3 targetDirection = adjustedTarget - transform.position;
            float angle = Vector3.Angle(targetDirection, transform.forward);
            transform.rotation = Quaternion.LookRotation(targetDirection);

            Vector3 initialVelocity = transform.forward * V_0x;
            initialVelocity.y = V_0y + 2f;

            fridgeRB = GetComponent<Rigidbody>();
            
            fridgeRB.angularVelocity = new Vector3(Random.Range(minRandRotMagn,maxRandRotMagn),
                                                   Random.Range(minRandRotMagn,maxRandRotMagn),
                                                   Random.Range(minRandRotMagn,maxRandRotMagn));
            fridgeRB.velocity = initialVelocity;
            hasThrown = true;
            fridgeAudio.PlayOneShot(fridgeRattle);
            Destroy(gameObject,6.0f); //Remove the object after 6 seconds
        }

        if(hasThrown){

        }
    }

    public void CommandToMoveReceiver()
    {
        startMovingTowardPlayer = true; 
    }

    void OnCollisionEnter(Collision collision)
    {
        theImpulse = (collision.impulse/Time.fixedDeltaTime).magnitude;  //Returns max. around 480-530 N if player stands still

        if(collision.gameObject.tag == "Ground" && hasThrown && theImpulse > 100.0f)
        {
            PlayHitEffect();
        }else if(collision.gameObject.tag == "Player" && hasThrown && theImpulse > 100.0f)
        {
            PlayHitEffect();

            float damage = theImpulse/9.0f; //float: higher = less damage taken
            if(damage > 10.0f){
                Debug.Log("Object hit player for: " + Mathf.Round(theImpulse) + " Newton, which is: " + Mathf.Round(damage) + " damage.");
                playerHealth.subtractHealth(damage);
            } 
        }
    }

    void PlayHitEffect(){
        fridgeAudio.Stop(); //stop rattling
        int randomIndex = Random.Range(0, groundHitSound.Length);
        ground = groundHitSound[randomIndex];
        fridgeAudio.PlayOneShot(ground, 0.8f); //play large impact effect
    }
}
