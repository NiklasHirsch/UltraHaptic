using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private ScriptableStudyManager _studyManager;

    [SerializeField]
    private string _solidSceneName = "Solid - Haptic sensation";

    [SerializeField]
    private string _liquidSceneName = "Liquid - Haptic sensation";

    [SerializeField]
    private string _gasSceneName = "Gas - Haptic sensation";

    public void LoadNextScene()
    {
        try
        {
            switch (_studyManager.currentParticipantList[_studyManager.currentStudyBlock])
            {
                case PhysicalState.Solid:
                    SceneManager.LoadScene(_solidSceneName, LoadSceneMode.Single);
                    break;
                case PhysicalState.Liquid:
                    SceneManager.LoadScene(_liquidSceneName, LoadSceneMode.Single);
                    break;
                case PhysicalState.Gas:
                    SceneManager.LoadScene(_gasSceneName, LoadSceneMode.Single);
                    break;
            }
            _studyManager.currentStudyBlock += 1;
        }
        catch (ArgumentException ex)
        {
            Debug.LogError("Error loading scene: " + ex.Message);
        }
    }

    public void ReloadScene()
    {
        try
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
        catch (ArgumentException ex)
        {
            Debug.LogError("Error loading scene: " + ex.Message);
        }
    }
}
