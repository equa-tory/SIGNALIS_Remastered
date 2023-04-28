using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseManager : MonoBehaviour
{
    private InputManager input;

    public Camera cam;
    public float useDistance;
    public float radius;

    private void Start() {
        input = InputManager.Instance;
    }

    private void Update() {
        if(input.use_Input){
            if(Physics.SphereCast(cam.transform.position, radius, cam.transform.forward*5, out RaycastHit hit, useDistance)){

                if(hit.collider.GetComponent<WorldItem>()) hit.collider.GetComponent<WorldItem>().PickUp();

            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(cam.transform.position, cam.transform.forward*5);
    }
}
