using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public float maxDamageThreshold = 1.0f; // Maximal skada som kan tas emot innan objektet förstörs
    public float minDamageThreshold = 0.5f; // Minimal skada som kan tas emot för att det ska räknas som en träff

    private float currentDamage = 0.0f; // Aktuell skada som har påverkat objektet

    private void OnCollisionEnter(Collision collision)
    {
        float impactForce = collision.impulse.magnitude / Time.fixedDeltaTime; // Beräkna kraften i kollisionen

        if (impactForce > minDamageThreshold) // Om kraften är tillräckligt hög för att det ska räknas som en träff
        {
            float objectSize = collision.collider.bounds.size.magnitude; // Beräkna storleken på det träffade objektet

            if (impactForce > objectSize) // Om kraften är större än storleken på objektet
            {
                currentDamage += impactForce / objectSize; // Beräkna hur mycket skada som ska tas emot baserat på förhållandet mellan kraft och storlek

                if (currentDamage >= maxDamageThreshold) // Om objektet har tagit tillräckligt mycket skada för att förstöras
                {
                    Destroy(gameObject); // Förstör objektet
                }
            }
        }
    }
}
