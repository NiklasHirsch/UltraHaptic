using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateCollision : MonoBehaviour
{

    public CollisionToSensation collisionToSensation;
    private void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision entered = {collision.gameObject.name}");
    }
    private void OnCollisionStay(Collision collisionInfo)
    {
        Debug.Log($"Contact = {collisionInfo.gameObject.name}");

        if (collisionInfo.gameObject.tag.Equals("Hand"))
        {
            // Debug-draw all contact points and normals
            foreach (ContactPoint contact in collisionInfo.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal * 10, Color.green);
            }
        }
        
    }

    
    private void OnTriggerStay(Collider other)
    {
        RaycastHit hit;
        if (Physics.Raycast(other.transform.position, other.transform.forward, out hit))
        {
            Debug.Log("Point of contact: " + hit.point);

            Debug.Log("Closest Point: " + other.ClosestPoint(hit.point));
            //SetSensation(hit);
        }
    }

    void SetSensation(RaycastHit hit)
    {
        Vector3[] points = new[] {
                hit.point,
                hit.point,
                hit.point,
                hit.point,
                hit.point,
                hit.point,
            };

        collisionToSensation.SetPath(points);
    }
}
