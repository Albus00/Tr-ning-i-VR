using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public string projectileType = "box";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Contains("mixamorig"))
        {
            //Debug.Log(collision.gameObject.name);
            collision.gameObject.transform.root.GetComponent<EnemyBehaviour>().projectileCollisionDetected(collision);
        }
    }
}
