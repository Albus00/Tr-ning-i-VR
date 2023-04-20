using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateXR.Devices;

public class ControllerTracking : MonoBehaviour
{
    public float threshold = 0.05f; // The threshold in meters that triggers controller tracking
    public float checkInterval = 0.1f; // The interval between checking for updates in seconds
    public Transform trackedObject; // The object that should be tracked
    public Transform leftHandItem; // The item currently held in the left hand, if any
    public Transform rightHandItem; // The item currently held in the right hand, if any

    public Vector3 SensorLeftPos { get; }
    public Vector3 SensorRightPos { get; }
    //public abstract Type RelatedControllerInputType { get; }

    private Vector3 lastLeftPosition;
    private Vector3 lastRightPosition;
    private bool trackingEnabled;

    void Start()
    {
        // Save the initial controller positions (realworld pos)
        lastLeftPosition = SensorLeftPos;
        lastRightPosition = SensorRightPos;
    }

    void Update()
    {
        // Only check for updates at the specified interval
        if (Time.time % checkInterval == 0)
        {
            // Get the current controller positions
            var currentLeftPosition = SensorLeftPos;
            var currentRightPosition = SensorRightPos;

            // Calculate the distance between the current and last positions
            var leftDistance = Vector3.Distance(currentLeftPosition, lastLeftPosition);
            var rightDistance = Vector3.Distance(currentRightPosition, lastRightPosition);

            // If either distance is greater than the threshold and neither hand is holding an item, enable tracking
            if ((leftDistance > threshold || rightDistance > threshold) && !leftHandItem && !rightHandItem)
            {
                trackingEnabled = true;
            }

            // Save the current positions for the next update
            lastLeftPosition = currentLeftPosition;
            lastRightPosition = currentRightPosition;
        }

       /* // If tracking is enabled and a button is pressed, unlock the tracked object
        if (trackingEnabled && (UxrControllerTracking.Left.GetButtonUp(UXRInputButton.Grip) || UxrControllerTracking.Right.GetButtonUp(UXRInputButton.Grip)))
        {
            trackedObject.parent = null;
            trackingEnabled = false;
        }*/
        // If tracking is disabled, lock the tracked object to the current controller positions
        else if (!trackingEnabled)
        {
            trackedObject.parent = transform;
            trackedObject.localPosition = Vector3.zero;
            trackedObject.localRotation = Quaternion.identity;
        }
    }
}
