using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class SensibleObject : MonoBehaviour
{
    public CollisionToSensation collisionToSensation;
    public HandleSensation handleSensation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Bone"))
        {
            handleSensation.activeTriggerObjects.Add(new TriggerObject(gameObject, other));
            handleSensation.UpdateSensation();

            Debug.Log("Bone triggered - TriggerObjects: " + handleSensation.activeTriggerObjects);
            Debug.Log("Objects in List: " + handleSensation.activeTriggerObjects.Count);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //TriggerObject searchT = new TriggerObject(gameObject, other);
        //TriggerObject found = handleSensation.activeTriggerObjects.Find(t => t == searchT);
        //handleSensation.activeTriggerObjects.Contains(searchT);
        //int index = handleSensation.activeTriggerObjects.IndexOf(searchT);
        //int index = handleSensation.activeTriggerObjects.BinarySearch(searchT);
        
        //TriggerObject found = handleSensation.activeTriggerObjects.Find(t => t.colliderGameObject.name == other.gameObject.name);
        //Debug.Log("Stay TriggerObject, Found: " + found);

        /*
        foreach (TriggerObject triggerObject in handleSensation.activeTriggerObjects)
        {
            Debug.Log(triggerObject.colliderGameObject.name);
        }*/

        //Debug.Log("Collider ID: " + other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Bone"))
        {
            //TriggerObject objToRemove = handleSensation.activeTriggerObjects.Find(x => x.ID == other.gameObject.GetInstanceID());
            //handleSensation.activeTriggerObjects.Remove(objToRemove);
            //Debug.Log("ObjectToRemove: " + objToRemove);

            handleSensation.activeTriggerObjects.Remove(new TriggerObject(gameObject, other));
            
            /*List<TriggerObject> found = handleSensation.activeTriggerObjects.FindAll(t => t.colliderGameObject.name == other.gameObject.name);
            foreach (TriggerObject triggerObject in found)
            {
                handleSensation.activeTriggerObjects.Remove(triggerObject);
            }
            */
            
            handleSensation.UpdateSensation();

            Debug.Log("Bone exited - TriggerObjects: " + handleSensation.activeTriggerObjects);
            Debug.Log("Objects in List: " + handleSensation.activeTriggerObjects.Count);
        }
    }
}