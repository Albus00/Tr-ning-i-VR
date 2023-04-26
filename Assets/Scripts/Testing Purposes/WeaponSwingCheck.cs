using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSwingCheck : MonoBehaviour
{
    public bool weaponActive = false;                       // True when player swings enough for the weapon to activate
    private List<Vector3> positions = new List<Vector3>();  // List of previous positions to compare movement
    public int maxPosInList = 20;                            // Size of the list, how many positions it keeps in memory

    private float travel = 0.0f;                            // The travel between the current position and the first (oldest) position in list
    public float threshold = 10.0f;                         // How far the object has to travel to activate weapon
    public float secondsActive = 0.5f;                      // How long the weapon will be active (in seconds)

    void Start()
    {
        // Add first position to the list
        positions.Add(transform.position);
    }


    private void FixedUpdate()
    {
        // Compare current position with first in list
        travel = Vector3.Distance(positions.First(), transform.position);


        // Only run distance check if weapon is not active
        if (!weaponActive)
        {
            if (travel > threshold)
            {
                weaponActive = true;
                Debug.Log("Weapon is active");
                StartCoroutine(ActiveTimer());
            }
        }

        // Add current position to list
        positions.Add(transform.position);

        if (positions.Count() > maxPosInList)
        {
            // Remove oldest pos in list
            positions.RemoveAt(0);
        }
    }

    /// <summary>
    /// Sets timer for how long the weapon stays acive, once activated
    /// </summary>
    IEnumerator ActiveTimer()
    {
        // Set timer until weapon is no longer active
        yield return new WaitForSeconds(secondsActive);

        if (travel < threshold)
        {
            // Deactivate if not enough movement is recorded
            weaponActive = false;
        }
        else
        {
            // Keep active if still in motion
            StartCoroutine(ActiveTimer());
        }
    }
}
