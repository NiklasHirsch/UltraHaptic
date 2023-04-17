using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOrientation : MonoBehaviour
{
	[SerializeField] Transform resetTransform;
	[SerializeField] GameObject player;
	[SerializeField] Camera playerHead;

	public void ResetPosition()
	{
		//OVRManager.display.RecenterPose();

		var roationAngleY = resetTransform.rotation.eulerAngles.y - playerHead.transform.rotation.eulerAngles.y;
		player.transform.Rotate(0, roationAngleY, 0);

		var distanceDiff = resetTransform.position - playerHead.transform.position;
		player.transform.position += distanceDiff;
	}
}
