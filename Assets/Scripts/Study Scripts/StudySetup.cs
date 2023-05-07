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
    VeryCold,
    Cold,
    Cool,
    SlightlyCool,
    Neutral,
    SlightlyWarm,
    Warm,
    Hot,
    VeryHot
}

public enum AnswerTypeQ2
{
    VeryInconsistent = 0,
    I3 = 1,
    I2 = 2,
    I1 = 3,
    Neutral = 4,
    C1 = 5,
    C2 = 6,
    C3 = 7,
    VeryConsistent = 8
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

        SetupRandomItems();

        _welcomeparticipantText.text += $" {_studyManager.participantNumber}:";

        _studyManager.currentStudyBlock = 0;
        _studyManager.trial = 0;//_studyManager.initalTrials;

        _studyManager.SetupLatinSquareList();

        _studyManager.SetupParticipantList();

        _studyManager.SetupWriter();

        if(_studyManager.startWithStep > 0)
        {            
            if (DataStream.Instance != null)
            {
                //Send data to stream that the expirement has started
                DataStream.Instance.SendData("0");
            }
            else if (TestStream.Instance != null)
            {
                TestStream.Instance.SendData("0");
            }

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

    private void SetupRandomItems()
    {
        //Set Random Seed with p-num
        UnityEngine.Random.InitState(_studyManager.participantNumber);
        _studyManager.SetupRandomizedTrialOrder(_studyManager.trialSolidConfigList);
        _studyManager.SetupRandomizedTrialOrder(_studyManager.trialLiquidConfigList);
        _studyManager.SetupRandomizedTrialOrder(_studyManager.trialGasConfigList);
        LogList(_studyManager.trialSolidConfigList);
        LogList(_studyManager.trialLiquidConfigList);
        LogList(_studyManager.trialGasConfigList);
    }

    private void LoadSceneOfStep(int step)
    {
        _studyManager.currentStudyBlock = Mathf.FloorToInt(step / _studyManager.initalTrials);// + 1;
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

    private void LogList(List<(ColorSelection, bool)> list)
    {
        // result String of list content
        string listContent = "";

        int listIndex = 0;
        foreach (var elem in list)
        {
            listContent += $"Color: {elem.Item1}, Haptic: {elem.Item2}";
            listContent += "\n";
            listIndex++;
        }

        Debug.Log("<color=#00FFFF>" + listContent + "</color>");
    }
}
