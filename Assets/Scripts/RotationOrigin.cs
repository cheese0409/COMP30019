using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Refer to Lab 2 Code XAxisSpin.cs
public class RotationOrigin : MonoBehaviour
{
    public float speed;
    public Color color;

    // Update is called once per frame
    void Update()
    {
        this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * speed, Vector3.right);
    }

    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
}
