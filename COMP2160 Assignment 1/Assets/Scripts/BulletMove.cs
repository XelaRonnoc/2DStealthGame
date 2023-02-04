using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float bulletLifetime = 1;

    private Transform bulletSprite;
    private GuardPatrol guardPatrol;

    private Vector2 moveDir;
    private Vector2 dir;

    private float destroyTimer;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        guardPatrol = transform.parent.parent.GetComponent<GuardPatrol>();
        moveDir = guardPatrol.getMoveDir();
        bulletSprite = transform.Find("BulletSprite");
        destroyTimer = bulletLifetime;
        rotateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if(destroyTimer <= 0)
        {
            Destroy(gameObject);
        }
        transform.Translate(moveDir * bulletSpeed * Time.deltaTime); // move bullet in direction it was fired 
        destroyTimer = destroyTimer - Time.deltaTime;
    }

    private void rotateSprite() // rotates sprite direction without rotating player 
    {
        dir = moveDir;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bulletSprite.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (playerLayer.Contains(other.gameObject) || terrainLayer.Contains(other.gameObject)) // destory bullet if it hits crate or player
        {
            Destroy(gameObject);
        }
    }

    public void setMoveDir(Vector2 direction)
    {
        moveDir = direction;
    }
}
