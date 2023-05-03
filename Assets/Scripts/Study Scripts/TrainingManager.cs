using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private ScriptableStudyManager _studyManager;
    [SerializeField] private StudySceneLoader _studySceneLoader;
    [SerializeField] private TextMeshPro _nextBtnText;
    [SerializeField] private GameObject _backBtn;

    [Header("Welcome - Info")]
    [SerializeField] private GameObject _welcomeInfo;
    [SerializeField] private GameObject _HandOutlines;

    [Header("Haptic sensation - Info")]
    [SerializeField] private GameObject _hapticInfo;
    [SerializeField] private GameObject _flyingSphere;
    [SerializeField] private GameObject _timerBar;

    [Header("Questionnaire training")]
    [SerializeField] private GameObject _questionnaireInfo;
    [SerializeField] private GameObject _trainingQuestionnaire;
    [SerializeField] private GameObject _trainingQuestionBlock1;
    [SerializeField] private GameObject _trainingQuestionBlock2;
    [SerializeField] private GameObject _ultrahapticsModel;

    [Header("Timer Settings")]
    [SerializeField] private StudySceneLoader sceneLoader;
    [SerializeField] private Image timerBar;
    [SerializeField] private float maxTime = 5.0f;
    private float timeLeft;
    [NonSerialized] public bool startTimer = false;

    // 0 = start; 1 = haptic sensation; 2 = Questionnaire
    private int _trainingState = 0;

    private bool _isQ1 = true;

    void Start() 
    { 
        timeLeft = maxTime;
    }

void Update()
    {
        if (startTimer)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timerBar.fillAmount = 1 - (timeLeft / maxTime);
            }
            else
            {
                //Wait one sec then reset timer so it can be started again
                timeLeft -= Time.deltaTime;
                if(timeLeft < -1)
                {
                    timerBar.fillAmount = 0;
                    timeLeft = maxTime;
                    startTimer = false;
                }
            }
        }
    }

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
                _timerBar.SetActive(true);
                _backBtn.SetActive(true);
                break;
            // haptic sensation
            case 1:
                _hapticInfo.SetActive(false);
                _flyingSphere.SetActive(false);
                _timerBar.SetActive(false);
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

    public void BackButtonPressed()
    {
        switch (_trainingState)
        {
            // haptic sensation
            case 1:
                _welcomeInfo.SetActive(true);
                _HandOutlines.SetActive(true);
                _timerBar.SetActive(false);
                _hapticInfo.SetActive(false);
                _flyingSphere.SetActive(false);
                _backBtn.SetActive(false);
                break;
            // questionnaire
            case 2:
                _hapticInfo.SetActive(true);
                _flyingSphere.SetActive(true);
                _timerBar.SetActive(true);
                _questionnaireInfo.SetActive(false);
                _trainingQuestionnaire.SetActive(false);
                _ultrahapticsModel.SetActive(true);
                _nextBtnText.text = "Next";
                break;
        }

        _trainingState--;
    }

    public void ClickedExampleAnswerButton()
    {
        _trainingQuestionBlock1.SetActive(!_isQ1);
        _trainingQuestionBlock2.SetActive(_isQ1);
        _isQ1 = !_isQ1;
    }
}
