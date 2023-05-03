using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensibleObject : MonoBehaviour
{
    public CollisionToSensation collisionToSensation;
    public HandleSensation handleSensation;

    public bool enableTimer = true;
    public SetupPhysicalStateScene setupPScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Bone"))
        {
            if (handleSensation.activeTriggerObjects.Count == 0)
            {
                // LSL send data
                DataStream.Instance.SendData();

                // start timer
                if (enableTimer)
                {
                    //Debug.Log($"<color=blue> Inside Count: {handleSensation.activeTriggerObjects.Count} </color>");
                    setupPScene.startTimer = true;
                }
            }

            handleSensation.activeTriggerObjects.Add(new TriggerObject(gameObject, other));
            handleSensation.UpdateSensation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Bone"))
        {
            handleSensation.activeTriggerObjects.Remove(new TriggerObject(gameObject, other));
            
            handleSensation.UpdateSensation();
        }
    }
}