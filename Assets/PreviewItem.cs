using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewItem : MonoBehaviour
{
    public static PreviewItem Instance;

    public Transform model;
    public Vector3 rotation;
    public float speed;

    public float timeToStart = 1f;
    private float timer;

    public bool isInspecting;

    private void Start() { Instance = this; SetMaxTimer(); }

    public void SetMaxTimer(){timer = timeToStart;}

    void Update()
    {
        if(timer > 0) timer-=Time.deltaTime;
        else if(!isInspecting) transform.Rotate(rotation * speed * Time.deltaTime);

        if(isInspecting) transform.Rotate(new Vector3(InputManager.Instance.movementInput.y, -InputManager.Instance.movementInput.x) * speed * Time.deltaTime);
    }

    public void SetModel(GameObject _model){
        if(model.childCount != 0) Destroy(model.GetChild(0).gameObject);
        Instantiate(_model, model);
    }

    public void RemoveModel(){
        if(model.childCount != 0) Destroy(model.GetChild(0).gameObject);
    }
}
