using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseManager : MonoBehaviour
{
    private InputManager input;

    public float useDistance;
    public float radius;

    private void Start() {
        input = InputManager.Instance;
    }

    private void Update() {
        if(input.use_Input){
            if(Physics.SphereCast(transform.position, radius, transform.forward, out RaycastHit hit, useDistance)){

                if(hit.collider.GetComponent<WorldItem>()) hit.collider.GetComponent<WorldItem>().PickUp();

            }
        }
    }
}
