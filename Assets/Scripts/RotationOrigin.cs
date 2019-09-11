using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationOrigin : MonoBehaviour
{
    public float speed;
  
    // Update is called once per frame
    void Update()
    {
        this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * speed, Vector3.right);
    }
}
