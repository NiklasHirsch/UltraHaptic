using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupQuestionnaireScene : MonoBehaviour
{

    [SerializeField] private ScriptableStudyManager _studyManager;

    [SerializeField] GameObject interactionOVRRig;

    void Start()
    {
        SetupOVRPosition();
    }

    private void SetupOVRPosition()
    {
        // Setup Position
        interactionOVRRig.transform.position = _studyManager.participantPos;

        // Setup Rotation
        interactionOVRRig.transform.rotation = _studyManager.participantRot;
    }

}
