using System;
using UnityEngine;
using UnityEngine.UI;

public class SetupPhysicalStateScene : MonoBehaviour
{
    [Header("General Settings")]
    #region general
    [SerializeField] private ScriptableStudyManager _studyManager;

    [SerializeField] GameObject interactionOVRRig;

    [SerializeField] private PhysicalState _physicalState;
    #endregion

    [Header("Scene Elements Settings - disbabled/enabled")]
    [SerializeField] private GameObject _ultrahapticModel;

    [SerializeField] private GameObject _hapticElements;

    [SerializeField] private CollisionToSensation _collisionToSensation;

    [SerializeField] private GameObject _blueSate;
    [SerializeField] private GameObject _neutralSate;
    [SerializeField] private GameObject _redSate;

    [Header("Timer Settings")]
    [SerializeField] private StudySceneLoader sceneLoader;
    [SerializeField] private Image timerBar;
    [SerializeField] private float maxTime = 5.0f;
    private float timeLeft;
    [NonSerialized] public bool startTimer = false;

    private (ColorSelection, bool) sceneConfig;

    void Start()
    {
        Debug.Log($"Started - {_physicalState} scene");

        sceneConfig = _studyManager.GetCurrentSceneConfig(_physicalState);

        SetupSceneElements();

        //timerBar = GetComponent<Image>();
        timeLeft = maxTime;
    }

    void Update()
    {
        if (startTimer)
        {
            if (timeLeft > 0)
            {
                Debug.Log($"<color=green>Time Left: {timeLeft} </color>");
                timeLeft -= Time.deltaTime;
                timerBar.fillAmount = 1 - (timeLeft / maxTime);
            }
            else
            {
                Debug.Log("<color=red> Load Next Scene </color>");
                sceneLoader.LoadQuestionniareScene();
            }
        }
        
    }

    private void SetupSceneElements()
    {
        SetupOVRPosition();

        SetupHaptics();

        SetupColors();
    }

    private void SetupOVRPosition()
    {
        interactionOVRRig.transform.position = _studyManager.participantPos;
    }

    private void SetupColors()
    {
        // setup colors
        _blueSate.SetActive(sceneConfig.Item1 == ColorSelection.Blue);
        _neutralSate.SetActive(sceneConfig.Item1 == ColorSelection.Neutral);
        _redSate.SetActive(sceneConfig.Item1 == ColorSelection.Red);
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
            LineRenderer lineRenderer = collisonPoints.gameObject.GetComponent<LineRenderer>();
            lineRenderer.enabled = addHaptics;
            foreach (Transform child in collisonPoints)
            {
                child.gameObject.SetActive(addHaptics);
            }
        }

        // ultrahaptics Model
        _ultrahapticModel.SetActive(true);
    }
}
