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

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(SceneManager.GetActiveScene().name != _questionnaireSceneName)
            {
                LoadQuestionniareScene();
            } else
            {
                Debug.Log("---- SPACE ---- Trials:" + _studyManager.trial + ", Inital Trials: " +  _studyManager.initalTrials);
                LoadNextScene();
            }
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
            if (_studyManager.trial > 0)
            {
                _studyManager.trial -= 1;
            }
            else
            {
                _studyManager.currentStudyBlock += 1;
                _studyManager.trial = _studyManager.initalTrials;
            }

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
        }
        catch (ArgumentException ex)
        {
            Debug.LogError("Error loading scene: " + ex.Message);
        }
    }

    private void LoadNext(string sceneName)
    {
        if (_studyManager.trial > 0)
        {
            _studyManager.trial -= 1;
        }
        else
        {
            _studyManager.currentStudyBlock += 1;
            _studyManager.trial = _studyManager.initalTrials;
        }

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
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
