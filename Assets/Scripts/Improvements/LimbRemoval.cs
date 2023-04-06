using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbRemoval : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    //private GameObject bodyPart;
    // private Rigidbody rigidbodyCopy;
    
    void Start()
    {
        meshRenderer= GetComponent<MeshRenderer>();

    }

    public void Remove()
    {
        meshRenderer.enabled=false;
    }
   
}
