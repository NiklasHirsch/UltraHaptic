using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private ScriptableStudyManager _studyManager;
    [SerializeField] private StudySceneLoader _studySceneLoader;
    [SerializeField] private TextMeshPro _nextBtnText;

    [Header("Welcome - Info")]
    [SerializeField] private GameObject _welcomeInfo;
    [SerializeField] private GameObject _HandOutlines;

    [Header("Haptic sensation - Info")]
    [SerializeField] private GameObject _hapticInfo;
    [SerializeField] private GameObject _flyingSphere;

    [Header("Questionnaire training")]
    [SerializeField] private GameObject _questionnaireInfo;
    [SerializeField] private GameObject _trainingQuestionnaire;
    [SerializeField] private GameObject _trainingQuestionBlock1;
    [SerializeField] private GameObject _trainingQuestionBlock2;
    [SerializeField] private GameObject _ultrahapticsModel;

    // 0 = start; 1 = haptic sensation; 2 = Questionnaire
    private int _trainingState = 0;

    private bool _isQ1 = true;

    public void NextButtonPressed()
    {
        switch(_trainingState)
        {
            // start
            case 0:
                _welcomeInfo.SetActive(false);
                _HandOutlines.SetActive(false);
                _hapticInfo.SetActive(true);
                _flyingSphere.SetActive(true);
                break;
            // haptic sensation
            case 1:
                _hapticInfo.SetActive(false);
                _flyingSphere.SetActive(false);
                _questionnaireInfo.SetActive(true);
                _trainingQuestionnaire.SetActive(true);
                _ultrahapticsModel.SetActive(false);
                _nextBtnText.text = "Start Study";
                break;
            // questionnaire
            case 2:
                _questionnaireInfo.SetActive(false);
                _trainingQuestionnaire.SetActive(false);
                _studySceneLoader.LoadNextScene();
                break;
        }

        _trainingState++;
    }

    public void ClickedExampleAnswerButton()
    {
        _trainingQuestionBlock1.SetActive(!_isQ1);
        _trainingQuestionBlock2.SetActive(_isQ1);
        _isQ1 = !_isQ1;
    }
}
