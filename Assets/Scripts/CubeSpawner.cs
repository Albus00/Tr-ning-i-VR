using System.Collections;
using System.Collections.Generic;
using UltimateXR.Mechanics.Weapons;
using UnityEngine;
// having a spawner prefab which spawns a projectile prefab might be dumb
public class CubeSpawner : MonoBehaviour // spawns single cube, then destroys itself
{
    private Transform target;
    public GameObject projectile; // must add projectile prefab to the spawner prefab
    private Rigidbody rb;
    public float forceAmount = 500f;
    private Vector3 spawnPosition;

    private void Start()
    {
        
        spawnPosition = gameObject.transform.position;
        
        gameObject.transform.LookAt(target); // spawner faces player
        StartCoroutine(destroySelfWithDelay());
    }
    IEnumerator destroySelfWithDelay()
    {
        yield return new WaitForSeconds(3);
        Instantiate(projectile, spawnPosition, Quaternion.identity); // instantiates projectile at the origin of the spawner with the same rotation as the spawner
        yield return new WaitForSeconds(1);
        Destroy(gameObject); // spawner destroys itself with 1s delay to avoid potential bugs
    }
}
