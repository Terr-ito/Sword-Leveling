using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearController : WeaponController
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedSpear = Instantiate(weaponData.Prefab);
        spawnedSpear.transform.position = transform.position; //Assign the position to be the same as this object which is parented to the player
        spawnedSpear.GetComponent<SpearBehaviour>().DirectionChecker(pm.lastMovedVector); //Assign the direction of the spear to be the same as the player
    }
}