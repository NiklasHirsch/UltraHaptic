using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StudySceneLoader : MonoBehaviour
{
    [SerializeField]
    private ScriptableStudyManager _studyManager;

    [SerializeField]
    private string _solidSceneName = "Solid - Haptic sensation";

    [SerializeField]
    private string _liquidSceneName = "Liquid - Haptic sensation";

    [SerializeField]
    private string _gasSceneName = "Gas - Haptic sensation";

    [SerializeField]
    private string _questionnaireSceneName = "Questionnaire Scene";

    [SerializeField]
    private string _startSceneName = "Start Scene";

    [SerializeField]
    private string _endSceneName = "End Scene";

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log($"<color=blue> Jumped forward trial: {_studyManager.trial} </color>");
            if(SceneManager.GetActiveScene().name == _startSceneName)
            {
                LoadNextScene();
            }

            else if (SceneManager.GetActiveScene().name != _questionnaireSceneName && SceneManager.GetActiveScene().name != _endSceneName)
            {
                LoadQuestionniareScene();
            }
            else
            {
                var uniqueID = _studyManager.GetUniqueID();
                var block = _studyManager.currentParticipantList[_studyManager.currentStudyBlock];
                var trial = _studyManager.currentStudyBlock * 30 + _studyManager.trial;
                var haptic = _studyManager.currentSceneConfig.Item2;
                var color = _studyManager.currentSceneConfig.Item1;
                _studyManager.AppendCSVLine($"{uniqueID}{_studyManager._dataSeperator}{block}{_studyManager._dataSeperator}{trial}{_studyManager._dataSeperator}{haptic}{_studyManager._dataSeperator}{color}{_studyManager._dataSeperator}skipped{_studyManager._dataSeperator}skipped{_studyManager._dataSeperator}{_studyManager.participantNumber}");

                LoadNextScene();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LoadPreviousScene();
        }
    }

    public void LoadQuestionniareScene()
    {
        try
        {
            SceneManager.LoadSceneAsync(_questionnaireSceneName, LoadSceneMode.Single);

        }
        catch (ArgumentException ex)
        {
            Debug.LogError("Error loading scene: " + ex.Message);
        }
    }
    public void LoadNextScene()
    {
        try
        {
            if (_studyManager.trial < _studyManager.initalTrials)
            {
                _studyManager.trial += 1;
                //_studyManager.trial -= 1;
            }
            else
            {
                _studyManager.currentStudyBlock += 1;
                _studyManager.trial = 0;
                //_studyManager.trial = _studyManager.initalTrials;
            }

            if (_studyManager.currentStudyBlock >= 0 && _studyManager.currentStudyBlock < _studyManager.currentParticipantList.Count)
            {
                switch (_studyManager.currentParticipantList[_studyManager.currentStudyBlock])
                {
                    case PhysicalState.Solid:
                        //Debug.Log("load - Solid scene");
                        SceneManager.LoadSceneAsync(_solidSceneName, LoadSceneMode.Single);
                        break;
                    case PhysicalState.Liquid:
                        //Debug.Log("load - Liquid scene");
                        SceneManager.LoadSceneAsync(_liquidSceneName, LoadSceneMode.Single);
                        break;
                    case PhysicalState.Gas:
                        //Debug.Log("load - Gas scene");
                        SceneManager.LoadSceneAsync(_gasSceneName, LoadSceneMode.Single);
                        break;
                }
            } else
            {
                SceneManager.LoadSceneAsync(_endSceneName, LoadSceneMode.Single);
            }
        }
        catch (ArgumentException ex)
        {
            Debug.LogError("Error loading scene: " + ex.Message);
        }
    }

    public void LoadPreviousScene()
    {
        // if started with a inital step other than 0 return if first loaded trial
        if (_studyManager.startWithStep > 0 && _studyManager.startWithStep == _studyManager.trial - 1 && SceneManager.GetActiveScene().name != _questionnaireSceneName)
        {
            return;
        }
        // return if start scene
        if (SceneManager.GetActiveScene().name == _startSceneName)
        {
            return;
        }

        Debug.Log($"<color=blue> Jumped backward trial: {_studyManager.trial}</color>");
        if (_studyManager.trial == 0)
        {
            _studyManager.currentStudyBlock -= 1;
            _studyManager.trial = _studyManager.initalTrials - 1;
        }
        else
        {
            _studyManager.trial -= 1;
        }

        if (SceneManager.GetActiveScene().name != _questionnaireSceneName)
        {
            if(_studyManager.trial == 0 && _studyManager.currentStudyBlock == 0)
            {
                LoadStartScene();
            } else
            {
                _studyManager.DeleteLastCSVLine();
                LoadQuestionniareScene();
            }
        }
        else
        {
            LoadNextScene();
        }
    }

    private void LoadStartScene()
    {
        SceneManager.LoadSceneAsync(_startSceneName);
    }

    public void ReloadScene()
    {
        try
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadSceneAsync(currentSceneName);
        }
        catch (ArgumentException ex)
        {
            Debug.LogError("Error loading scene: " + ex.Message);
        }
    }
}
