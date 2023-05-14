using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

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
    
    // --------- Sound Handling --------- //
    //--Throwing--//
    public AudioClip[] throwSounds;
    private AudioClip lastAudioClip;
    private AudioSource thisSound;
    
    //--Punching--//
    public AudioClip[] punchSounds;
    private AudioClip lastPunchSound;

    //--Getting Hit--/
    public AudioClip[] enemyHitSounds;
    public AudioClip lastHitSound;

    // --------- Physics --------- //
    //private float gravityForce = -9.82f;
    //public bool isGrounded = false;
    //public float groundedLength = 0.2f;

    // --------- Movement handling --------- //
    public Transform target;
    private float distanceToTarget;
    private float movementSpeed;
    private bool canRun;

    // ---- Dashing ---- //
    private bool canDash;
    public float dashDistance;
    public float dashSpeed;
    Vector3 dashTarget;
    public float dashVariationFactor;
    public float dashTargetLimit; // how close the enemy can dash to the player (if within this distance the dash is ended)

    // ---- Attacking ---- //
    private float attackDistance;
    private bool isFridgeThrower;
    private bool hasThrownFridge;
    public float fridgeThrowDistance;
    public Transform fridgeSpawnPoint;

    // ---- Dying ---- //
    private int _frameCounter;
    private Vector3 _previousPosition;
    private Vector3 _positionChange;

    // --------- Components --------- //
    private Animator animator;
    private EnemyHandler enemyHandler;
    public AudioClip clip;
    public KillCounter killCounterScript;
    public AudioSource source;
    public GameObject fridgePrefab;
    private GameObject fridge;

    // --------- Action Timing --------- //
    public float bpm;
    private float secPerBeat;
    public float beatsPerAction; // decides how many beats have to occur for an action to be allowed
    private float timer;
    private float localTimer;
    private bool doAction; // is set to true -> do something -> is set to false

    // --------- Difficulty --------- //
    public DifficultyManager difficultyManager; //controls damage taken by enemies in close range
    private int difficulty = 0;
    private int gameification = 0;
    private float enemyDamage = 0;

    void Awake()
    {
        isFridgeThrower = false;
        enemyHandler = GameObject.FindWithTag("EnemyHandler").GetComponent<EnemyHandler>();
        playerHealth = GameObject.FindWithTag("HealthHandler").GetComponent<HealthManager>();
        thisSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        DisableRagdoll(); // No ragdoll as long as enemy isnt dead.
    }
    private void Start()
    {
        canRun = true;
        dashTargetLimit = 2f;
        canDash = true;
        fridgeThrowDistance = 13f;
        hasThrownFridge = false;
        attackDistance = 1.1f;
        killCounterScript = GameObject.FindWithTag("killCounter").GetComponent<KillCounter>();
        isDead = false;
        actionInProgress = false;
        bpm = GameObject.FindWithTag("music").GetComponent<soundDetection>().BPM;
        secPerBeat = 60f / bpm;
        beatsPerAction = 1f;
        //firstCall = true;
        movementSpeed = 12f;
        dashDistance = 2f;
        dashSpeed = 12f;

        difficultyManager = GameObject.Find("DifficultyManager").GetComponent<DifficultyManager>();
        difficulty = difficultyManager.difficulty;
        gameification = difficultyManager.gameification;

        enemyDamage = 2f + 1f*(float)difficulty;
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
        actionInProgress = true;
        animator.SetBool("dash", true);
    }
    private void RunningBehaviour()
    {
        distanceToTarget = (target.position - gameObject.transform.position).magnitude;
        gameObject.transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), movementSpeed * Time.deltaTime);
        if (distanceToTarget <= attackDistance)
        {
            EndRunning();
        }
      


        
        //Debug.Log("Running");
        // debug for running direction
        //Debug.DrawLine(transform.position, transform.forward.normalized, new Color(1.0f, 0.0f, 0.0f));
    }
    private void EndRunning()
    {
        canRun= false; // stop trying to run when enemy has reached player
        animator.SetBool("dash", false);
        EndAction();
    }


    // ------------------------------------ RAGDOLL ------------------------------------ //
    private void RagdollBehaviour()
    {
        
        _frameCounter++;

        if (_frameCounter % 5 == 0) // compare position every 5 frames
        {
            _positionChange = transform.position - _previousPosition;
            _previousPosition = transform.position;
        }
        if(_positionChange.magnitude < 0.001f) // if the enemy is dead and stops moving, start despawn timer
        {
            localTimer += Time.deltaTime;
            if (localTimer > 5f) // despawn timer = 5s
            {
                this.enabled = false;
                Object.Destroy(gameObject, 3f);
            }
        }
        else // if movement stops, reset despawn timer
        {
            localTimer = 0f;
        }
        
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
        EndAction();
        while (thisSound.clip == lastHitSound) { //dont play the same sound twice
            thisSound.clip = enemyHitSounds[Random.Range(0, enemyHitSounds.Length)];
        }
        lastHitSound = thisSound.clip;
        thisSound.PlayOneShot(thisSound.clip, 1.0f);
        localTimer = 0; // when ragdoll is enabled, initialize despawn timer
        _frameCounter = 0; // also initialize frame counter which compares positional change
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
    }


    // ------------------------------------ DASHING ------------------------------------ //
    // check this later: https://answers.unity.com/questions/1716253/how-to-move-towards-a-random-position-higher-than.html

    private void StartDash()
    {
        actionInProgress = true;
        Vector3 direction = (target.position - transform.position).normalized;
        animator.SetBool("dash",true);
        //float distance = (target.position - transform.position).magnitude;
        // Add random variation in the X and Z directions
        Vector3 randomVariation = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        dashVariationFactor = 0.7f; // Adjust this value to control the amount of variation
        Vector3 newDirection = Vector3.Lerp(direction, randomVariation, dashVariationFactor);
        float dotProduct = Vector3.Dot(direction, newDirection);
        // If the dot product is negative, it means the new direction is pointing away from the player. sets it to -itself to always dash towards the player.
        if (dotProduct < 0)
        {
            newDirection = Vector3.Lerp(direction, -randomVariation, dashVariationFactor);
        }
        dashTarget = transform.position + newDirection * dashDistance;
        dashTarget.y = transform.position.y;
        //Instantiate(testBox, dashTarget, Quaternion.identity); // instantiates a box for debugging purposes
    }

    private void DashBehaviour()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
        //Debug.Log("In DashBehaviour: " + dashTarget);
        //Debug.Log("Dashing");
        // play animation
        if ((target.position - transform.position).magnitude <= dashTargetLimit) // hacky bugfix, stop dashing through player and stop trying to dash instead of attacking
        {
            canDash = false;
            EndDash();
        }
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
        transform.LookAt(target);
        animator.SetBool("closeAttack", true);
        animator.applyRootMotion= false; // this + GroundCheck() in CloseAttackBehaviour() fixed the sliding away issue

    }
    private void CloseAttackBehaviour()
    {
        GroundCheck();
        float distance = (target.position - gameObject.transform.position).magnitude;
        if(distance > attackDistance) // keeps enemies from sliding away when attacking
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), movementSpeed * Time.deltaTime);
        }
        // Check if the attack animation has reached the cutoff point (assuming it's on layer 0)

    }
    private void EndCloseAttack() //called from animation
    {
        Debug.Log("ended close attack");
        animator.SetBool("closeAttack", false);
        StartCloseAttack();
    }
    private void giveDamage()
    {
        //-------Makes the player take damage-------//
        playerHealth.subtractHealth(enemyDamage);

        while (thisSound.clip == lastPunchSound) { //dont play the same sound twice
            thisSound.clip = punchSounds[Random.Range(0, punchSounds.Length)];
        }
        lastPunchSound = thisSound.clip;

        thisSound.PlayOneShot(thisSound.clip, 0.6f);
    }

    private void StartFridgeAttack()
    {
        actionInProgress = true;
        hasThrownFridge = true;
        animator.SetBool("throwFridge", true);
    }
    private void FridgeAttackBehaviour() {
       
    }
    private void EndFridgeAttack()
    {
        Debug.Log("ended fridge attack");
        animator.SetBool("throwFridge", false);
        EndAction();
    }
    public void SpawnFridge()
    {
        fridge = Instantiate(fridgePrefab, fridgeSpawnPoint.position, fridgeSpawnPoint.rotation);
        fridge.transform.parent = fridgeSpawnPoint;
        
        while (thisSound.clip == lastAudioClip) { //dont play the same sound twice
            thisSound.clip = throwSounds[Random.Range(0, throwSounds.Length)];
        }
        lastAudioClip = thisSound.clip;
        
        StartCoroutine(playSoundDelayed(thisSound.clip, 0.6f));
    }

    IEnumerator playSoundDelayed(AudioClip theAudio, float delay){
        yield return new WaitForSeconds(delay);
        thisSound.PlayOneShot(theAudio);
    }

    public void ReleaseFridge()
    {
        hasThrownFridge = true;
        fridge.transform.parent = null;
        fridge.GetComponent<EnemyProjectile>().CommandToMoveReceiver();
    }

    // ------------------------------------ TAKING DAMAGE ------------------------------------ //
    public void projectileCollisionDetected(Collider limbCollider, Vector3 projectilePosition, float _positionChange)
    {
        hitRigidbody = limbCollider.attachedRigidbody;

        //if (limbCollider.transform.tag == "Weapon")
        //{
        //    // Get WeaponActivator script
        //    WeaponActivator activator = limbCollider.transform.GetComponent<WeaponActivator>();
        //    if (activator.weaponActive)
        //    {
        //        Debug.Log("ded");
        //    }
        //}

        if (!isDead) {
            source.PlayOneShot(clip);
            killCounterScript.AddKill();
            isDead = true;

            EnableRagdoll();
            currentState = EnemyState.Ragdoll;
            if (hitRigidbody != null) // put this inside the if(!isDead)
            {
                // Enable the rigidbody and collider components
                hitRigidbody.isKinematic = false;
                limbCollider.enabled = true;

                // Apply a force to the rigidbody in the direction of the projectile
                Vector3 forceDirection = limbCollider.transform.position - projectilePosition;
                float forceMagnitude = _positionChange*100000f; // Adjust this value to control the strength of the force
                hitRigidbody.AddForce(forceDirection.normalized * forceMagnitude);
            }
        }
        // Find the rigidbody that corresponds to the limb collider
        
        

        

    }


    private void HandleBehaviour()
    {
        timer += Time.deltaTime; // keeps time
        if (timer >= secPerBeat * beatsPerAction) // Action on beat through BPM
        {

            if (doAction && !actionInProgress && !isDead) // DoAction is flipped in BeatReceiver() which is called from the "soundDetection" script
            {

                timer = 0f;
                doAction = false;
                distanceToTarget = (target.position - gameObject.transform.position).magnitude;
                //  if (distanceToTarget <= attackDistance && !hasDoneJumpAttack) 
                if (distanceToTarget <= attackDistance+0.3f && !canRun) 
                {
                    // Attack when within attackDistance
                    StartCloseAttack();
                    currentState = EnemyState.Attack;
                }
                else if (distanceToTarget > 5f && distanceToTarget <= fridgeThrowDistance && isFridgeThrower && !hasThrownFridge)
                {
                    Debug.Log("started fridge");
                    StartFridgeAttack();
                    currentState = EnemyState.FridgeAttack;
                }
                else if(canDash && distanceToTarget > 4f)
                {
                    // Dash otherwise
                    StartDash();
                    currentState = EnemyState.Dash;
                }
                else if(distanceToTarget <= 4f && canRun) // if within 4 meters, run straight to 'attackDistance' meters away from player (1.1m)
                {
                    StartRunning();
                    currentState = EnemyState.Running;
                }
                else
                {
                    currentState = EnemyState.Idle;
                }
            }
        }
    }
    private void EndAction()
    {
        actionInProgress = false;
        currentState = EnemyState.Idle;
        // make a call to enemyhandler with the distance from the enemy to the player so it knows if the enemy can move in to attack depending on if there are other enemies already there.
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

    public void setBPM(int bpm2)
    {
        bpm = bpm2;
    }

    // TODO: soundDetection should call doAction in enemyHandler which calls doAction in all the enemies instead of soundDetection calling doAction directly in the enemies.
    // this means enemyHandler can act as a switch for how many enemies can perform actions at the same time. (control the pace of the game better)
    public void BeatReceiver() // turns doAction to true when the soundDetection script detects an amplitude spike at selected frequency band.
    {
        doAction = true;
    }
    public bool GetActionInProgress() { return actionInProgress; } //not in use (i think)

    public bool GetIsFridgeThrower() { return isFridgeThrower; } //not in use (i think)
    public void AssignFridgeThrower() {
        //Debug.Log("received command");
        isFridgeThrower = true;


    }

}
