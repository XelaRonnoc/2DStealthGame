using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GuardBehavior))]
public class GuardPatrol : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform patrolStart;
    [SerializeField] private Transform patrolEnd;
    [SerializeField] private float moveSpeed = 1f;

    private Vector3 moveDir;
    private Vector2 dir;

    private Transform patrolTarget;
    private Transform guardSprite;
    private Transform patrolTargetSave;
    private Transform playerTransform;
    private GuardBehavior guardBehavior;

    private float minDist;
    private float movement;
    private float angle;
    private float distToTargetX;
    private float distToTargetY;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.transform;
        transform.position = patrolStart.position;
        patrolTarget = patrolEnd;
        patrolTargetSave = patrolEnd;
        movement = moveSpeed;
        guardSprite = transform.Find("GuardSprite");
        minDist = 0f;
        guardBehavior = transform.GetComponent<GuardBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (patrolTarget != null)
        {
            distToTargetX = Mathf.Abs(patrolTarget.position.x - transform.position.x); // calculates distance to target on x plane 
            distToTargetY = Mathf.Abs(patrolTarget.position.y - transform.position.y); // calculates distance to target on y plane 
        }
        // if at target object switch target object to other patrol object;
         if(distToTargetX  <= minDist && distToTargetY <= minDist)
         {
             if (patrolTarget == patrolEnd)
             {
                patrolTarget = patrolStart;
             }
             else if(patrolTarget == patrolStart)
             {
                patrolTarget = patrolEnd;
             }
        }
        if (patrolTarget != null)
        {
            // moves towards current target object
            transform.position = Vector2.MoveTowards(transform.position, patrolTarget.position, movement * Time.deltaTime);
            if (patrolTarget != playerTransform || guardBehavior.getGuardCanSee())
            {
                // point sprite in direction of travel
                moveDir = (patrolTarget.position - transform.position).normalized;
            }
        }
         rotateSprite();
    }

    private void rotateSprite() // rotates sprite direction
    {
        dir = moveDir;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        guardSprite.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void setMovement(int val) // sets movement to input val
    {
        movement = val;
    }

    public void resetMovement() // resets movement to moveSpeed
    {
        movement = moveSpeed;
    }

    public void targetPlayer () // changes target of guard to player
    {
        if (patrolTarget != playerTransform)
        {
            patrolTargetSave = patrolTarget;
        }
        patrolTarget = playerTransform;
    }

    public void resetPatrolTarget() // resets patrol target to previous patrol target
    {
        patrolTarget = patrolTargetSave;
    }

    public Vector3 getMoveDir() // returns move direction of guard
    {
        return moveDir;
    }

    public GameObject getPlayerLinked() // get player game object that was added to this script
    {
        return player;
    }
}
