using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSwingCheck : MonoBehaviour
{
    public GameObject weapon;               // Weaponobject that contains the WeaponActivator script.
    private WeaponActivator activator;     // Reference to the WeaponActivator script.

    private List<Vector3> positions = new List<Vector3>();  // List of previous positions to compare movement
    public int maxPosInList = 20;                           // Size of the list, how many positions it keeps in memory

    public float travel = 0.0f;                            // The travel between the current position and the first (oldest) position in list
    public float threshold = 10.0f;                         // How far the object has to travel to activate weapon

    void Start()
    {
        // Check if hand reference is set
        if (weapon != null)
        {
            // Get WeaponSwingCheck reference from hand
            activator = weapon.GetComponent<WeaponActivator>();
            activator.threshold = threshold;
        }
        else
        {
            Debug.LogError("Set weapon reference!", this.gameObject);
        }

        // Add first position to the list
        positions.Add(transform.position);
    }


    private void FixedUpdate()
    {
        // Compare current position with first in list
        travel = Vector3.Distance(positions.First(), transform.position);


        if (travel > threshold)
        {
            Debug.Log("WEAPON ACTIVE");
            StartCoroutine(activator.ActiveTimer());
        }
        
        // Add current position to list
        positions.Add(transform.position);

        if (positions.Count() > maxPosInList)
        {
            // Remove oldest pos in list
            positions.RemoveAt(0);
        }
    }
}
