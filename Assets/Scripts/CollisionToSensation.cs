using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltrahapticsCoreAsset;

public class CollisionToSensation : MonoBehaviour
{
    [SerializeField]
    private bool hasHealthPotion = true;

    public Transform Point0;
    public Transform Point1;
    public Transform Point2;
    public Transform Point3;
    public Transform Point4;
    public Transform Point5;

    public LineRenderer LineRenderer;
    public SensationSource Sensation;
    private float scaling_ = 1f;

    void Start()
    {
        LineRenderer.positionCount = 6;
        Point0.position = Sensation.Inputs["point0"].Value * scaling_;
        Point1.position = Sensation.Inputs["point1"].Value * scaling_;
        Point2.position = Sensation.Inputs["point2"].Value * scaling_;
        Point3.position = Sensation.Inputs["point3"].Value * scaling_;
        Point4.position = Sensation.Inputs["point4"].Value * scaling_;
        Point5.position = Sensation.Inputs["point5"].Value * scaling_;
    }

    private UnityEngine.Vector3[] PolylinePoints()
    {
        UnityEngine.Vector3[] points = new[] {
            Point0.position,
            Point1.position,
            Point2.position,
            Point3.position,
            Point4.position,
            Point5.position
        };
        return points;
    }

    void Update()
    {
        LineRenderer.SetPositions(PolylinePoints());
        Sensation.Inputs["point0"].Value = ModifyPositionData(Point0.position) / scaling_;
        Sensation.Inputs["point1"].Value = ModifyPositionData(Point1.position) / scaling_;
        Sensation.Inputs["point2"].Value = ModifyPositionData(Point2.position) / scaling_;
        Sensation.Inputs["point3"].Value = ModifyPositionData(Point3.position) / scaling_;
        Sensation.Inputs["point4"].Value = ModifyPositionData(Point4.position) / scaling_;
        Sensation.Inputs["point5"].Value = ModifyPositionData(Point5.position) / scaling_;
    }

    private Vector3 ModifyPositionData(Vector3 point)
    {
        Vector3 result = point;
        var yValue = point.y;
        result.y = result.z;
        result.z = yValue;
        result.z = result.z; //- 0.2f;// value is the height of the gameobject
        return result;
        //return point;
    }

    public void SetPath(Vector3[] points)
    {
        Point0.position = points[0];
        Point1.position = points[1];
        Point2.position = points[2];
        Point3.position = points[3];
        Point4.position = points[4];
        Point5.position = points[5];
    }

    public void SetSensationEnabledStatus(bool enabled)
    {
        if(Sensation.enabled == !enabled)
        {
            Sensation.enabled = enabled;
        }

    }
}
