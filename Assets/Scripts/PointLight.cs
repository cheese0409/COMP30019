using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLight : MonoBehaviour
{
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        GameObject terrain = GameObject.Find("LandScape");
        DiamondSquare script = terrain.GetComponent<DiamondSquare>();

        transform.position = new Vector3(0.0f, 0.0f, script.LandSize); ;

    }

    // Update is called once per frame
    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
}
