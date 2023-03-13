using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using System;
using UltrahapticsCoreAsset;

public class HandleSensation : MonoBehaviour
{
    public CollisionToSensation collisionToSensation;

    public RigidHand leftRigidHand;
    public RigidHand rightRigidHand;

    [SerializeField]
    private bool _isSteamSensation = false;

    #region  Old variables
    private float minXY = -0.083f;
    private float maxXY = 0.083f;
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
        //Debug.Log("Objects in List: " + activeTriggerObjects.Count);
        if((leftRigidHand.gameObject.activeSelf || rightRigidHand.gameObject.activeSelf))
        {
            collisionToSensation.SetSensationEnabledStatus(true);
            SortByDistance();

            UpdateSensation();

        } else
        {
            collisionToSensation.SetSensationEnabledStatus(false);
            for (int i = 0; i < 6; ++i)
            {
                sensationPoints[i] = new Vector3(0, 0, 0);
            }
        }

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
        float squaredRangeA = (a.name == "bone3") ? (CalculateNewPositionForTip(a) - _comparePoint).sqrMagnitude : (a.transform.position - _comparePoint).sqrMagnitude;
        float squaredRangeB = (b.name == "bone3") ? (CalculateNewPositionForTip(b) - _comparePoint).sqrMagnitude : (b.transform.position - _comparePoint).sqrMagnitude;
        return squaredRangeA.CompareTo(squaredRangeB);
    }
    private void UpdateSensation()
    {
        int counter = 1;

        for (int i = 0; i < 6; ++i)
        {
            // there are collision points
            if (activeTriggerObjects.Count > 0 && (leftRigidHand.gameObject.activeSelf || rightRigidHand.gameObject.activeSelf))
            {
                if (activeTriggerObjects.Count > i)
                {
                    UpdatePointPosition(activeTriggerObjects[i]);
                    //sensationPoints[i] = (activeTriggerObjects[i].name == "bone3") ? CalculateNewPositionForTip(activeTriggerObjects[i]) : activeTriggerObjects[i].transform.position;
                    sensationPoints[i] = activeTriggerObjects[i].transform.position;

                    // if steam then modify points randomly 
                    if (_isSteamSensation) {
                        UpdateSteamSensation(i);
                    }
                }
                else
                {
                    if (activeTriggerObjects.Count > 1)
                    {
                        // draw a sensation line between the last two point back and forth
                        sensationPoints[i] = sensationPoints[i - counter - (counter % 2)];
                        
                    } else
                    {
                        // if only 1 sensation collision is present then apply a small movement to the secon point to get a small sensation line to feel the sensation 
                        sensationPoints[i] = sensationPoints[0] + activeTriggerObjects[0].transform.forward * 0.01f; //sensationPoints[0] - new Vector3(0, -0.002f, 0); //ApplyNoise(sensationPoints[0]);
                        //sensationPoints[0] = sensationPoints[0] - activeTriggerObjects[0].transform.forward * 0.01f;
                    }
                    //sensationPoints[i] = sensationPoints[i - counter];
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

    private void UpdatePointPosition(GameObject obj)
    {
        if (obj.name == "bone3")
        {
            CalculateNewPositionForTip(obj);
        }
        else
        {
            OptimizePosition(obj);
        }
    }

    private Vector3 CalculateNewPositionForTip(GameObject obj)
    {
        CapsuleCollider capsule = obj.GetComponent<CapsuleCollider>();
        if (capsule != null)
        {
            // Update Position
            return obj.transform.position + new Vector3(0f, capsule.height / 2f) + capsule.center;
        }

        return new Vector3(0, 0, 0);
    }

    private Vector3 OptimizePosition(GameObject obj)
    {
        CapsuleCollider capsule = obj.GetComponent<CapsuleCollider>();
        if (capsule != null)
        {
            // Update Position
            return obj.transform.position + new Vector3(0f, -1 * capsule.radius, 0f);
        }

        return new Vector3(0, 0, 0);
    }


    private Vector3 zeroVec = new Vector3(0, 0, 0);
    private void UpdateSteamSensation(int i)
    {
        if (sensationPoints[i] != zeroVec)
        {
            sensationPoints[i] = ApplyNoise(sensationPoints[i]);
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

    /*
    private void UpdateSolidSensation()
    {
        int counter = 1;

        for (int i = 0; i < 6; ++i)
        {
            // there are collision points
            if (activeTriggerObjects.Count > 0 && (leftRigidHand.gameObject.activeSelf || rightRigidHand.gameObject.activeSelf))
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
    */
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
