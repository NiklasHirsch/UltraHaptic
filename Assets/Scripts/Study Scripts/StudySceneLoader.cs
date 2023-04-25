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
    private string _endSceneName = "End Scene";

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name != _questionnaireSceneName)
            {
                Debug.Log("---- SPACE ----: load questionnaire scene");
                LoadQuestionniareScene();
            }
            else
            {
                Debug.Log("---- SPACE ---- Trials:" + _studyManager.trial + ", Inital Trials: " + _studyManager.initalTrials);
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
        int lastSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;

        SceneManager.LoadScene(lastSceneIndex);

        if (_studyManager.trial == 0)
        {
            _studyManager.currentStudyBlock -= 1;
            _studyManager.trial = _studyManager.initalTrials - 1;
        }
        else
        {
            _studyManager.trial -= 1;
        }

        if (SceneManager.GetSceneAt(lastSceneIndex).name != _questionnaireSceneName)
        {
            _studyManager.DeleteLastCSVLine();
            LoadQuestionniareScene();
        }
        else
        {
            LoadNextScene();
        }
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
