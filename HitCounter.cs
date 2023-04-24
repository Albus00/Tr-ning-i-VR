using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCounter : MonoBehaviour
{
                                                //Tanken
    // ta in position av object/vapen titta om den matchar motståndare kropp. Om det är kontakt så add till counter
    // sedan tittar vi varje frame om positionen ändras och fortfarande kontakt isåfall add counter
    // när vapnet inte är i kontakt längre så skicka count värdet och reseta count

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

// Tanken är då att först tittar vi i FixedUpdate() där vi tittar om det finns kontakt om det finns kontakt så ökas counter, Sedan när kontakt försvinner så slutar counter att öka. Sedan gör vi ett test om det finns ett counter värde skillt från 0 vilket betyder att vi nyss varit
// i kontakt med ett object och vi inte är i kontakt så skickar vi iväg motståndaren. Vi loggar även countern så att den kan användas som dmg. Så att vi sedan kan lägga til att efter viss dmg så dör motståndarna.
