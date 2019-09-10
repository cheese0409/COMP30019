﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquare : MonoBehaviour
{
    public int nPoints;
    public float LandSize;
    public float maxHeight;
    public Shader shader;
    public Material material;
    private float currMaxHeight;


  
    public PointLight pointLight;

    Vector3[] allVerts;

    void Start()
    {
        MeshFilter landscape = this.gameObject.AddComponent<MeshFilter>();
        landscape.mesh = this.createLandscape();

        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material= this.material;
        renderer.material.shader = this.shader;

        MeshCollider mc = this.gameObject.GetComponent<MeshCollider>();
        mc.sharedMesh = landscape.mesh;

        currMaxHeight = -maxHeight;
    }
    void Update()
    {
        // Get renderer component (in order to pass params to shader)
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader
        //renderer.material.SetColor("_PointLightColor", this.pointLight.color);
        //renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
    }


    Mesh createLandscape(){
        int numVerts = nPoints * nPoints;
        int numGap = this.nPoints - 1;
        float halfLength = 0.5f * LandSize;
        float step = LandSize / numGap;
        int triIndex = 0;

        Mesh m = new Mesh();

        allVerts = new Vector3[numVerts];
        Vector2[] uvs = new Vector2[numVerts];
        int[] triangles = new int [numGap*numGap*6];


        for (int i=0; i < nPoints; i++){
            for (int j=0; j < nPoints; j++){
                allVerts[nPoints*i+j] = new Vector3 (step*j-halfLength, 0.0f, halfLength-step*i);
                uvs[i*nPoints+j] = new Vector2 ((float)i/numGap, (float)j/numGap);

                if (i<numGap && j<numGap){
                  int top = i*nPoints + j;
                  int bot = (i+1)*nPoints +j;

                  triangles[triIndex] = top;
                  triangles[triIndex+1] = top + 1;
                  triangles[triIndex+2] = bot + 1;

                  triangles[triIndex+3] = top;
                  triangles[triIndex+4] = bot + 1;
                  triangles[triIndex+5] = bot;

                  triIndex += 6;
                }
            }
        }

        //initialize the height for four corner points
        allVerts[0].y = Random.Range(-maxHeight,maxHeight);
        allVerts[numGap].y = Random.Range(-maxHeight,maxHeight);
        allVerts[allVerts.Length-1].y = Random.Range(-maxHeight,maxHeight);
        allVerts[allVerts.Length-1-numGap].y = Random.Range(-maxHeight,maxHeight);

        //splitInto how many Squares
        int splitInto = 1;
        int squareSize = numGap;

        for (int i = 0;  i<(int)Mathf.Log(numGap,2); i++){
            int currentline = 0;
            for (int j = 0; j<splitInto; j++){
                int currentcol = 0;
                for (int k = 0; k<splitInto; k++){

                  int half = (int)(squareSize*0.5f);
                  int top = currentline*nPoints + currentcol;
                  int bot = (currentline+squareSize)*nPoints + currentcol;
                  int mid = (int)((currentline+half)*nPoints)+ (int)(currentcol + half);

                  diamond(top, bot, mid, squareSize, 0.7f*maxHeight);
                  square(top, bot, mid, squareSize, 0.7f*maxHeight);

                  currentcol += squareSize;

                }
                currentline += squareSize;

            }
            splitInto *= 2;
            squareSize /= 2;
            maxHeight *= 0.5f;

        }
        // find the max height
        for ( int i=0; i< numVerts; i++)
        {
            if(allVerts[i].y > currMaxHeight)
            {
                currMaxHeight = allVerts[i].y;
            }
        }

        Color[] terrainColor = new Color[numVerts];

        for (int i = 0; i < allVerts.Length; i++)
        {
            if (allVerts[i].y > currMaxHeight * 0.75)
            {

                terrainColor[i] = new Color(1.0f, 1.0f, 1.0f, 1.0f); //snow

            }
            else if (allVerts[i].y > currMaxHeight * 0.55 && allVerts[i].y < currMaxHeight * 0.75)
            {

                terrainColor[i] = Color.grey; //rock

            }
            else if (allVerts[i].y > -currMaxHeight * 0.1 && allVerts[i].y < currMaxHeight * 0.55)
            {

                terrainColor[i] = new Color(0.0f, 0.45f, 0.0f, 1.0f); // grassland

            }
            else if (allVerts[i].y > -currMaxHeight * 0.2 && allVerts[i].y < -currMaxHeight * 0.1)
            {

                terrainColor[i] = new Color(0.5f, 0.5f, 0.0f, 1.0f); // beach

            }
            else if (allVerts[i].y > -currMaxHeight * 0.3 && allVerts[i].y < -currMaxHeight * 0.2)
            {

                terrainColor[i] = new Color(0.2f, 0.5f, 0.8f, 1.0f); //shallow water

            }
            else if (allVerts[i].y < -currMaxHeight * 0.3)
            {

                terrainColor[i] = Color.blue; //ocean

            }

        }

        m.vertices = allVerts;
        m.uv = uvs;
        m.colors = terrainColor;
        m.triangles = triangles;
        m.RecalculateBounds();
        m.RecalculateNormals();
       
        return m;

    }

    void diamond(int top, int bot, int mid, int size, float noise ){
        allVerts[mid].y = (allVerts[top].y + allVerts[top+size].y + allVerts[bot+size].y + allVerts[bot].y)*0.25f + Random.Range(-noise, noise);
    }

    void square(int top, int bot, int mid, int size, float noise){
        int half = (int)(size*0.5f);

        allVerts[top+half].y = (allVerts[top].y + allVerts[top+size].y + allVerts[mid].y)/3 + Random.Range(-noise, noise);
        allVerts[mid-half].y = (allVerts[top].y + allVerts[bot].y + allVerts[mid].y)/3 + Random.Range(-noise, noise);
        allVerts[mid+half].y = (allVerts[top+size].y + allVerts[mid].y + allVerts[bot+size].y)/3 + Random.Range(-noise, noise);
        allVerts[bot+half].y = (allVerts[bot+size].y + allVerts[bot].y + allVerts[mid].y)/3 + Random.Range(-noise, noise);
    }
}
