using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IPQSceneManager : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private ScriptableStudyManager _studyManager;
    [SerializeField] private StudySceneLoader _studySceneLoader;

    [Header("Question / Answer Settings")]
    [SerializeField] private TextMeshProUGUI _questionTitle;
    [SerializeField] private TextMeshProUGUI _questionQuestion;

    [SerializeField] private List<string> _allQuestions;

    public List<GameObject> answerObjects = new List<GameObject>(7);
    public List<string> answerHeaders = new List<string>(14);


    #region private variables
    // private variables
    private int _currentQuestion = 1;

    private int[] selectedAnswers = new int[14];

    private bool _showNextQuestion = false;
    #endregion 

    private void Start()
    {
        //selectedAnswers = new List<int>(_allQuestions.Count);

        StartCoroutine(DisableBtns(0.5f));

        ChangeQuestionAndTitle();
        ChangeAnswerHeaders();
    }

    private void Update()
    {
        if (_showNextQuestion)
        {
            NextQuestion();
            _showNextQuestion = false;
        }
    }

    IEnumerator DisableBtns(float time)
    {
        int index = 0;
        foreach (GameObject obj in answerObjects)
        {

            var textObject = obj.transform.GetChild(1).gameObject;
            textObject.GetComponent<Button>().interactable = false;
            index++;
        }
        yield return new WaitForSeconds(time);

        int index2 = 0;
        foreach (GameObject obj in answerObjects)
        {

            var textObject = obj.transform.GetChild(1).gameObject;
            textObject.GetComponent<Button>().interactable = true;
            index2++;
        }
        yield return null;
    }


    #region QuestionAnswers
    private void ChangeAnswerHeaders()
    {
        string[] subs = answerHeaders[_currentQuestion - 1].Split(',');
        SetBtnText(0, subs[0]);
        SetBtnText(3, subs[1]);
        SetBtnText(6, subs[2]);
    }

    private void SetBtnText(int index, string aText)
    {
        var textObject = answerObjects[index].transform.GetChild(0).gameObject;
        textObject.GetComponent<TextMeshProUGUI>().text = aText;
    }

    private void ChangeQuestionAndTitle()
    {
        _questionTitle.text = $"IPQ - Question {_currentQuestion}:";
        _questionQuestion.text = _allQuestions[_currentQuestion - 1];
    }

    public void QuestionAnswerSelected(int answerType)
    {
        /*
        if(selectedAnswers.Count == 0)
        {
            selectedAnswers = new List<int>(_allQuestions.Count);
        }*/
        
        Debug.Log($"Count: {selectedAnswers.Length} CQ: {_currentQuestion - 1} AType:{answerType}");
        selectedAnswers[_currentQuestion - 1] = answerType;

        Debug.Log($"<color=green>IPQ - Question {_currentQuestion}: {answerType} </color>");

        if(_currentQuestion < _allQuestions.Count)
        {
            _showNextQuestion = true;
        } else
        {
            SubmitAnswers();
        }
    }
    public void NextQuestion()
    {
        _currentQuestion += 1;
        ChangeQuestionAndTitle();
        ChangeAnswerHeaders();
        StartCoroutine(DisableBtns(0.25f));
    }
    #endregion

    #region submit selection
    public void SubmitAnswers()
    {

        var block = _studyManager.currentParticipantList[_studyManager.currentStudyBlock];
        string csvString = $"{_studyManager.participantNumber}{_studyManager._dataSeperator}{block}{_studyManager._dataSeperator}";

        foreach (int answer in selectedAnswers)
        {
            csvString += $"{answer}{_studyManager._dataSeperator}";
        }

        _studyManager.AppendCSVLineIPQ(csvString);

        _studySceneLoader.LoadNextScene();
    }
    #endregion
}
