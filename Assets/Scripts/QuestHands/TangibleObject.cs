using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class TangibleObject : MonoBehaviour
{
    public CollisionToSensation collisionToSensation;
    public HandleSensation handleSensation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Bone"))
        {
            //Debug.Log("Bone triggered");

            handleSensation.activeTriggerObjects.Add(other.gameObject); //new TriggerObject(other.gameObject)
            collisionToSensation.SetSensationEnabledStatus(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Bone"))
        {
            //TriggerObject objToRemove = _activeTriggerObjects.Find((x) => x.oGameObject.GetInstanceID() == other.gameObject.GetInstanceID());
            //_activeTriggerObjects.Remove(objToRemove);

            handleSensation.activeTriggerObjects.Remove(other.gameObject);

            //Debug.Log("Bone exited");

            if (handleSensation.activeTriggerObjects.Count <= 0)
            {
                collisionToSensation.SetSensationEnabledStatus(false);
            }
        }
    }

    /*

    private void Update()
    {
        Debug.Log("Objects in List: " + _activeTriggerObjects.Count);

        SortByDistance();

        UpdateSensation();
    }

    private void SortByDistance()
    {
        _activeTriggerObjects.Sort(CompareDistanceToPoint);
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

    private void UpdateSensation()
    {
        int counter = 1;

        for (int o = 0; o < 6; ++o)
        {
            // there are collision points
            if(_activeTriggerObjects.Count > 0)
            {
                if (_activeTriggerObjects.Count > o)
                {
                    sensationPoints[o] = (_activeTriggerObjects[o].name == "bone3") ? calculateNewPositionForTip(_activeTriggerObjects[o]) : _activeTriggerObjects[o].transform.position;
                } else
                {
                    sensationPoints[o] = sensationPoints[o - counter];
                    counter++;
                }
            }
            // there are NO collision points
            else
            {
                sensationPoints[o] = new Vector3(0, 0, 0);
            }
        }

        collisionToSensation.SetPath(sensationPoints);
    }
    */
}