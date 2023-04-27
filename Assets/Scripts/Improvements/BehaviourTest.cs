using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
// Make an enemyHandler class that delegates actions to enemies. 
// example: 
// loop through list of alive enemies => give each enemy an action based on how many enemies can do the same action at the same time
// if 10 enemies alive -> 6 move, 4 attack
// if close to player then attack = melee, if far away then attack = throw.
// this action can be called every X beats
// How to make it so that all enemies dont perform actions at the same time?
// enemy1 does action on beat X+1
// enemy2 does action on beat X+3 etc...
public class BehaviourTest : MonoBehaviour
{
    // PS: remember to set BPM based on the song. 
    // --------- Enemy State --------- //
    private enum EnemyState
    {
        Idle,
        Running,
        Dash,
        Ragdoll,
        Attack,
        CloseAttack,
        FridgeAttack
    }

    private EnemyState currentState = EnemyState.Idle;
    
    public HealthManager playerHealth;

    private bool isDead;
    private bool actionInProgress;
    // --------- Limb Handling --------- //
    private Rigidbody[] ragdollRigidbodies;
    private Collider[] colliders;
    private Rigidbody hitRigidbody;
    

    // --------- Physics --------- //
    //private float gravityForce = -9.82f;
    //public bool isGrounded = false;
    //public float groundedLength = 0.2f;

    // --------- Movement handling --------- //
    private Transform target;
    private float distanceToTarget;
    public float movementSpeed;
    // ---- Dashing ---- //
    public float dashDistance;
    public float dashSpeed;
    Vector3 dashTarget;
    public float dashVariationFactor;

    // ---- Attacking ---- //
    public float attackDistance = 1.0f;
    private bool isFridgeThrower;
    private bool hasThrownFridge;
    public float fridgeThrowDistance;
    public Transform fridgeSpawnPoint;


    // --------- Components --------- //
    private Animator animator;
    private EnemyHandler enemyHandler;
    public AudioClip clip;
    KillCounter killCounterScript;
    public AudioSource source;
    public GameObject fridgePrefab;
    private GameObject fridge;

    // --------- Action Timing --------- //
    public float bpm;
    private float secPerBeat;
    public float beatsPerAction; // decides how many beats have to occur for an action to be allowed
    private float timer;
    private bool doAction; // is set to true -> do something -> is set to false

 
    

    void Awake()
    {
        enemyHandler = GameObject.FindWithTag("EnemyHandler").GetComponent<EnemyHandler>();
        playerHealth = GameObject.FindWithTag("HealthHandler").GetComponent<HealthManager>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        DisableRagdoll(); // No ragdoll as long as enemy isnt dead.
    }
    private void Start()
    {
        fridgeThrowDistance = 15f;
        hasThrownFridge = false;
        isFridgeThrower = false;
        killCounterScript = GameObject.Find("KCO").GetComponent<KillCounter>();
        isDead = false;
        actionInProgress = false;
        bpm = 110f;
        secPerBeat = 60f / bpm;
        beatsPerAction = 1f;
        //firstCall = true;
        movementSpeed = 2f;
        dashDistance = 1.5f;
        dashSpeed = 15f;
    }

    // Use boolean flags and coroutines instead of the state handler. so a bool for dash. one for attack and so on.
    void Update()
    {
       
        switch (currentState)
        {
            case EnemyState.Idle:
                IdleBehaviour();
                break;
            case EnemyState.Running:
                RunningBehaviour();
                break;
            case EnemyState.Ragdoll:
                RagdollBehaviour();
                break;
            case EnemyState.Dash:
                DashBehaviour();
                break;
            case EnemyState.Attack:
                AttackBehaviour();
                break;
            case EnemyState.CloseAttack:
                CloseAttackBehaviour();
                break;
            case EnemyState.FridgeAttack:
                FridgeAttackBehaviour();
                break;
        }
        HandleBehaviour();
        if(!isDead) {
            GroundCheck(); 
        }
        
    }
    // ------------------------------------ IDLE ------------------------------------ //

