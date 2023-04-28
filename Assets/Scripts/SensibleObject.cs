using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class SensibleObject : MonoBehaviour
{
    public CollisionToSensation collisionToSensation;
    public HandleSensation handleSensation;

    public bool enableTimer = true;
    public SetupPhysicalStateScene setupPScene;


    #region LSL
    string StreamName = "LSL4Unity.Samples.SimpleCollisionEvent";
    string StreamType = "Markers";
    private StreamOutlet outlet;
    private string[] sample = { "" };

    void Start()
    {
        var hash = new Hash128();
        hash.Append(StreamName);
        hash.Append(StreamType);
        hash.Append(gameObject.GetInstanceID());
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 1, LSL.LSL.IRREGULAR_RATE,
            channel_format_t.cf_string, hash.ToString());
        outlet = new StreamOutlet(streamInfo);
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Bone"))
        {
            if (handleSensation.activeTriggerObjects.Count == 0)
            {
                // LSL send data
                if (outlet != null)
                {
                    sample[0] = "TriggerEnter " + gameObject.GetInstanceID();
                    outlet.push_sample(sample);
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