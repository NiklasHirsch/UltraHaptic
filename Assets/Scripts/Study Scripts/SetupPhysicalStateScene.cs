using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupPhysicalStateScene : MonoBehaviour
{
    [Header("General Settings")]
    #region general
    [SerializeField]
    private ScriptableStudyManager _studyManager;

    [SerializeField]
    private PhysicalState _physicalState;
    #endregion

    [Header("Scene Elements Settings - disbabled/enabled")]
    [SerializeField]
    private GameObject _ultrahapticModel;

    [SerializeField]
    private GameObject _hapticElements;

    [SerializeField]
    private CollisionToSensation _collisionToSensation;

    [SerializeField]
    private GameObject _blueSate;
    [SerializeField]
    private GameObject _neutralSate;
    [SerializeField]
    private GameObject _redSate;

    private (ColorSelection, bool) sceneConfig;

    void Start()
    {
        Debug.Log($"Started - {_physicalState} scene");

        sceneConfig = _studyManager.GetRandomElementOfTrialList(_physicalState);
        _studyManager.currentSceneConfig = sceneConfig;

        SetupSceneElements();

        //_studyManager.currentStudyBlock -> 0 = start scene, 1 = first physical state, 2 = second, 3 = third state
        //_studyManager.AppendCSVLine($"Block {_studyManager.currentStudyBlock}:{_studyManager._dataSeperator}{_physicalState}{_studyManager._dataSeperator}Config:{_studyManager._dataSeperator}{sceneConfig}");
    }

    private void SetupSceneElements()
    {
        SetupHaptics();

        SetupColors();
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

        // disable ultrahaptics Model
        _ultrahapticModel.SetActive(addHaptics);
    }
}
