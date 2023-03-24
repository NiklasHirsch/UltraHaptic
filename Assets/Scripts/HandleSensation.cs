using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using System;
using UltrahapticsCoreAsset;


public class TriggerObject : MonoBehaviour
{
    // General
    public float ID;

    // Sensible Object
    public GameObject sensibleGameObject;
    public Collider sensibleObjectCollider;

    // Bone
    public Collider collider;
    public GameObject colliderGameObject;

    // Collision
    public Vector3 collisionPos;


    public TriggerObject(GameObject sensibleGameObject, Collider collider)
    {
        
        this.collider = collider;
        this.colliderGameObject = collider.gameObject;
        this.ID = colliderGameObject.GetInstanceID();

        this.sensibleGameObject = sensibleGameObject;
        this.sensibleObjectCollider = sensibleGameObject.GetComponent<Collider>();
        this.UpdateCollisonPosition();
    }

    public void UpdateCollisonPosition()
    {
        Collider boneCollider = this.collider;

        Vector3 pointOnSensibleObjCollider = this.sensibleObjectCollider.ClosestPoint(boneCollider.transform.position);
        Vector3 pointOnBoneCollider = boneCollider.ClosestPoint(this.sensibleObjectCollider.transform.position);
        Vector3 middlePoint = pointOnSensibleObjCollider + (pointOnBoneCollider - pointOnSensibleObjCollider) / 2;

        // Point on the edge of the Bone
        //collisionPos = boneCollider.ClosestPoint(middlePoint);

        // Point on the edge of the object
        collisionPos = this.sensibleObjectCollider.ClosestPoint(middlePoint);
    }
}

public class HandleSensation : MonoBehaviour
{
    public CollisionToSensation collisionToSensation;

    [SerializeField]
    private bool _isSteamSensation = false;

    #region  Old variables
    private float minXY = -0.083f;
    private float maxXY = 0.083f;
    #endregion
    
    [NonSerialized] public List<TriggerObject> activeTriggerObjects = new List<TriggerObject>();
    private Vector3 _comparePoint = new Vector3(0, 0.4f, 0);
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
        UpdateSensation();
    }

    public void UpdateSensation()
    {
        //Debug.Log("Objects in List: " + activeTriggerObjects.Count);
        if (activeTriggerObjects.Count > 0)
        {
            collisionToSensation.SetSensationEnabledStatus(true);

            // Update the collision position for every TriggerObject
            UpdateAllCollisonPos();

            SortByDistance();

            UpdateSensationPoints();
        }
        else
        {
            collisionToSensation.SetSensationEnabledStatus(false);
            for (int i = 0; i < 6; ++i)
            {
                sensationPoints[i] = new Vector3(0, 0, 0);
            }
        }

        collisionToSensation.SetPath(sensationPoints);
    }

    public void UpdateAllCollisonPos()
    {
        foreach (TriggerObject triggerObject in activeTriggerObjects)
        {
            triggerObject.UpdateCollisonPosition();

        }
    }

    public void SortByDistance()
    {
        activeTriggerObjects.Sort(CompareDistanceToPoint);
    }

    private int CompareDistanceToPoint(TriggerObject a, TriggerObject b)
    {
        // check if it is a fingertip bone
        //   true: then use the very tip
        //   false: use the middle point
        //float squaredRangeA = (a.colliderGameObject.name == "bone3") ? (CalculateNewPositionForTip(a.colliderGameObject) - _comparePoint).sqrMagnitude : (a.colliderGameObject.transform.position - _comparePoint).sqrMagnitude;
        //float squaredRangeB = (b.colliderGameObject.name == "bone3") ? (CalculateNewPositionForTip(b.colliderGameObject) - _comparePoint).sqrMagnitude : (b.colliderGameObject.transform.position - _comparePoint).sqrMagnitude;

        float squaredRangeA = (a.collisionPos - _comparePoint).sqrMagnitude;
        float squaredRangeB = (b.collisionPos - _comparePoint).sqrMagnitude;
        return squaredRangeA.CompareTo(squaredRangeB);
    }
    private void UpdateSensationPoints()
    {
        int counter = 1;

        for (int i = 0; i < 6; ++i)
        {
            // there are collision points
            if (activeTriggerObjects.Count > 0)
            {
                if (activeTriggerObjects.Count > i)
                {
                    sensationPoints[i] = activeTriggerObjects[i].collisionPos;

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
                        sensationPoints[i] = sensationPoints[0];// + activeTriggerObjects[0].colliderGameObject.transform.forward * 0.01f;
                    }
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

    private Vector3 UpdatePointPosition(TriggerObject triggerObject)
    {
        Collider sensibleObjectCollider = triggerObject.sensibleGameObject.GetComponent<Collider>();
        Collider boneCollider = triggerObject.collider;

        Vector3 pointOnSensibleObjCollider = sensibleObjectCollider.ClosestPoint(boneCollider.transform.position);
        Vector3 pointOnBoneCollider = boneCollider.ClosestPoint(sensibleObjectCollider.transform.position);
        Vector3 middlePoint = pointOnSensibleObjCollider + (pointOnBoneCollider - pointOnSensibleObjCollider) / 2;
        
        Vector3 mClosestAtBone = boneCollider.ClosestPoint(middlePoint);
        //Vector3 mClosestAtSensibleObj = sensibleObjectCollider.ClosestPoint(middlePoint);

        return mClosestAtBone;
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

    // get bottom point of collision
    private Vector3 OptimizePosition(GameObject obj)
    {
        CapsuleCollider capsule = obj.GetComponent<CapsuleCollider>();
        if (capsule != null)
        {
            // Update Position
            return obj.transform.position - new Vector3(0f, capsule.radius, 0f);
        }

        return new Vector3(0, 0, 0);
    }

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
