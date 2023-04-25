using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOrientation : MonoBehaviour
{
	[SerializeField] ScriptableStudyManager _studymanager;
	[SerializeField] Transform resetTransform;
	[SerializeField] GameObject player;
	[SerializeField] Camera playerHead;

	[Header("Hand Reset")]
	[SerializeField] Transform resetTransformRightHand;
	[SerializeField] Transform resetTransformLeftHand;
	[SerializeField] GameObject rightHand;
	[SerializeField] GameObject leftHand;

	private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
			ResetPositionToHands();
        }
    }

	public void ResetPositionToHands()
	{
		//var roationAngleY = resetTransformRightHand.rotation.eulerAngles.y - playerHead.transform.rotation.eulerAngles.y;
		//player.transform.Rotate(0, roationAngleY, 0);

		// TODO X & Z
		var rightDistanceDiff = resetTransformRightHand.position - rightHand.transform.position;
		var leftDistanceDiff = resetTransformLeftHand.position - leftHand.transform.position;
		var combinedDiff = (rightDistanceDiff + leftDistanceDiff) / 2;
		player.transform.position += rightDistanceDiff;

		_studymanager.participantPos = player.transform.position;
	}

	public void ResetPosition()
	{
		//OVRManager.display.RecenterPose();

		var roationAngleY = resetTransform.rotation.eulerAngles.y - playerHead.transform.rotation.eulerAngles.y;
		player.transform.Rotate(0, roationAngleY, 0);

		var distanceDiff = resetTransform.position - playerHead.transform.position;
		player.transform.position += distanceDiff;
	}
}
