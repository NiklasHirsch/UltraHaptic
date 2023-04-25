using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _questionBlocks;

    private int _currentQuestion = 1;

    [SerializeField]
    private GameObject _prevBtn;

    [SerializeField]
    private GameObject _nextBtn;

    [SerializeField]
    private RoundedBoxProperties _submitBtnPanel;
    [SerializeField]
    private TextMeshPro _submitBtnText;
    [SerializeField]
    private RawImage _submitBtnIcon;

    private bool _question1AnswerSelected = false;
    private bool _question2AnswerSelected = false;

    private AnswerType q1Answer;
    private AnswerType q2Answer;

    private int q1AnswerNum;
    private int q2AnswerNum;

    public List<AnswerType> answerTypes = new List<AnswerType>(5);

    [SerializeField]
    private ScriptableStudyManager _studyManager;

    [SerializeField]
    private StudySceneLoader _studySceneLoader;

    //------------ Additional Stuff -------------

    [Header("Slider Settings")]
    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private TextMeshProUGUI _sliderValue;

    private bool _showNextQuestion = false;

    private void Update()
    {
        if (_showNextQuestion)
        {
            _questionBlocks[_currentQuestion].gameObject.SetActive(true);
            _currentQuestion += 1;
            _showNextQuestion = false;
        }
    }

    #region Next / Prev
    public void NextQuestion()
    {
        if (_questionBlocks[_currentQuestion] != null && _questionBlocks[_currentQuestion - 1] != null)
        {
            // show next question 
            _questionBlocks[_currentQuestion - 1].gameObject.SetActive(false);
            _showNextQuestion = true;
        }
    }

    public void NextQuestionWithNextBtn()
    {
        if (_questionBlocks[_currentQuestion] != null)
        {
            // show next question 
            _questionBlocks[_currentQuestion - 1].gameObject.SetActive(false);
            _questionBlocks[_currentQuestion].gameObject.SetActive(true);
            _currentQuestion += 1;

            // set Btns active / inactive 
            _nextBtn.SetActive(false);
            _prevBtn.SetActive(true);
        }
    }

    public void PrevQuestion()
    {
        if (_questionBlocks[_currentQuestion - 2] != null)
        {
            // show previous question
            _questionBlocks[_currentQuestion - 1].gameObject.SetActive(false);
            _questionBlocks[_currentQuestion - 2].gameObject.SetActive(true);
            _currentQuestion -= 1;

            // set Btns active / inactive 
            _prevBtn.SetActive(false);
            _nextBtn.SetActive(true);
        }
    }
    #endregion

    #region Answers
    public void  QuestionAnswerSelected(int answerType)
    {
       switch(_currentQuestion)
       {
            case 1:
                Question1AnswerSelected(answerType);
                break;
            case 2:
                Question2AnswerSelected(answerType);
                break;
            default:
                break;
       }
    }

    public void Question1AnswerSelected(int answerType)
    {
        if(!_question1AnswerSelected)
        {
            _question1AnswerSelected = true;
            //UpdateSubmitBtn();
        }

        q1AnswerNum = answerType;
        q1Answer = answerTypes[answerType];
        Debug.Log("A1: " + q1Answer);

        // no next btn anymore
        NextQuestion();
    }

    public void Question2AnswerSelected(int answerType)
    {
        if (!_question2AnswerSelected)
        {
            _question2AnswerSelected = true;
            //UpdateSubmitBtn();
        }

        q2AnswerNum = answerType;
        q2Answer = answerTypes[answerType];
        Debug.Log("A2: " + q2Answer);

        // no submit btn anymore
        SubmitAnswers();
    }

    private void UpdateSubmitBtn()
    {
        if (_question1AnswerSelected && _question2AnswerSelected)
        {
            Color tempColP = _submitBtnPanel.Color;
            tempColP.a = 1f;
            _submitBtnPanel.Color = tempColP;

            Color tempColT = _submitBtnText.color;
            tempColT.a = 1f;
            _submitBtnText.color = tempColT;

            Color tempColI = _submitBtnIcon.color;
            tempColI.a = 1f;
            _submitBtnIcon.color = tempColI;
        }
    }

    public void HandleSliderValue()
    {
        _sliderValue.text = "" + _slider.value.ToString("F1") + "°C";
    }

    public void SubmitAnswers()
    {
        if (_question1AnswerSelected && _question2AnswerSelected && !q1Answer.Equals(null) && !q2Answer.Equals(null))
        {
            var uniqueID = _studyManager.GetUniqueID();
            var block = _studyManager.currentParticipantList[_studyManager.currentStudyBlock];
            var trial = _studyManager.currentStudyBlock * 30 + _studyManager.trial ;
            var haptic = _studyManager.currentSceneConfig.Item2;
            var color = _studyManager.currentSceneConfig.Item1;
            _studyManager.AppendCSVLine($"{uniqueID}{_studyManager._dataSeperator}{block}{_studyManager._dataSeperator}{trial}{_studyManager._dataSeperator}{haptic}{_studyManager._dataSeperator}{color}{_studyManager._dataSeperator}{q1AnswerNum}, {q1Answer}{_studyManager._dataSeperator}{q2AnswerNum}, {q2Answer}{_studyManager._dataSeperator}{_studyManager.participantNumber}");

            _studySceneLoader.LoadNextScene();
        }
    }
    #endregion
}
