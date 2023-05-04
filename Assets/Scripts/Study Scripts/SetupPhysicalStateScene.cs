using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetupPhysicalStateScene : MonoBehaviour
{
    [Header("General Settings")]
    #region general
    [SerializeField] private ScriptableStudyManager _studyManager;

    [SerializeField] GameObject interactionOVRRig;

    [SerializeField] private PhysicalState _physicalState;

    [SerializeField] private RetractArea _retractArea;
    private bool handsWereOutsideOfRetractArea = false;
    #endregion

    [Header("Scene Elements Settings - disbabled/enabled")]
    [SerializeField] private GameObject _ultrahapticModel;

    [SerializeField] private GameObject _hapticElements;

    [SerializeField] private CollisionToSensation _collisionToSensation;

    [SerializeField] private GameObject _blueState;
    [SerializeField] private GameObject _neutralState;
    [SerializeField] private GameObject _redState;

    [Header("Timer Settings")]
    [SerializeField] private StudySceneLoader sceneLoader;
    [SerializeField] private Image timerBar;
    [SerializeField] private float maxTime = 5.0f;
    private float timeLeft;
    [NonSerialized] public bool startTimer = false;

    [Header("Info Text")]
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private string textHands = "Retract your hands to your body please.";
    [SerializeField] private string textInteract = "Interact with the solid cube and try to get a sensation.";

    private (ColorSelection, bool) sceneConfig;

    private bool startWithCheck = false;

    void Start()
    {
        sceneConfig = _studyManager.GetCurrentSceneConfig(_physicalState);

        infoText.text = textHands;

        // Log state to have in in the console
        Debug.Log($"<color=yellow> Started - ID: {_studyManager.GetUniqueID()},  Block: {_studyManager.currentParticipantList[_studyManager.currentStudyBlock]}, Trial: {_studyManager.currentStudyBlock * 30 + _studyManager.trial}, Config: {sceneConfig} </color>");
        SetAllStatesOff();
        SetupSceneElements();

        StartCoroutine(WaitForCheck());

        timeLeft = maxTime;
    }

    void Update()
    {
        if (startWithCheck)
        {
            RetractCheckSetup();
        }

        TimerFunction();
    }
    IEnumerator WaitForCheck()
    {
        yield return new WaitForSeconds(0.2f);
        startWithCheck = true;
        yield return null;
    }

    private void RetractCheckSetup()
    {
        if (!handsWereOutsideOfRetractArea)
        {
            if (_retractArea.IsNotColliding)
            {
                _retractArea.GetComponent<MeshRenderer>().enabled = false;
                infoText.text = textInteract;
                handsWereOutsideOfRetractArea = true;
                SetupColors();
            }
        }
    }
    private void TimerFunction()
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
                sceneLoader.LoadQuestionniareScene();
            }
        }
    }

    private void SetAllStatesOff()
    {
        _blueState.SetActive(false);
        _neutralState.SetActive(false);
        _redState.SetActive(false);
    }

    private void SetupSceneElements()
    {
        SetupOVRPosition();

        SetupHaptics();

        //RetractCheckSetup();
    }

    private void SetupOVRPosition()
    {
        // Setup Position
        interactionOVRRig.transform.position = _studyManager.participantPos;

        // Setup Rotation
        interactionOVRRig.transform.rotation = _studyManager.participantRot;
    }

    private void SetupColors()
    {
        // setup colors
        _blueState.SetActive(sceneConfig.Item1 == ColorSelection.Blue);
        _neutralState.SetActive(sceneConfig.Item1 == ColorSelection.Neutral);
        _redState.SetActive(sceneConfig.Item1 == ColorSelection.Red);
    }

    private void SetupHaptics(){
        bool addHaptics = sceneConfig.Item2;

        if (_studyManager.disableHapticFeedbackElements)
        {
            _hapticElements.SetActive(false);
        } else
        {
            _collisionToSensation.SetSensationEnabledStatus(addHaptics);
            // disable polyline sensation part
            Transform polylineSensation = _hapticElements.transform.Find("PolylineSensation");
            polylineSensation.gameObject.SetActive(addHaptics);

            // disable Line Renderer and path points
            Transform collisonPoints = _hapticElements.transform.Find("CollisonPoints");
            /*
            LineRenderer lineRenderer = collisonPoints.gameObject.GetComponent<LineRenderer>();
            lineRenderer.enabled = addHaptics;
            foreach (Transform child in collisonPoints)
            {
                child.gameObject.SetActive(addHaptics);
            }*/
        }

        // ultrahaptics Model
        _ultrahapticModel.SetActive(true);
    }
}
