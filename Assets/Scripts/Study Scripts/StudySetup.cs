using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public enum ColorSelection
{
    Blue,
    Neutral,
    Red
}
public enum PhysicalState
{
    Solid,
    Liquid,
    Gas
}

public enum AnswerTypeQ1
{
    FullyDisagree,
    Disagree,
    PartialDisagree,
    Neutral,
    PartialAgree,
    Agree,
    FullyAgree
}

public enum AnswerTypeQ2
{
    Colder = 0,
    C2 = 1,
    C1 = 2,
    Neutral = 3,
    W1 = 4,
    W2 = 5,
    Warmer = 6
}

public class StudySetup : MonoBehaviour
{
    [SerializeField] private ScriptableStudyManager _studyManager;

    [SerializeField] private StudySceneLoader _sceneLoader;

    [SerializeField] private TextMeshProUGUI _welcomeparticipantText;

    // Start is called before the first frame update
    void Start()
    {
        if(_studyManager.numberOfParticipants == 0)
        {
            _studyManager.numberOfParticipants = 30;
        }

        //Set Random Seed with p-num
        UnityEngine.Random.InitState(_studyManager.participantNumber);

        _welcomeparticipantText.text += $" {_studyManager.participantNumber}:";

        _studyManager.currentStudyBlock = 0;
        _studyManager.trial = 0;//_studyManager.initalTrials;

        _studyManager.SetupLatinSquareList();

        _studyManager.SetupParticipantList();

        _studyManager.SetupWriter();

        if(_studyManager.startWithStep > 0)
        {
            LoadSceneOfStep(_studyManager.startWithStep);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_studyManager.participantNumber < 1)
        {
            Debug.Log("<color=red> PARTICIPANT NOT SET OR NEAGTIVE! </color>");
        }
    }

    private void LoadSceneOfStep(int step)
    {
        _studyManager.currentStudyBlock = Mathf.FloorToInt(step / _studyManager.initalTrials);
        _studyManager.trial = (step % _studyManager.initalTrials) - 1;
        _sceneLoader.LoadNextScene();
    }

    private void LogList(List<List<PhysicalState>> list)
    {
        // result String of list content
        string listContent = "";

        int listIndex = 0;
        foreach (var listX in list)
        {
            listContent += $"Participant {listIndex + 1}: ";
            foreach (var pState in listX)
            {
                
                listContent += $"{pState}, ";
            }
            listContent += "\n";
            listIndex++;
        }

        Debug.Log("<color=#00FFFF>" + listContent + "</color>");
    }
}
