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

    public List<AnswerType> answerTypes = new List<AnswerType>(5);

    [SerializeField]
    private ScriptableStudyManager _studyManager;

    [SerializeField]
    private StudySceneLoader _studySceneLoader;


    #region Next / Prev
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

    public void NextQuestion()
    {
        if(_questionBlocks[_currentQuestion] != null)
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
    #endregion

    #region Answers

    public void Question1AnswerSelected(int answerType)
    {
        if(!_question1AnswerSelected)
        {
            _question1AnswerSelected = true;
            UpdateSubmitBtn();
        }

        q1Answer = answerTypes[answerType];
        Debug.Log("A1: " + q1Answer);
    }

    public void Question2AnswerSelected(int answerType)
    {
        if (!_question2AnswerSelected)
        {
            _question2AnswerSelected = true;
            UpdateSubmitBtn();
        }

        q2Answer = answerTypes[answerType];
        Debug.Log("A2: " + q2Answer);
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

    public void SubmitAnswers()
    {
        if (_question1AnswerSelected && _question2AnswerSelected && !q1Answer.Equals(null) && !q2Answer.Equals(null))
        {
            _studyManager.AppendCSVLine($"Trial:{_studyManager._dataSeperator}{_studyManager.trial}");
            _studyManager.AppendCSVLine($"Answer 1:{_studyManager._dataSeperator}{q1Answer}{_studyManager._dataSeperator}Answer 2:{_studyManager._dataSeperator}{q1Answer}");
            _studySceneLoader.LoadNextScene();
        }
    }
    #endregion
}
