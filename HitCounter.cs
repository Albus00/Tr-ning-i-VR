using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCounter : MonoBehaviour
{
                                                //Tanken
    // ta in position av object/vapen titta om den matchar motst�ndare kropp. Om det �r kontakt s� add till counter
    // sedan tittar vi varje frame om positionen �ndras och fortfarande kontakt is�fall add counter
    // n�r vapnet inte �r i kontakt l�ngre s� skicka count v�rdet och reseta count

    public string projectileType = "box";
    int Dmgcounter = 0;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        other.gameObject.transform.root.GetComponent<BehaviourTest>().projectileCollisionDetected(other, transform.position);

    }

    void FixedUpdate()
    {
        Debug.Log(other.gameObject.name);
        BehaviourTest behaviourTest = other.gameObject.transform.root.GetComponent<BehaviourTest>();

            if (behaviourTest != null)
            {
                Dmgcounter++;
            }

        Debug.Log(counter);

        if (behaviourTest = null && counter !=0 )
            {
                behaviourTest.projectileCollisionDetected(other, transform.position);
            Dmgcounter = 0;
            }

    }
}

// Tanken �r d� att f�rst tittar vi i FixedUpdate() d�r vi tittar om det finns kontakt om det finns kontakt s� �kas counter, Sedan n�r kontakt f�rsvinner s� slutar counter att �ka. Sedan g�r vi ett test om det finns ett counter v�rde skillt fr�n 0 vilket betyder att vi nyss varit
// i kontakt med ett object och vi inte �r i kontakt s� skickar vi iv�g motst�ndaren. Vi loggar �ven countern s� att den kan anv�ndas som dmg. S� att vi sedan kan l�gga til att efter viss dmg s� d�r motst�ndarna.
