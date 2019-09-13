using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water : MonoBehaviour
{
    public Shader shader;
    public RotationOrigin pointLight;

    // Start is called before the first frame update
    void Start()
    {
        MeshFilter water = this.gameObject.AddComponent<MeshFilter>();
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        water.mesh = plane.GetComponent<MeshFilter>().mesh;
        Destroy(plane);
        water.gameObject.AddComponent<MeshCollider>();
        GameObject terrain = GameObject.Find("LandScape");
        DiamondSquare script = terrain.GetComponent<DiamondSquare>();
        transform.position = new Vector3(0, -script.maxHeight, 0);
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = shader;
        renderer.material.color = new Color (0, 0, 255, 1);
    }

    // Update is called once per frame
    void Update()
    {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        // Pass updated light positions to shader
        GameObject terrain = GameObject.Find("LandScape");
        DiamondSquare script = terrain.GetComponent<DiamondSquare>();
        transform.position = new Vector3(0, 10+script.currMinHeight, 0);
        renderer.material.SetColor("_PointLightColor", this.pointLight.color);
        renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
    }
}
