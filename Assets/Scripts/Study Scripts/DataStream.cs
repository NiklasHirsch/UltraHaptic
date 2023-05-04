using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class DataStream : Singleton<DataStream>
{
    #region LSL
    string StreamName = "LSL4Unity.Samples.SimpleCollisionEvent";
    string StreamType = "Markers";
    private StreamOutlet outlet;
    private string[] sample = { "" };

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        var hash = new Hash128();
        hash.Append(StreamName);
        hash.Append(StreamType);
        hash.Append(gameObject.GetInstanceID());
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 1, LSL.LSL.IRREGULAR_RATE,
            channel_format_t.cf_string, hash.ToString());
        outlet = new StreamOutlet(streamInfo);
    }
    #endregion
    public void SendData(string dataText)
    {
        // LSL send data
        if (outlet != null)
        {
            sample[0] = dataText;
            outlet.push_sample(sample);
        }
    }
    
}
