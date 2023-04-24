using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewItem : MonoBehaviour
{
    public Transform model;
    public Vector3 rotation;
    public float speed;

    void Update()
    {
        transform.Rotate(rotation * speed * Time.deltaTime);
    }
}
