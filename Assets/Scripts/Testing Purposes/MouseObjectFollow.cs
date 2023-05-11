using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseObjectFollow : MonoBehaviour
{
    public float screenToCamRatio;  // Converts pixel measurements of mouse (usually 1920x1080) to the scene space, within the camera view
    private Vector2 startDiff;      // Makes the object able to start away from origin

    private WeaponSwingCheck check;
    private Renderer rend;

    private void Start()
    {
        check = GetComponent<WeaponSwingCheck>();
        rend = GetComponent<Renderer>();
        startDiff = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        var mousePos = Input.mousePosition;
        transform.position = new Vector3(mousePos.x*screenToCamRatio + startDiff.x, mousePos.y * screenToCamRatio + startDiff.y, transform.position.z);

        //// Change material with the use of the weapon check script
        //if (check.weaponActive)
        //{
        //    rend.material.color = new Color(0, 1, 0);
        //}
        //else
        //{
        //    rend.material.color = new Color(1, 0, 0);
        //}

    }
}
