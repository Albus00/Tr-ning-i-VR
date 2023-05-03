using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActivator : MonoBehaviour
{
    public GameObject hand;                 // Hand object that contains the swing check script.
    private WeaponSwingCheck swingCheck;    // Reference to the weapon swing check script.

    public GameObject weaponEdge;           // The part of the weapon that's supposed to glow when weapon is active. Set to this if left empty.

    public bool disableEdge = false;       // Checked if weapon edge is only supposed to be visible when weapon is active.

    public bool weaponActive = false;       // True when player swings enough for the weapon to activate
    public float threshold;                 // How far the object has to travel to activate weapon. Copied from SwingCheck for efficiency
    public float secondsActive = 0.5f;      // How long the weapon will be active (in seconds)

    // Start is called before the first frame update
    void Start()
    {
        // If part of weapon that glows has not been set: set to this.
        if (weaponEdge == null)
        {
            weaponEdge = this.gameObject;
        }

        // Check if hand reference is set
        if (hand != null)
        {
            // Get WeaponSwingCheck reference from hand
            swingCheck = hand.GetComponent<WeaponSwingCheck>();
            threshold = swingCheck.threshold;
        }
        else
        {
            Debug.LogError("Set hand reference!", this.gameObject);
        }
    }

    /// <summary>
    /// Sets timer for how long the weapon stays acive, once activated
    /// </summary>
    public IEnumerator ActiveTimer()
    {
        // Make the weapon glow
        if (disableEdge)
        {
            weaponEdge.SetActive(true);
        }
        MeshRenderer mat = weaponEdge.GetComponent<MeshRenderer>();
        mat.material.EnableKeyword("_EMISSION");
        mat.material.SetColor("_EmissionColor", Color.white);

        // Set timer until weapon is no longer active
        yield return new WaitForSeconds(secondsActive);

        if (swingCheck.travel < threshold)
        {
            // Deactivate if not enough movement is recorded
            weaponActive = false;
            mat.material.SetColor("_EmissionColor", Color.black);
            Debug.Log("WEAPON INACTIVE");
            if (disableEdge)
            {
                weaponEdge.SetActive(false);
            }
        }
        else
        {
            // Keep active if still in motion
            StartCoroutine(ActiveTimer());
        }
    }

}
