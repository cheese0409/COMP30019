using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

public class CameraControl : MonoBehaviour
{
    private float mouseSpeed = 0.1f;
    private float keyboard = 0.1f;
    private Vector3 mousePos;
   

    private void Start()
    {
        GameObject terrain = GameObject.Find("LandScape");
        DiamondSquare script = terrain.GetComponent<DiamondSquare>();
        transform.position = new Vector3(0, script.maxHeight*2, script.LandSize / 2);
        transform.LookAt(new Vector3(10, 0, 0));
        mousePos = Input.mousePosition;
    }

    private void Update()
    {
        checkOutOfBound();
        keyboardControl();
        cameraRotate();
        
    }


    void cameraRotate()
    {
        mousePos = Input.mousePosition - mousePos;
        mousePos = new Vector3(-mousePos.y * mouseSpeed, mouseSpeed * mousePos.x, 0);
        mousePos = new Vector3(transform.eulerAngles.x + mousePos.x, transform.eulerAngles.y + mousePos.y, 0);

        if (mousePos.x > 80 && mousePos.x < 90)
        {
            mousePos.x = 80;
        }
        transform.eulerAngles = mousePos;
        mousePos = Input.mousePosition;
    }

    void keyboardControl()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(Input.GetAxis("Horizontal") * keyboard, 0, 0);
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(0, 0, Input.GetAxis("Vertical") * keyboard);
        }
    }

    // stop camera moving when hitting the Rigidbody
    void FixedUpdate(){
      Vector3 v = new Vector3(0, 0, 0);
      this.gameObject.GetComponent<Rigidbody>().velocity = v;
    }

    void checkOutOfBound(){
      if (this.transform.localPosition.x < -20){
        this.transform.localPosition = new Vector3(-20.0f, transform.localPosition.y, transform.localPosition.z);
      }

      if(this.transform.localPosition.x > 20){
        this.transform.localPosition = new Vector3(20.0f, transform.localPosition.y, transform.localPosition.z);
      }

      if(this.transform.localPosition.z > 20){
        this.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 20.0f);
      }

      if(this.transform.localPosition.z < -20){
        this.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -20.0f);
      }

      if(this.transform.localPosition.y > 30){
        this.transform.localPosition = new Vector3(transform.localPosition.x, 30.0f, transform.localPosition.z);
      }
    }
}
