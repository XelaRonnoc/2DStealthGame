using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float crateMoveSpeed = 0.5f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Color hiddenColor = new Color(255, 255, 255, 0.5f);
    [SerializeField] private Color visibleColor = new Color(255, 255, 255, 1);
    [SerializeField] private LayerMask bulletLayer;
    [SerializeField] public LayerMask coinLayer;
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private UIManager manager;

    private Vector3 moveDir;
    private Vector2 dir;

    private Transform playerSprite;
    private SpriteRenderer sprite;

    private float movement;
    private float angle;
    private bool isVisible;
    private int score;
    private bool inCrate;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        movement = moveSpeed;
        playerSprite = transform.Find("PlayerSprite");
        sprite = playerSprite.GetComponent<SpriteRenderer>();
        isVisible = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (inCrate)
        {
            movement = crateMoveSpeed;
        }
        else
        {
            movement = moveSpeed;
        }

        if (isVisible)
        {
            moveDir = (Vector3.right * movement * Time.deltaTime * Input.GetAxis(InputAxes.Horizontal)) + (Vector3.up * movement * Time.deltaTime * Input.GetAxis(InputAxes.Vertical));
            transform.Translate(moveDir, Space.Self);
        }

        if (Input.GetAxis(InputAxes.Horizontal) != 0 || Input.GetAxis(InputAxes.Vertical) != 0) 
        { 
            rotateSprite();
        }

        if (Input.GetButton("Jump"))
        {
            sprite.color = hiddenColor;
            movement = 0;
            isVisible = false;
        }
        if (Input.GetButtonUp("Jump"))
        {
            sprite.color = visibleColor;
            movement = moveSpeed;
            isVisible = true;
        }

    }

    private void rotateSprite() // rotates sprite direction
    {
        dir = -moveDir;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        playerSprite.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 
    }

    public bool getPlayerVisibility() // returns player visibility 
    {
        return isVisible;
    }

    public Collider2D getCollider() // returns collider for player
    {
        return this.GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (coinLayer.Contains(collision.gameObject)) // if coin pick up coin
        {
            score = score + collision.GetComponent<TreasureStats>().getValue();
                
        }
        if (bulletLayer.Contains(collision.gameObject)) // if collided with bullet call playerDestroyed
        {
            playerDestroyed();
            
        }

        if (terrainLayer.Contains(collision.gameObject)) // if in crate set that to true
        {
            inCrate = true; 
        }

        
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (terrainLayer.Contains(other.gameObject)) // on leaving crate set in crate to false
        {
            inCrate = false;
        }
    }

    public int getScore(){ // returns curret score
        return score;
    }

    public void playerDestroyed() // destroys player and displays game over panel
    {
        Destroy(this.gameObject);
        manager.gameOver();
        Time.timeScale = 0; // pause game
    }
}
