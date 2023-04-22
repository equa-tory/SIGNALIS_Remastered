using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public InputManager input;

    public Transform targetTransform;
    public Transform cameraPivot;
    public Transform cam;
    public Vector3 offset;
    public LayerMask collisionLayer;
    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPos;

    public float camCollisionOffSet = 0.2f;
    public float minimumCollisionOffSet = 0.2f;
    public float camCollisionRadius = 2f;
    [Range(0,1)]public float cameraFollowSpeed = 0.2f;
    [Range(0,1)]public float cameraLookSpeed = 2f;
    public float cameraPivotSpeed = 2f;

    public float lookAngel;
    public float pivotAngel;
    public float minimumPivotAngle = -35f;
    public float maximumPivotAngle = 35f;


    private void Awake()
    {
        defaultPosition = cam.localPosition.z;

    }

    public void AllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        CameraCollision();
    }

    private void FollowTarget()
    {
        Vector3 targetPostion = Vector3.SmoothDamp
                (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPostion;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRot;

        lookAngel = lookAngel + (input.cameraInputX * cameraLookSpeed);
        pivotAngel = pivotAngel - (input.cameraInputY * cameraPivotSpeed);
        pivotAngel = Mathf.Clamp(pivotAngel, minimumPivotAngle, maximumPivotAngle);

        // rotation = Vector3.zero;
        // targetRot = Quaternion.Euler(rotation);
        // transform.rotation = targetRot;

        rotation = Vector3.zero;
        rotation.y = lookAngel;
        rotation.x = pivotAngel;
        targetRot = Quaternion.Euler(rotation);
        transform.localRotation = targetRot;
    }

    private void CameraCollision()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 dir = cam.position - cameraPivot.position;
        dir.Normalize();

        if (Physics.SphereCast
            (cameraPivot.position, camCollisionRadius, dir, out hit, Mathf.Abs(targetPosition), collisionLayer))
        {
            float distance = Vector3.Distance(targetTransform.position, hit.point);
            targetPosition = -(distance - camCollisionOffSet);
        }

        if(Mathf.Abs(targetPosition) < minimumCollisionOffSet)
        {
            targetPosition = targetPosition - minimumCollisionOffSet;
        }

        cameraVectorPos.z = Mathf.Lerp(cam.localPosition.z, targetPosition, 0.2f);
        cam.localPosition = cameraVectorPos;
    }
   
    

}
