using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneLoaded : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (DataStream.Instance != null)
        {
            //Send data to stream that the expirement has ended
            DataStream.Instance.SendData("1");
        }
        else if (TestStream.Instance != null)
        {
            TestStream.Instance.SendData("1");
        }
    }
}
