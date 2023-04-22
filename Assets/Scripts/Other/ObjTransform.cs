//by TheSuspect

using UnityEngine;

public class ObjTransform : MonoBehaviour {

    public Transform target1;
    public Transform target2;
    public Vector3 posOffset;
    public Quaternion rotOffset;

    public bool onlyPos;


    void Update() {

        if (onlyPos)
        {
            transform.position = target1.transform.position + posOffset;
        }
        else
        {
            var targetPos = target1.position - posOffset;
            var targetRot = target1.rotation * rotOffset;
            
            transform.position = RotatePointAroundPivot(targetPos, target1.position, targetRot);
            transform.localRotation = targetRot;
        }
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
     {
         //Get a direction from the pivot to the point
         Vector3 dir = point - pivot;
         //Rotate vector around pivot
         dir = rotation * dir; 
         //Calc the rotated vector
         point = dir + pivot; 
         //Return calculated vector
         return point; 
     }
}
