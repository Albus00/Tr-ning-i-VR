using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class punching : MonoBehaviour
{

    private Vector3 _previousPosition;
    private Vector3 _positionChange;
    private int _frameCounter;
    public AudioSource thisSound;
    public AudioClip punchSound;
    void Start()
    {
        thisSound = GetComponent<AudioSource>();
        _frameCounter = 0;
        _previousPosition = transform.position;
    }
    void Update()
    {
        _frameCounter++;

        if (_frameCounter % 5 == 0)
        {
            _positionChange = transform.position - _previousPosition;
            _previousPosition = transform.position;

            // Log the position change to the console
            //Debug.Log($"Position Change: {_positionChange.magnitude}");
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag("punchable") && _positionChange.magnitude > 0.2f)
        {
            thisSound.PlayOneShot(punchSound, 0.5f);
            other.gameObject.transform.root.GetComponent<BehaviourTest>().projectileCollisionDetected(other, transform.position);
           

        }
        //Debug.Log(other.gameObject.name);


    }

}
