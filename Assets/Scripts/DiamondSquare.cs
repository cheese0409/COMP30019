//general ideas from https://www.youtube.com/watch?v=1HV8GbFnCik
// modify and apply on for this particular problem
// also made use of COMP30019 multiple workshops' code

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DiamondSquare : MonoBehaviour
{
    public int nPoints;
    public float LandSize;
    public float maxHeight;
    public float waterLevel; // midpoint of landscape
    public float mountainHeight;//height of mountain above waterlevel
    public Shader shader;
    public float currMinHeight;
    public float currMaxHeight;
    public PointLight PointLight;
    MeshCollider mc;
    private Vector3[] allVerts;

    void Start()
    {
        MeshFilter landscape = this.gameObject.AddComponent<MeshFilter>();
        landscape.mesh = this.createLandscape();

        mc = this.gameObject.GetComponent<MeshCollider>();
        mc.sharedMesh = landscape.mesh;

        MeshRenderer lanscapeRenderer = this.gameObject.AddComponent<MeshRenderer>();
        lanscapeRenderer.material.shader = shader;
    }

    void Update()
    {
        // Get renderer component (in order to pass params to shader)
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader
        renderer.material.SetColor("_PointLightColor", this.PointLight.color);
        renderer.material.SetVector("_PointLightPosition", this.PointLight.GetWorldPosition());
    }


    Mesh createLandscape(){
        int numVerts = nPoints * nPoints;
        int numGap = this.nPoints - 1;
        float halfLength = 0.5f * LandSize;
        float step = LandSize / numGap;
        float varHeight = maxHeight;
        int triIndex = 0;

        Mesh m = new Mesh();

        allVerts = new Vector3[numVerts];
        Vector2[] uvs = new Vector2[numVerts];
        int[] triangles = new int [numGap*numGap*6];

        //create triangles
        for (int i=0; i < nPoints; i++){
            for (int j=0; j < nPoints; j++){
                allVerts[nPoints*i+j] = new Vector3 (step*j-halfLength, 0.0f, halfLength-step*i);
                uvs[i*nPoints+j] = new Vector2 ((float)i/numGap, (float)j/numGap);

                //one square needs two triangles
                //and each triangle needs three vertices
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

        //initialize the height for four corner points randomly
        allVerts[0].y = UnityEngine.Random.Range(-varHeight,varHeight);
        allVerts[numGap].y = UnityEngine.Random.Range(-varHeight,varHeight);
        allVerts[allVerts.Length-1].y = UnityEngine.Random.Range(-varHeight,varHeight);
        allVerts[allVerts.Length-1-numGap].y = UnityEngine.Random.Range(-varHeight,varHeight);

        //splitInto how many Squares
        int splitInto = 1;
        int squareSize = numGap;

        //DiamondSquare algorithm Implementation
        for (int i = 0;  i<(int)Mathf.Log(numGap,2); i++){
            int currentline = 0;
            for (int j = 0; j<splitInto; j++){
                int currentcol = 0;
                for (int k = 0; k<splitInto; k++){

                  int half = (int)(squareSize*0.5f);
                  int top = currentline*nPoints + currentcol;
                  int bot = (currentline+squareSize)*nPoints + currentcol;
                  int mid = (int)((currentline+half)*nPoints)+ (int)(currentcol + half);

                  diamond(top, bot, mid, squareSize, 0.7f*varHeight);
                  square(top, bot, mid, squareSize, 0.7f*varHeight);

                  currentcol += squareSize;

                }
                currentline += squareSize;

            }
            splitInto *= 2;
            squareSize /= 2;
            varHeight *= 0.5f;

        }

        currMaxHeight = -maxHeight;
        currMinHeight = maxHeight;
        // find the max height
        for ( int i=0; i< numVerts; i++)
        {
            if(allVerts[i].y > currMaxHeight)
            {
                currMaxHeight = allVerts[i].y;
            }
            if (allVerts[i].y < currMinHeight)
            {
                currMinHeight = allVerts[i].y;
            }
        }


        Color[] terrainColor = new Color[numVerts];

        mountainHeight = (Math.Abs(currMaxHeight) + Math.Abs(currMinHeight))/2;
        waterLevel = (currMaxHeight + currMinHeight) / 2;

        //adding colors
        for (int i = 0; i < allVerts.Length; i++)
        {
            if (allVerts[i].y > (waterLevel + 0.82 * mountainHeight))
            {

                terrainColor[i] = new Color(250/255f, 250/255f, 250/255f, 1.0f); //snow

            }
            else if (allVerts[i].y > (waterLevel + 0.6 * mountainHeight) && allVerts[i].y < (waterLevel + 0.82 * mountainHeight))
            {

                terrainColor[i] = new Color(128/255f, 132/255f, 135/255f, 1.0f); //rock (128,132,135)

            }
            else if (allVerts[i].y < (waterLevel + 0.6 * mountainHeight) && allVerts[i].y > (waterLevel + mountainHeight * 0.05))
            {

                terrainColor[i] = new Color(30/255f, 130/255f, 76/255f, 1.0f); // grassland rgba(30/255, 130/255, 76/255, 1)

            }
            else if (allVerts[i].y > waterLevel && allVerts[i].y < (waterLevel + mountainHeight * 0.05))
            {

                terrainColor[i] = new Color(129/255f, 108/255f, 91/255f, 1.0f); // beach (129,108,91)

            }
            
            else if (allVerts[i].y < waterLevel)
            {

                terrainColor[i] = new Color(69/255f, 59/255f, 048/255f, 1.0f); //riverbed(69,59,48)

            }
            //Debug.Log(terrainColor[i]);

        }

        m.vertices = allVerts;
        m.triangles = triangles;
        m.uv = uvs;
        m.colors = terrainColor;
        m.RecalculateBounds();
        m.RecalculateNormals();

        return m;

    }

    // diamond steps
    void diamond(int top, int bot, int mid, int size, float noise ){
        allVerts[mid].y = (allVerts[top].y + allVerts[top+size].y + allVerts[bot+size].y + allVerts[bot].y)*0.25f + UnityEngine.Random.Range(-noise, noise);
    }

    // square steps
    void square(int top, int bot, int mid, int size, float noise){
        int half = (int)(size*0.5f);

        allVerts[top+half].y = (allVerts[top].y + allVerts[top+size].y + allVerts[mid].y)/3 + UnityEngine.Random.Range(-noise, noise);
        allVerts[mid-half].y = (allVerts[top].y + allVerts[bot].y + allVerts[mid].y)/3 + UnityEngine.Random.Range(-noise, noise);
        allVerts[mid+half].y = (allVerts[top+size].y + allVerts[mid].y + allVerts[bot+size].y)/3 + UnityEngine.Random.Range(-noise, noise);
        allVerts[bot+half].y = (allVerts[bot+size].y + allVerts[bot].y + allVerts[mid].y)/3 + UnityEngine.Random.Range(-noise, noise);
    }
}
