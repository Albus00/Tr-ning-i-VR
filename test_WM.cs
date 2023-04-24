using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_VM : MonoBehaviour
{
    public Transform controller; // The controller transform
    public float movementThreshold = 0.1f; // The movement threshold in meters
    private Vector3 initialPosition; // The initial position of the controller

    void Start()
    {
        initialPosition = controller.position; // Set the initial position of the controller
    }

    void Update()
    {
        // Calculate the distance between the current and initial position of the controller
        float distance = Vector3.Distance(controller.position, initialPosition);

        // Check if the distance has exceeded the movement threshold
        if (distance >= movementThreshold)
        {
            // Translate the object by the controller's forward direction
            transform.Translate(controller.forward * Time.deltaTime, Space.World);
        }
    }
}
/*
 https://www.ultimatexr.io/api/P_UltimateXR_Manipulation_UxrGrabbableObject_TranslationLimitsMin
 https://www.ultimatexr.io/guides/scripting-how-do-i
     */
