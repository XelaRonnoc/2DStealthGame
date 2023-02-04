using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bulletContainer;

    private GuardPatrol guardPatrol;

    void Start()
    {
      guardPatrol = transform.parent.GetComponent<GuardPatrol>();
    }

    public void fireBullet()
    {
        if (transform.childCount < 1)
        {
            GameObject bullet = Instantiate(bulletContainer);
            bullet.transform.parent = transform;
            bullet.transform.localPosition = Vector3.zero;
            bullet.GetComponent<BulletMove>().setMoveDir(guardPatrol.getMoveDir());
        }
    }




}
