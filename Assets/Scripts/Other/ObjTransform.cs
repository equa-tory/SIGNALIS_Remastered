//by TheSuspect

using UnityEngine;

public class ObjTransform : MonoBehaviour {

    public Transform target;
    public Vector3 posOffset;
    public Quaternion rotOffset = Quaternion.identity;

    public bool onlyPos;


    void Update() {

        if (onlyPos)
        {
            transform.position = target.transform.position + posOffset;
        }
        else
        {
            var targetPos = target.position - posOffset;
            var targetRot = target.rotation * rotOffset;
            
            transform.position = RotatePointAroundPivot(targetPos, target.position, targetRot);
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
