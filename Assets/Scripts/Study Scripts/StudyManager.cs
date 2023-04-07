using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PhysicalState
{
    Solid,
    Liquid,
    Gas
}

public class StudyManager : MonoBehaviour
{

    [SerializeField]
    public int participantNumber = 0;

    [RangeEx(6, 60, 6)]
    public int numberOfParticipants = 30;

    public List<List<PhysicalState>> latinAquareList = new List<List<PhysicalState>>();
    
    // Start is called before the first frame update
    void Start()
    {
        SetupLatinSquareList();
    }

    // Update is called once per frame
    void Update()
    {
        if (participantNumber == 0 || participantNumber < 0)
        {
            Debug.Log("<color=#FF0000> PARTICIPANT NOT SET OR NEAGTIVE! </color>");
        }
    }

    private void SetupLatinSquareList()
    {
        for (int p = 0; p < numberOfParticipants; p++)
        {
            var sec = p % 6;
            List<PhysicalState> row = new List<PhysicalState>();
            row.Add(PhysicalState.Solid);
            row.Add(PhysicalState.Liquid);
            row.Add(PhysicalState.Gas);
            latinAquareList.Add(row);
        }
    }
}
