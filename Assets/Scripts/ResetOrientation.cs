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
	[SerializeField] float thumbRadius = 0.011f;
	[SerializeField] float indexRadius = 0.007f;

	[Header("Hand Right")]
	[SerializeField] Transform resetTransformWristRightHand;
	[SerializeField] Transform resetTransformIndexRight;
	[SerializeField] Transform resetTransformThumbRight;
	[SerializeField] GameObject rightHand;
	[SerializeField] GameObject rightHandIndex;
	[SerializeField] GameObject rightHandThumb;

	[Header("Hand Left")]
	[SerializeField] Transform resetTransformWristLeftHand;
	[SerializeField] Transform resetTransformIndexLeft;
	[SerializeField] Transform resetTransformThumbLeft;
	[SerializeField] GameObject leftHand;
	[SerializeField] GameObject leftHandIndex;
	[SerializeField] GameObject leftHandThumb;

	private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
			ResetPositionToHandsImproved();
			//ResetPositionToHandsImprovedRightHand();
        }
    }

	public void ResetPositionToHandsImproved()
	{
		// ------------- Rotation --------------
		var rotRIndex = resetTransformIndexRight.rotation.eulerAngles.y - rightHandIndex.transform.rotation.eulerAngles.y;
		var rotRThumb = resetTransformThumbRight.rotation.eulerAngles.y - rightHandThumb.transform.rotation.eulerAngles.y;
		var middleRotR = (rotRIndex + rotRThumb) / 2;

		var rotLIndex = resetTransformIndexLeft.rotation.eulerAngles.y - leftHandIndex.transform.rotation.eulerAngles.y;
		var rotLThumb = resetTransformThumbLeft.rotation.eulerAngles.y - leftHandThumb.transform.rotation.eulerAngles.y;
		var middleRotL = (rotLIndex + rotLThumb) / 2;

		var combinedRotation = (middleRotR + middleRotL) / 2;
		player.transform.Rotate(0, combinedRotation, 0);


		// ------------- POS: X, Y, Z --------------
		// X & Z
		var rightDistanceDiff = resetTransformWristRightHand.position - rightHand.transform.position;
		var leftDistanceDiff = resetTransformWristLeftHand.position - leftHand.transform.position;

		// Right X,Z & Y
		var deskdRIndex = rightHandIndex.transform.position.y - indexRadius;
		var deskdRThumb = rightHandThumb.transform.position.y - thumbRadius;

		var indexRYDistanceDiff = resetTransformIndexRight.position.y - deskdRIndex;
		var thumbRYDistanceDiff = resetTransformThumbRight.position.y - deskdRThumb;
		var middleRYDistanceDiff = (indexRYDistanceDiff + thumbRYDistanceDiff) / 2;

		var rightAdaptedYDiff = new Vector3(rightDistanceDiff.x, middleRYDistanceDiff, rightDistanceDiff.z);

		// Left X,Z & Y
		var deskdLIndex = leftHandIndex.transform.position.y - indexRadius;
		var deskdLThumb = leftHandThumb.transform.position.y - thumbRadius;

		var indexLYDistanceDiff = resetTransformIndexLeft.position.y - deskdLIndex;
		var thumbLYDistanceDiff = resetTransformThumbLeft.position.y - deskdLThumb;
		var middleLYDistanceDiff = (indexLYDistanceDiff + thumbLYDistanceDiff) / 2;

		var leftAdaptedYDiff = new Vector3(leftDistanceDiff.x, middleLYDistanceDiff, leftDistanceDiff.z);

		var combinedDiff = (rightAdaptedYDiff + leftAdaptedYDiff) / 2;
		player.transform.position += combinedDiff;

		// Set Study Manager Values to set all new scenes to the calibrated Position.
		_studymanager.participantPos = player.transform.position;
		_studymanager.participantRot = player.transform.rotation;
	}

	public void ResetPositionToHandsImprovedRightHand()
	{
		// ------------- Rotation --------------
		var rotRIndex = resetTransformIndexRight.rotation.eulerAngles.y - rightHandIndex.transform.rotation.eulerAngles.y;
		var rotRThumb = resetTransformThumbRight.rotation.eulerAngles.y - rightHandThumb.transform.rotation.eulerAngles.y;
		var middleRotR = (rotRIndex + rotRThumb) / 2;

		var rotLIndex = resetTransformIndexLeft.rotation.eulerAngles.y - leftHandIndex.transform.rotation.eulerAngles.y;
		var rotLThumb = resetTransformThumbLeft.rotation.eulerAngles.y - leftHandThumb.transform.rotation.eulerAngles.y;
		var middleRotL = (rotLIndex + rotLThumb) / 2;

		var combinedRotation = (middleRotR + middleRotL) / 2;
		player.transform.Rotate(0, middleRotR, 0);  // TODO use combinedRotation


		// ------------- POS: X, Y, Z --------------
		// X & Z
		var rightDistanceDiff = resetTransformWristRightHand.position - rightHand.transform.position;
		var leftDistanceDiff = resetTransformWristLeftHand.position - leftHand.transform.position;

		// Right X,Z & Y
		var deskdRIndex = rightHandIndex.transform.position.y - indexRadius;
		var deskdRThumb = rightHandThumb.transform.position.y - thumbRadius;

		var indexRYDistanceDiff = resetTransformIndexRight.position.y - deskdRIndex;
		var thumbRYDistanceDiff = resetTransformThumbRight.position.y - deskdRThumb;
		var middleRYDistanceDiff = (indexRYDistanceDiff + thumbRYDistanceDiff) / 2;

		var rightAdaptedYDiff = new Vector3(rightDistanceDiff.x, middleRYDistanceDiff, rightDistanceDiff.z);

		// Left X,Z & Y
		var deskdLIndex = leftHandIndex.transform.position.y - indexRadius;
		var deskdLThumb = leftHandThumb.transform.position.y - thumbRadius;

		var indexLYDistanceDiff = resetTransformIndexLeft.position.y - deskdLIndex;
		var thumbLYDistanceDiff = resetTransformThumbLeft.position.y - deskdLThumb;
		var middleLYDistanceDiff = (indexLYDistanceDiff + thumbLYDistanceDiff) / 2;

		var leftAdaptedYDiff = new Vector3(leftDistanceDiff.x, middleLYDistanceDiff, leftDistanceDiff.z);

		var combinedDiff = (rightAdaptedYDiff + leftAdaptedYDiff) / 2;
		player.transform.position += rightAdaptedYDiff; // TODO use combinedDiff

		// Set Study Manager Values to set all new scenes to the calibrated Position.
		_studymanager.participantPos = player.transform.position;
		_studymanager.participantRot = player.transform.rotation;
	}
	public void ResetPositionToHands()
	{
		// Rotation
		var addRot = 190;
		var rightRoationAngleY = resetTransformWristRightHand.rotation.eulerAngles.y - rightHand.transform.rotation.eulerAngles.y;
		var leftRoationAngleY = resetTransformWristLeftHand.rotation.eulerAngles.y - leftHand.transform.rotation.eulerAngles.y;
		var combinedRotation = (rightRoationAngleY + leftRoationAngleY) / 2;
		player.transform.Rotate(0, rightRoationAngleY + addRot, 0); // TODO combinedRotation

		// X & Z
		var rightDistanceDiff = resetTransformWristRightHand.position - rightHand.transform.position;
		var leftDistanceDiff = resetTransformWristLeftHand.position - leftHand.transform.position;
		var combinedDiff = (rightDistanceDiff + leftDistanceDiff) / 2;
		player.transform.position += rightDistanceDiff; // TODO combinedDiff

		_studymanager.participantPos = player.transform.position;
		//_studymanager.participantRot = player.transform.rotation;
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
