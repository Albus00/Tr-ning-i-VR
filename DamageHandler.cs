using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public float maxDamageThreshold = 1.0f; // Maximal skada som kan tas emot innan objektet f�rst�rs
    public float minDamageThreshold = 0.5f; // Minimal skada som kan tas emot f�r att det ska r�knas som en tr�ff

    private float currentDamage = 0.0f; // Aktuell skada som har p�verkat objektet

    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.impulse.magnitude / Time.fixedDeltaTime; // Ber�kna kraften i kollisionen

        if (impactForce > minDamageThreshold) // Om kraften �r tillr�ckligt h�g f�r att det ska r�knas som en tr�ff
        {
            float objectSize = collision.collider.bounds.size.magnitude; // Ber�kna storleken p� det tr�ffade objektet

            if (impactForce > objectSize) // Om kraften �r st�rre �n storleken p� objektet
            {
                currentDamage += impactForce / objectSize; // Ber�kna hur mycket skada som ska tas emot baserat p� f�rh�llandet mellan kraft och storlek

                if (currentDamage >= maxDamageThreshold) // Om objektet har tagit tillr�ckligt mycket skada f�r att f�rst�ras
                {
                    Destroy(gameObject); // F�rst�r objektet
                }
            }
        }
    }
}
