using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Water Script
/// </summary>

public class water : MonoBehaviour
{
    public Shader shader;
    public PointLight pointLight;

    // Start is called before the first frame update
    void Start()
    {
        // Create a new plane instance and assigned to water MeshFilter
        MeshFilter water = this.gameObject.AddComponent<MeshFilter>();
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        water.mesh = plane.GetComponent<MeshFilter>().mesh;

        // Remove the instance
        Destroy(plane);
        water.gameObject.AddComponent<MeshCollider>();

        // Get terrain object and the maxHeight variable in order to make sure the water is initially generated at the lowest height level
        GameObject terrain = GameObject.Find("LandScape");
        DiamondSquare script = terrain.GetComponent<DiamondSquare>();
        transform.position = new Vector3(0, -script.maxHeight, 0);
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();

        // Set the initial color and shader
        renderer.material.shader = shader;
        renderer.material.color = new Color (0, 128/255f, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        // Pass updated light positions to shader
        GameObject terrain = GameObject.Find("LandScape");
        DiamondSquare script = terrain.GetComponent<DiamondSquare>();
        transform.position = new Vector3(0, (script.currMaxHeight+script.currMinHeight)/2, 0);
        renderer.material.SetColor("_PointLightColor", this.pointLight.color);
        renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
    }
}
