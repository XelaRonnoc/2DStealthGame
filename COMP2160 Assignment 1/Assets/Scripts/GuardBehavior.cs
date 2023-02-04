using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardBehavior : MonoBehaviour
{
    [SerializeField] private float alertMultiple = 2f;
    [SerializeField] private float cautionTime = 5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask guardLayer;



    private Vector2 alertVisionCircle;
    private Vector2 patrolVisionCircle;

    private GameObject current;
    private Transform guardVision;
    private CircleCollider2D visionCollider;
    private SpriteRenderer visionCircle;
    private GuardPatrol guardPatrol;
    private Transform bulletSpawner;
    private BulletSpawner gun;
    private GameObject player;

    private float alertColliderRadius;
    private float patrolColliderRadius;
    private int guardAttackMovement;
    private float cautionTimer; // amount of time guard is in caution time
    private float timeElapsed = 0; // used for lerps
    private bool guardCanSee = false; // can guard see player
    private bool intersectingCrate = false; // checks if raycast is intersecting a crate at any point
    private bool initialChange = false; // used to determine first frame where a new state has been started
    
    enum GuardState
    {
        Patroling,
        Attacking,
        Caution
    }

    private GuardState guardState;

    // Start is called before the first frame update
    void Start()
    {

        player = transform.GetComponent<GuardPatrol>().getPlayerLinked();
        current = this.gameObject;
        guardState = GuardState.Patroling;
        guardVision = transform.Find("GuardVision");
        visionCircle = guardVision.GetComponent<SpriteRenderer>();
        visionCollider = GetComponent<CircleCollider2D>();
        patrolColliderRadius = visionCollider.radius;
        patrolVisionCircle = (visionCircle.transform.localScale);
        alertColliderRadius = visionCollider.radius * alertMultiple;
        alertVisionCircle = visionCircle.transform.localScale * alertMultiple;
        visionCircle.color = Color.green;
        guardPatrol = current.GetComponent<GuardPatrol>();
        bulletSpawner = current.transform.Find("bulletSpawner");
        gun = bulletSpawner.GetComponent<BulletSpawner>();
        cautionTimer = cautionTime;
        guardAttackMovement = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (player != null) // player not destroyed
        {
             advancedIntersectCrate(); // raycast

            switch (guardState)
            {
                case GuardState.Patroling:
                    cautionTimer = cautionTime;
                    timeElapsed = 0;

                    if (initialChange)
                    {
                        guardPatrol.resetPatrolTarget(); // sets patrol target to either start or end depending on previous target saved
                        guardPatrol.resetMovement(); // sets movment back to appropriate value
                        visionCircle.gameObject.transform.localScale = patrolVisionCircle; // changes vision circle spirte back to patrol radius
                        visionCollider.radius = patrolColliderRadius; // changes vision collider back to patrol radius
                        visionCircle.color = Color.green; 
                        initialChange = false;
                    }
                    
                    break;

                case GuardState.Attacking:
                    cautionTimer = cautionTime;
                    timeElapsed = 0;
                    if (!player.GetComponent<PlayerMove>().getPlayerVisibility()) // if player is not visible switch to caution state
                    {
                        initialChange = true;
                        guardState = GuardState.Caution;
                    }
                    if (initialChange)
                    {
                        guardPatrol.targetPlayer(); // makle player patrol target
                        guardPatrol.setMovement(guardAttackMovement); // set movement to guard attacking movement speed
                        visionCircle.color = Color.red;
                        visionCircle.gameObject.transform.localScale = alertVisionCircle; // changes vision circle to bigger vision circle
                        visionCollider.radius = alertColliderRadius; // changes vision collider to bigger vision collider
                        initialChange = false;
                    }
                    else
                    {
                        gun.fireBullet();
                    }
                    break;

                case GuardState.Caution:

                    if (cautionTimer <= 0)
                    {
                        initialChange = true;
                        guardState = GuardState.Patroling;
                    }
                    else
                    {
                        guardPatrol.targetPlayer(); // make player patrol target
                        guardPatrol.setMovement(guardAttackMovement); // set movement to guard attacking movement speed
                        visionCircle.color = Color.yellow;
                        visionCircle.gameObject.transform.localScale = Vector2.Lerp(alertVisionCircle, patrolVisionCircle, timeElapsed / cautionTime); // lerp between attack vision circle and patrol vision circle over time
                        visionCollider.radius = Mathf.Lerp(alertColliderRadius, patrolColliderRadius, timeElapsed / cautionTime); // lerp between attack vision collider and patrol vision collider over time
                        cautionTimer -= Time.deltaTime;
                    }
                    timeElapsed += Time.deltaTime;
                    break;

            }

        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (playerLayer.Contains(other.gameObject) && guardCanSee) // if other object is available and guard can see
        {
            if (player.GetComponent<PlayerMove>().getPlayerVisibility()) // if player is not invisible
            {
                initialChange = true;
                guardState = GuardState.Attacking;
            }
            else if(!player.GetComponent<PlayerMove>().getPlayerVisibility() && guardState != GuardState.Patroling && cautionTimer <=0) // if player is invisible and guard state is not patroling and caution timer has run out (i.e. in caution or attack state)
            {
                initialChange = true;
                guardState = GuardState.Patroling;
            }
            else if(guardState != GuardState.Patroling) // guard state caution if guard not already in patroling state (i.e. were in attacking state or caution state)
            {
                
                guardState = GuardState.Caution; // doesn't set inital changes as not always initial change here
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // if it's the player exiting collider and they are visible and guard can see
        if (playerLayer.Contains(other.gameObject) && guardCanSee && player.GetComponent<PlayerMove>().getPlayerVisibility())
        {
            if (guardState != GuardState.Caution)
            {
                
                guardState = GuardState.Caution;
            }
        }

    }

    private void advancedIntersectCrate() // ray casting from guard 
    {

        RaycastHit2D[] lineOfSight; // creates an array of all hits raycast does
        lineOfSight = Physics2D.RaycastAll(transform.position, player.transform.position - transform.position);
        foreach (var hit in lineOfSight)
        {
            if (terrainLayer.Contains(hit.collider.gameObject)) // if crate is between guard and player
            {
                intersectingCrate = true;
                break;
            }
            else if (playerLayer.Contains(hit.collider.gameObject))
            {
                intersectingCrate = false;
                break; // this means if it hits player first before crate it won't set intersectingCrate to true if player is in front of crate
            }
            else
            {
                intersectingCrate = false;
            }
        }

        if (intersectingCrate)
        {
            guardCanSee = false;
            if (guardState == GuardState.Attacking)
            {
                guardState = GuardState.Caution;
            }
        }
        else
        {
            guardCanSee = true;
        }
    }

    public bool getGuardCanSee()
    {
        return guardCanSee;
    }

}


