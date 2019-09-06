﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquare : MonoBehaviour
{
    public int nPoints;
    public float LandSize;
    public float maxHeight;
    private float maxGenHeight;
    public Shader shader;
    public Material material;
    MeshCollider collider;
    private Vector3[] allVerts;

    void Start()
    {
        maxGenHeight = -maxHeight;
        MeshFilter landscape = this.gameObject.AddComponent<MeshFilter>();
        landscape.mesh = this.createLandscape();

        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = this.shader;
        //renderer.material.color = 
       // createLandscape();
    }

    private void Update()
    {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
    }

    Mesh createLandscape(){
        int numVerts = nPoints * nPoints;
        int numGap = this.nPoints - 1;
        float halfLength = 0.5f * LandSize;
        float step = LandSize / numGap;
        int triIndex = 0;
        

        allVerts = new Vector3[numVerts];
        Vector2[] uvs = new Vector2[numVerts];
        int[] triangles = new int [numGap*numGap*6];

        Mesh m = new Mesh();
        GetComponent<MeshFilter>().mesh = m;

        for (int i=0; i < nPoints; i++){
            for (int j=0; j < nPoints; j++){
                allVerts[nPoints*i+j] = new Vector3 (step*j-halfLength, 0.0f, halfLength-step*i);
                uvs[i*nPoints+j] = new Vector2 ((float)i/numGap, (float)j/numGap);

                if (i<numGap && j<numGap){
                  int top = i*nPoints + j;
                  int bot = (i+1)*nPoints +j;

                  triangles[triIndex] = top;
                  triangles[triIndex+1] = top + 1;
                  triangles[triIndex+2] = bot;

                  triangles[triIndex+3] = top + 1;
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

                  diamond(top, bot, mid, squareSize, maxHeight);
                  square(top, bot, mid, squareSize, maxHeight);

                  currentcol += squareSize;

                }
                currentline += squareSize;

            }
            splitInto *= 2;
            squareSize /= 2;
            maxHeight *= 0.5f;

        }

        for(int i = 0; i<allVerts.Length; i++)
        {
            if (allVerts[i].y > maxGenHeight)
            {
                maxGenHeight = allVerts[i].y;
            }
        }

        double snowLine = 0.8 * maxGenHeight;
        double rockLine = 0.6 * maxGenHeight;
        double grassLine = 0.3 * maxGenHeight;
        double sandLine = -0.1 * maxGenHeight;
        double shallowSea = -0.3 * maxGenHeight;

        

        Color[] color = new Color[allVerts.Length];


        for (int i =0; i<allVerts.Length; i++)
        {
            float currentHeight = allVerts[i].y;

            if (currentHeight >= snowLine)
            {
                color[i] = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else if (currentHeight >= rockLine && currentHeight < snowLine)
            {
                color[i] = Color.gray;
            }else if(currentHeight >= grassLine && currentHeight < rockLine)
            {
                color[i] = new Color(0.0f, 0.45f, 0.0f, 1.0f);
            }else if(currentHeight >= sandLine && currentHeight < grassLine)
            {
                color[i] = new Color(0.5f, 0.5f, 0.0f, 1.0f);
            }
            else if(currentHeight >= shallowSea && currentHeight < sandLine)
            {
                color[i] = new Color(0.2f, 0.5f, 0.8f, 1.0f);
            }else if(currentHeight < shallowSea)
            {
                color[i] = Color.blue;
            }
        }
     
        m.vertices = allVerts;
        m.triangles = triangles;
       // m.uv = uvs;

        m.colors = color;
        m.RecalculateBounds();
        m.RecalculateNormals();
       
        collider = GetComponent<MeshCollider>();
        collider.sharedMesh = m;
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
