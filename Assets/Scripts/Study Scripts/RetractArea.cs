using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetractArea : MonoBehaviour
{
    public int collisionCount = 0;

    public bool IsNotColliding
    {
        get { return collisionCount == 0; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Bone"))
        {
            collisionCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Bone"))
        {
            collisionCount--;
        }
    }
}
