using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using System;

public class HandleSensation : MonoBehaviour
{
    public CollisionToSensation collisionToSensation;

    #region  Old variables
    public RigidHand leftRigidHand;
    public RigidHand rightRigidHand;

    public float minXY = -0.083f;
    public float maxXY = 0.083f;
    #endregion

    [NonSerialized] public List<GameObject> activeTriggerObjects = new List<GameObject>();
    private Vector3 _comparePoint = new Vector3(0, 0, 0);
    private Vector3[] sensationPoints = new[] {
                new Vector3(0,0,0),
                new Vector3(0,0,0),
                new Vector3(0,0,0),
                new Vector3(0,0,0),
                new Vector3(0,0,0),
                new Vector3(0,0,0),
            };

    private void Update()
    {
        Debug.Log("Objects in List: " + activeTriggerObjects.Count);

        SortByDistance();

        UpdateSolidSensation();

        UpdateSteamSensation();

        collisionToSensation.SetPath(sensationPoints);
    }

    private void SortByDistance()
    {
        activeTriggerObjects.Sort(CompareDistanceToPoint);
    }

    private int CompareDistanceToPoint(GameObject a, GameObject b)
    {
        // check if it is a fingertip bone
        //   true: then use the very tip
        //   false: use the middle point
        float squaredRangeA = (a.name == "bone3") ? (calculateNewPositionForTip(a) - _comparePoint).sqrMagnitude : (a.transform.position - _comparePoint).sqrMagnitude;
        float squaredRangeB = (b.name == "bone3") ? (calculateNewPositionForTip(b) - _comparePoint).sqrMagnitude : (b.transform.position - _comparePoint).sqrMagnitude;
        return squaredRangeA.CompareTo(squaredRangeB);
    }

    private Vector3 calculateNewPositionForTip(GameObject obj)
    {
        CapsuleCollider capsule = obj.GetComponent<CapsuleCollider>();
        if (capsule != null)
        {
            // Update Position
            return obj.transform.position + new Vector3(0f, capsule.height / 2f) + capsule.center;
        }

        return new Vector3(0, 0, 0);
    }

    private void UpdateSolidSensation()
    {
        int counter = 1;

        for (int i = 0; i < 6; ++i)
        {
            // there are collision points
            if (activeTriggerObjects.Count > 0)
            {
                if (activeTriggerObjects.Count > i)
                {
                    sensationPoints[i] = (activeTriggerObjects[i].name == "bone3") ? calculateNewPositionForTip(activeTriggerObjects[i]) : activeTriggerObjects[i].transform.position;
                }
                else
                {
                    sensationPoints[i] = sensationPoints[i - counter];
                    counter++;
                }
            }
            // there are NO collision points
            else
            {
                sensationPoints[i] = new Vector3(0, 0, 0);
            }
        }
    }

    private void UpdateSteamSensation()
    {
        var zeroVec = new Vector3(0, 0, 0);

        for (int i = 0; i < 6; ++i)
        {
            if (sensationPoints[i] != zeroVec)
            {
                sensationPoints[i] = ApplyNoise(sensationPoints[i]);
            }
        }
    }

    public float minNoise = -0.005f;
    public float maxNoise = 0.005f;
    private Vector3 ApplyNoise(Vector3 point)
    {
        Vector3 result = point;
        result.x += GetRandomNoiseVal();
        result.y += GetRandomNoiseVal();
        result.z += GetRandomNoiseVal();
        
        return result;
    }

    private float GetRandomNoiseVal()
    {
        return UnityEngine.Random.Range(minNoise, maxNoise);
    }

    // --------------------------------------- older Methods ------------------------------------

    private void IterateOverBones(RigidHand hand)
    {
        // Iterate through the fingers
        for (int f = 0; f < hand.fingers.Length; ++f)
        {
            var finger = hand.fingers[f];

            if (finger != null)
            {
                // Iterate through bones of a finger
                for (int i = 0; i < finger.bones.Length; ++i)
                {
                    if (finger.bones[i] != null)
                    {
                        // Set bone dimensions.
                        CapsuleCollider capsule = finger.bones[i].GetComponent<CapsuleCollider>();

                    }
                }
            }
        }
    }

    private Vector3 RaycastForFingers(GameObject finger, Collider other)
    {
        RaycastHit hit;
        if (Physics.Raycast(finger.transform.position, finger.transform.forward, out hit))
        {
            var hitpoint = hit.point;
            Debug.Log(finger + ", Point of contact: " + hitpoint);

            var closestPoint = other.ClosestPoint(hit.point);
            Debug.Log(finger + ", Closest Point: " + closestPoint);

            var result = new Vector3(Mathf.Clamp(closestPoint.x, minXY, maxXY), Mathf.Clamp(closestPoint.y, minXY, maxXY), 0);
            //Debug.Log("Result Point: " + result);

            return result;
        }
        return new Vector3(0, 0, 0);
    }
}


public class TriggerObject : MonoBehaviour
{
    public GameObject oGameObject;
    public Vector3 position;
    public TriggerObject(GameObject oGameObject)
    {
        this.oGameObject = oGameObject;

        if (oGameObject.name == "bone3")
        {
            calculateNewPositionForTip();
        }
        else
        {
            position = oGameObject.transform.position;
        }
    }

    private void Update()
    {
        position = oGameObject.transform.position;
    }

    private void calculateNewPositionForTip()
    {
        CapsuleCollider capsule = oGameObject.GetComponent<CapsuleCollider>();
        if (capsule != null)
        {
            // Update Position
            position = oGameObject.transform.position + new Vector3(0f, capsule.height / 2f) + capsule.center;
        }
    }
}