    private void IdleBehaviour()
    {
        gameObject.transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
    }


    // ------------------------------------ RUNNING ------------------------------------ //
    private void StartRunning()
    {
        animator.SetBool("dash", true);
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


    // ------------------------------------ RAGDOLL ------------------------------------ //
    private void RagdollBehaviour()
    {
        // Nothing for now
    }
    private void DisableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = true; // stops physics from affecting the rigidbodies in the child objects like arms, legs etc
        }
        foreach (var collider in colliders)
        {
            collider.isTrigger = true;
        }
        animator.enabled = true;
        
    }
    private void EnableRagdoll() // when die, go ragdoll
    {
        
        isDead = true;
        foreach (var collider in colliders)
        {
            collider.isTrigger = false;
        }
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = false; // enables physics  affecting the rigidbodies in the child objects like arms, legs etc
        }
        animator.enabled = false;
        enemyHandler.RemoveEnemy(this.gameObject);
        this.enabled= false;
        Object.Destroy(gameObject,3f);
    }


    // ------------------------------------ DASHING ------------------------------------ //
    // check this later: https://answers.unity.com/questions/1716253/how-to-move-towards-a-random-position-higher-than.html

    private void StartDash()
    {
        
        actionInProgress = true;
        animator.SetBool("dash",true);
        // Debug.Log("Started dash");
        
        Vector3 direction = (target.position - transform.position).normalized;

        // Add random variation in the X and Z directions
        Vector3 randomVariation = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        dashVariationFactor = 0.3f; // Adjust this value to control the amount of variation

        Vector3 newDirection = Vector3.Lerp(direction, randomVariation, dashVariationFactor);
        float dotProduct = Vector3.Dot(direction, newDirection);

        // If the dot product is negative, it means the new direction is pointing away from the player. sets it to -itself to always dash towards the player.
        if (dotProduct < 0)
        {
            newDirection = Vector3.Lerp(direction, -randomVariation, dashVariationFactor);
        }

        dashTarget = transform.position + newDirection * dashDistance;
        dashTarget.y = transform.position.y;
        //gameObject.transform.LookAt(dashTarget);

        //animator.SetTrigger("dash");
        //Instantiate(testBox, dashTarget, Quaternion.identity); // instantiates a box for debugging purposes
    }

    private void DashBehaviour()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
        //Debug.Log("In DashBehaviour: " + dashTarget);
        //Debug.Log("Dashing");
        // play animation

        if (transform.position == dashTarget)
        {
            EndDash();
        }
        // revert to base Behaviour

    }
    private void EndDash()
    {
        //Debug.Log("Dash ended");
        animator.SetBool("dash", false);
        EndAction();
    }

    // ------------------------------------ ATTACKING ------------------------------------ //

    // MOCAP: https://www.youtube.com/watch?v=6lBkgr-asLs&t=119s
    private void AttackBehaviour()
    {
        // Check if the attack animation has reached the cutoff point (assuming it's on layer 0)
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("CloseAttack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.94f)
        {
            EndAttack();
        }
        //transform.LookAt(target);

    }
    private void StartAttack()
    {

    }
    private void EndAttack()
    {
        EndAction();
    }


    private void StartCloseAttack()
    {
        actionInProgress = true;
        animator.SetBool("closeAttack", true);
        //animator.SetLayerWeight(1, 1);
        // Debug.Log("Started attack");

        //-------DEBUG REMOVE HEALTH -- SHOULD BE DONE WITH A HITBOX CHECK!-------
        playerHealth.subtractHealth(20.0f);

    }
    private void CloseAttackBehaviour()
    {
        // Check if the attack animation has reached the cutoff point (assuming it's on layer 0)
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("CloseAttack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
        {
            EndCloseAttack();
        }
        //transform.LookAt(target);
    }
    private void EndCloseAttack()
    {
        //animator.SetLayerWeight(1, 0);
        EndAction();
    }

    private void StartFridgeAttack()
    {
        actionInProgress = true;
        animator.SetBool("throwFridge", true);
    }
    private void FridgeAttackBehaviour() {
       
    }
    private void EndFridgeAttack()
    {
        EndAction();
    }
    public void SpawnFridge()
    {
        fridge = Instantiate(fridgePrefab, fridgeSpawnPoint.position, fridgeSpawnPoint.rotation);
        fridge.transform.parent = fridgeSpawnPoint;
    }
    public void ReleaseFridge()
    {
        fridge.transform.parent = null;
        fridge.GetComponent<EnemyProjectile>().CommandToMoveReceiver();
    }

    // ------------------------------------ TAKING DAMAGE ------------------------------------ //
    public void projectileCollisionDetected(Collider limbCollider, Vector3 projectilePosition)
    {
        hitRigidbody = limbCollider.attachedRigidbody;

        if (!isDead) {
            source.PlayOneShot(clip);
            killCounterScript.AddKill();
            isDead = true;

            EnableRagdoll();
            currentState = EnemyState.Ragdoll;
        }
        // Find the rigidbody that corresponds to the limb collider
        
        

        if (hitRigidbody != null) // put this inside the if(!isDead)
        {
            // Enable the rigidbody and collider components
            hitRigidbody.isKinematic = false;
            limbCollider.enabled = true;

            // Apply a force to the rigidbody in the direction of the projectile
            Vector3 forceDirection = limbCollider.transform.position - projectilePosition;
            float forceMagnitude = 3000f; // Adjust this value to control the strength of the force
            hitRigidbody.AddForce(forceDirection.normalized * forceMagnitude);
        }

    }


    private void HandleBehaviour()
    {
       
        timer += Time.deltaTime; // keeps time
        if (timer >= secPerBeat * beatsPerAction) // Action on beat through BPM
        {

            if (doAction && !actionInProgress) // DoAction is flipped in BeatReceiver() which is called from the "soundDetection" script
            {
                AssignFridgeThrower(); // Remove Later
                timer = 0f;
                doAction = false;
                distanceToTarget = (target.position - gameObject.transform.position).magnitude;
                //  if (distanceToTarget <= attackDistance && !hasDoneJumpAttack) 
                if (distanceToTarget <= attackDistance) 
                {
                    // Attack when within attackDistance
                    StartCloseAttack();
                    currentState = EnemyState.Attack;
                }
                else if (distanceToTarget > attackDistance && distanceToTarget <= fridgeThrowDistance && isFridgeThrower && !hasThrownFridge)
                {
                    StartFridgeAttack();
                    currentState = EnemyState.FridgeAttack;
                }
                else
                {
                    // Dash otherwise
                    StartDash();
                    currentState = EnemyState.Dash;
                }
            }
        }
    }
    private void EndAction()
    {
        actionInProgress = false;
        currentState = EnemyState.Idle;
        
    }

    private void GroundCheck()
    {
    if(currentState != EnemyState.FridgeAttack)
        {
            RaycastHit hit;
            float groundCheckDistance = 0.1f;
            float minHeightAboveGround = 0.01f;
            Vector3 raycastStartPoint = new Vector3(transform.position.x, transform.position.y + groundCheckDistance, transform.position.z);

            if (Physics.Raycast(raycastStartPoint, Vector3.down, out hit, groundCheckDistance))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    float newYPosition = hit.point.y + minHeightAboveGround;
                    transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
                }
            }
        }
    }
    

    //BeatReceiver() needs to stay, cant be moved to EnemyHandler.
    public void BeatReceiver() // turns doAction to true when the soundDetection script detects an amplitude spike at selected frequency band.
    {
        doAction = true;
    }
    public bool GetActionInProgress() { return actionInProgress; }

    public bool GetIsFridgeThrower() { return isFridgeThrower; }
    public void AssignFridgeThrower() {
        isFridgeThrower = true;
        
        //Instantiate(fridge, )

    } // maybe a dumb way of doing it

}
