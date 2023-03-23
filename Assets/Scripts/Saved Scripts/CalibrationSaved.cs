using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CalibrationSaved : MonoBehaviour
{
    [Header("Oculus Quest 2 Hands:")]
    [SerializeField]
    private GameObject interactionRigOVR;

    [SerializeField]
    private GameObject leftHandAnchor;

    [SerializeField]
    private GameObject rightHandAnchor;

    [Header("LeapMotion Hand:")]
    [SerializeField]
    private GameObject leftHandPalm;

    [SerializeField]
    private GameObject rightHandPalm;

    [Header("Other Settings:")]
    [SerializeField]
    private Vector3 palmOffset = new Vector3(0, -0.02f, 0.055f);
    //X: 0.445 <-> ; Y: -0.8551418 <-> -0.83 ; Z: -0.29 <-> -0.345 = -0.055


    // Update is called once per frame
    void Update()
    {
        // check if space pressed
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("space");
            SetOVRPos();
        }
    }

    private void SetOVRPos()
    {
        if (interactionRigOVR == null)
        {
            return;
        }

        if (leftHandPalm != null && leftHandAnchor != null && leftHandPalm.gameObject.activeSelf)
        {
            var difference = leftHandPalm.transform.position - leftHandAnchor.transform.position - palmOffset;
            Debug.Log("Left Difference: " + difference + ", OVR Pos: " + interactionRigOVR.transform.position);

            interactionRigOVR.transform.position += difference;

        } else if (rightHandPalm != null && rightHandAnchor != null && rightHandPalm.gameObject.activeSelf) 
        {
            var difference = rightHandPalm.transform.position - rightHandAnchor.transform.position;
            Debug.Log("Right Difference: " + difference + ", OVR Pos: " + interactionRigOVR.transform.position);
        }
    }
}
