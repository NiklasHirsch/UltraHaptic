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
    private GameObject _hapticElements;

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

        SetupSceneElements();

        //_studyManager.currentStudyBlock -> 0 = start scene, 1 = first physical state, 2 = second, 3 = third state
        _studyManager.AppendCSVLine($"Block {_studyManager.currentStudyBlock}:{_studyManager._dataSeperator}{_physicalState}{_studyManager._dataSeperator}Config:{_studyManager._dataSeperator}{sceneConfig}");
    }

    private void SetupSceneElements()
    {
        _hapticElements.SetActive(sceneConfig.Item2);
        _blueSate.SetActive(sceneConfig.Item1 == ColorSelection.Blue);
        _neutralSate.SetActive(sceneConfig.Item1 == ColorSelection.Neutral);
        _redSate.SetActive(sceneConfig.Item1 == ColorSelection.Red);
    }
}
