using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeKill : MonoBehaviour // for player running into guard
{
    [SerializeField] LayerMask playerLayer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerLayer.Contains(collision.gameObject))
        {
            collision.gameObject.GetComponent<PlayerMove>().playerDestroyed();
        }
    }
}
