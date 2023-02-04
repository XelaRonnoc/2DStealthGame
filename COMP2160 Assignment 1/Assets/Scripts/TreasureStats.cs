using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureStats : MonoBehaviour
{
    [SerializeField] private int treasureSize = 1;
    [SerializeField] private Collider2D player;

    private float size;
    private int value;
    void Start()
    {
        size = treasureSize *0.2f;
        value = (int) (10f*size);
        transform.localScale = new Vector3(transform.localScale.x * size, transform.localScale.y * size, transform.localScale.z);
    }

    public int getValue() // returns treasures value
    {
        return value;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == player) // destory self on collsion with player
        {
            Destroy(gameObject);
        }
    }
}
