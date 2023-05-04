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
                if(DataStream.Instance != null)
                {
                    // LSL send data
                    string dataString = handleSensation.studyManager.GetUniqueID();
                    // remove last character
                    dataString = dataString.Substring(0, dataString.Length - 1);
                    DataStream.Instance.SendData(dataString);
                } else if (TestStream.Instance != null)
                {
                    TestStream.Instance.SendData("2");
                }

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