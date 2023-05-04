using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class TestStream : Singleton<DataStream>
{
    #region LSL
    string StreamName = "TestStream";
    string StreamType = "EEG";
    private StreamOutlet outlet;
    private float[] sample = { 0.0f };

    private void Update()
    {
        SendData(UnityEngine.Random.Range(0, 100));
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        var hash = new Hash128();
        hash.Append(StreamName);
        hash.Append(StreamType);
        hash.Append(gameObject.GetInstanceID());
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 1, 100,
            channel_format_t.cf_float32, hash.ToString());
        outlet = new StreamOutlet(streamInfo);
    }
    #endregion
    public void SendData(float data)
    {
        // LSL send data
        if (outlet != null)
        {
            sample[0] = data;
            outlet.push_sample(sample);
        }
    }

}

