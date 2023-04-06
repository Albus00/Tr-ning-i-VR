using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathing : MonoBehaviour // Skit kod, använd inte
{
    [Header("References")]
    [SerializeField] private Animator animator = null;

    
    //public Transform target;
    private Transform target;
    private float distanceToTarget;
    public float movementSpeed;
    public float healthPoints = 2f;
    public bool killEnemy = false;
    public GameObject weapon;
    private bool isKill = false;


    [Header("Movement on audio")]
    [Range(0, 7)]
    public int band;
    public float lowerActivationLimit;
    private int beatCount;
    public int beatsPerDash;
    private float timer;
    public float minDashInterval = 3f;
    public float dashDistance = 1f; // Dashes 1 unit
    private bool readyToDash;
    private Vector3 dashStartPosition;

    // Start is called before the first frame update
    public void Start()
    {
        readyToDash = false;
        target = GameObject.FindWithTag("player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(!readyToDash)
        {
            NotDashing(); 
        }
        if (readyToDash)
        {
            DashStart();
            //readyToDash = false;
        }

        distanceToTarget = (target.position - gameObject.transform.position).magnitude;
        
        

        //if(distanceToTarget > 1)
        //{
        //    //Vector3 targetDirection = target.position - gameObject.transform.position;
        //    transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
        //}



        gameObject.transform.LookAt(target);
    }



    public void CollisionDetected(GameObject childObject, Vector3 collisionDirection)
    {
        Debug.Log("child collided: " + childObject);
        animator.enabled = false;
        //childObject.GetComponent<Rigidbody>().AddForce(collisionDirection*300, ForceMode.Impulse);
        this.enabled = false;

        //kill();
    }

    
    private void NotDashing()
    {
        animator.Play("tpose");
        timer += Time.time;
        
        if (AudioP.audioBandbuffer[band] >= lowerActivationLimit)
        {
            beatCount++;
        }
        if (timer >= minDashInterval && beatCount >= beatsPerDash)
        {
            readyToDash = true;
            dashStartPosition = transform.position; // records position where dash started
        }
        Debug.Log("Beat Count: " + beatCount + "    Ready To Dash?: " + readyToDash);
        
    }
    private void DashStart() 
    { 
        if((transform.position - dashStartPosition).magnitude < dashDistance)
        {
            animator.Play("run");
            transform.position = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
            Debug.Log("Dashing");
        }
        else
        {
            timer = 0;
            beatCount = 0;
            readyToDash = false;
            Debug.Log("Reset-------------------------------------");
        }
    }
    //private void DashOngoing()
    //{

    //}
    //private void DashEnd() 
    //{ 
        
    //}
    
}
