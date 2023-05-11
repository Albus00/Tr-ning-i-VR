using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UltimateXR.Extensions.Unity.Math;
using Unity.VisualScripting;
using UnityEngine;

// https://forum.unity.com/threads/dynamic-ragdoll-example-package.653281/
//Set up mixamo model: https://www.youtube.com/watch?v=KuMe6Iz8pFI
public class EnemyBehaviour : MonoBehaviour
{
    public Animation lightAnimation;

    // Counter for kills and variables for playing death sound
    KillCounter killCounterScript;
    public AudioSource source;
    public AudioClip clip;

    // OBS: State machine anv�nds inte �n.
    // --------- Enemy State --------- //
    private enum EnemyState
    {
        Running,
        Dash,
        Ragdoll
    }
    private EnemyState currentState = EnemyState.Running;

    // --------- Physics --------- //
    private float gravityForce = -9.82f;
    public bool isGrounded = false;
    public float groundedLength = 0.2f;

    // --------- Limb Handling --------- //
    private Rigidbody[] ragdollRigidbodies;
    private Rigidbody hitRigidbody;


    // --------- Movement handling --------- //
    private Transform target;
    private float distanceToTarget;
    public float movementSpeed;
    // ---- Dashing ---- //
    private float dashDistance;
    public float dashSpeed;
    Vector3 dashTarget;
    public GameObject testBox;

    // --------- Components --------- //
    private Animator animator;
    private CharacterController characterController;

    void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        target = GameObject.FindWithTag("Player").transform;
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll(); // No ragdoll as long as enemy isnt dead.
    }
    private void Start()
    {
        lightAnimation = GameObject.Find("LightKillAnimation").GetComponent<Animation>();

        killCounterScript = GameObject.Find("KCO").GetComponent<KillCounter>();
        //firstCall = true;
        dashDistance = 1f;
        dashSpeed = 10f;
    }
    
    // Update is called once per frame
    void Update()
    {
        isGrounded = (Physics.Raycast(transform.position, Vector3.down, groundedLength));
        // Checks if enemy is grounded (done in child gameobject)
        if (!isGrounded)
        {
            Vector3 downForce = new Vector3(0, gravityForce * Time.deltaTime, 0);
            transform.Translate(downForce);
            // Debug.Log("Grounded");
        }

        switch (currentState)
        {
            case EnemyState.Running: 
                RunningBehaviour();
                break;
            case EnemyState.Ragdoll:
                RagdollBehaviour();
                break;
            case EnemyState.Dash: 
                DashBehaviour(); 
                break;
        }
        HandleBehaviour();
    }

    private void DisableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = true; // stops physics from affecting the rigidbodies in the child objects like arms, legs etc
        }
        animator.enabled = true;
        characterController.enabled = true;
    }

    private void EnableRagdoll() // when die, go ragdoll
    {
        lightAnimation.Play("LightKillAnimation");

        // Add kill to killcounter
        killCounterScript.AddKill();

        // Play death sound
        source.PlayOneShot(clip);

        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = false; // enables physics  affecting the rigidbodies in the child objects like arms, legs etc
        }
        animator.enabled = false;
        characterController.enabled = false;
        this.enabled= false;
    }

    private void StartDash() // check this later: https://answers.unity.com/questions/1716253/how-to-move-towards-a-random-position-higher-than.html
    {
        Vector3 randPoint = Random.insideUnitSphere;
        randPoint.y = transform.position.y;
        dashTarget = new Vector3(transform.position.x + randPoint.x, transform.position.y, transform.position.z + randPoint.z) * dashDistance;

    }
    private void EndDash()
    {
        Debug.Log("Dash ended");
        currentState = EnemyState.Running;
    }


    private void RunningBehaviour()
    {
       

        distanceToTarget = (target.position - gameObject.transform.position).magnitude;
        if (distanceToTarget > 1)
        {
            //Vector3 targetDirection = target.position - gameObject.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), movementSpeed * Time.deltaTime);
        }

        gameObject.transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        
        //Debug.Log("Running");
        // debug for running direction
        //Debug.DrawLine(transform.position, transform.forward.normalized, new Color(1.0f, 0.0f, 0.0f));
    }

    private void RagdollBehaviour()
    {
        // Nothing for now
    }

    private void DashBehaviour()
    {
        transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
        Debug.Log("In DashBehaviour: " + dashTarget);
        //Debug.Log("Dashing");
        // play animation

        if (transform.position == dashTarget)
        {
            EndDash();
        }
        // revert to base Behaviour

    }

    public void projectileCollisionDetected(Collision collision)
    {

        Debug.Log(collision.gameObject.name);

        
        //for (int i = 0; i < ragdollRigidbodies.Length; i++)
        //{
        //    if (ragdollRigidbodies[i].gameObject == collision.gameObject)
        //    {
        //        //ragdollRigidbodies[i].gameObject.

        //        break;
        //    }
        //}

        EnableRagdoll();
        //foreach (var rigidbody in ragdollRigidbodies)
        //{
        //    if(collision.rigidbody == rigidbody)
        //    {
        //        rigidbody.AddForce(transform.forward*5);
        //    }
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        EnableRagdoll();
    }

    private void HandleBehaviour() // to keep the update function clean.
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnableRagdoll();
            currentState = EnemyState.Ragdoll;
        }

        if (Input.GetKeyDown(KeyCode.D)) // It doesnt remember the value of test2 and resets to 0 in DashBehaviour
        {
            StartDash();
            currentState = EnemyState.Dash;
        }
    }
}
