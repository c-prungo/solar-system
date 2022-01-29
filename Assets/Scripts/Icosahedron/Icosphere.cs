using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icosphere : MonoBehaviour
{

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    public int radius;
    [Range (1, 100)]
    public int resolution;

    private void OnValidate ()
    {
        Initialize ();
        GenerateMesh ();
    }

    void Initialize ()
    {
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[20];
        }
        terrainFaces = new TerrainFace[20];

        for (int i = 0; i < 20; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObject = new GameObject("mesh" + i);
                meshObject.transform.parent = transform;

                meshObject.AddComponent<MeshRenderer> ().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObject.AddComponent<MeshFilter> ();
                meshFilters[i].sharedMesh = new Mesh ();
            }
        }

        Vector3[] vertices = GenerateVertices ();
        GenerateTriangles (vertices);
    }

    Vector3[] GenerateVertices ()
    {

        //////////////////////////////////////////////////////////
        // GENERATE UNIT VERTICES using 3 Orthogonal Rectangles //
        //                                                      //
        //                    /|  - R1                          //
        //                   / |                                //
        //                  /  |                                //
        //                 /   |                                //
        //                |    |                                //
        //               _|    |_______                         //
        //      ________/_|   _|______/__                       //
        //     |          |  |           |                      //
        //     |      ____|  |______     |                      //
        //     |     /    | /      /     |                      //
        //     |____/     |/      /______|  - R2                //
        //         /             /                              //
        //        /_____________/                               //
        //                |    |                                //
        //         ^      |    /                                //
        //        R3      |   /                                 //
        //                |  /                                  //
        //                | /                                   //
        //                |/                                    //
        //                                                      //
        //////////////////////////////////////////////////////////
        // Credit to https://superhedral.com/2020/05/17/building-the-unit-icosahedron/
        // for the majority of the maths / calculations involved in this section of the project

        // The golden ratio
        float t = (1f + (float) Mathf.Sqrt(5f)) / 2f;

        // The short side (if the golden ratio is 1.618... this is compared to the short side of 1)
        float s = 1f;

        // A collection of the 12 vertices
        Vector3[] vertices = new Vector3[12];

        // Trace the four vertices of a Golden rectangle [R1]
        vertices[0] = new Vector3 (-s, t, 0);
        vertices[1] = new Vector3 (s, t, 0);
        vertices[2] = new Vector3 (-s, -t, 0);
        vertices[3] = new Vector3 (s, -t, 0);
        
        // Trace the four verices of a Golden rectangle orthagonal to the last [R2]
        vertices[4] = new Vector3 (0,-s, t);
        vertices[5] = new Vector3 (0, s, t);
        vertices[6] = new Vector3 (0, -s, -t);
        vertices[7] = new Vector3 (0, s, -t);
        
        // Trace the four verices of a Golden rectangle orthagonal to the last two [R3]
        vertices[8] = new Vector3 (t, 0, -s);
        vertices[9] = new Vector3 (t, 0, s);
        vertices[10] = new Vector3 (-t, 0, -s);
        vertices[11] = new Vector3 (-t, 0, s);

        return vertices;
    }

    void GenerateTriangles (Vector3[] vertices)
    {

        int[] circularPointsA = new int[]
        {
            11, 5, 1, 7, 10
        };

        int[] circularPointsB = new int[]
        {
            9, 4, 2, 6, 8
        };

        // 5 faces around point 0
        // terrainFaces[0] = new TerrainFace ( meshFilters[0].sharedMesh, vertices[0], vertices[11], vertices[5] );
        // terrainFaces[1] = new TerrainFace ( meshFilters[1].sharedMesh, vertices[0], vertices[5], vertices[1] );
        // terrainFaces[2] = new TerrainFace ( meshFilters[2].sharedMesh, vertices[0], vertices[1], vertices[7] );
        // terrainFaces[3] = new TerrainFace ( meshFilters[3].sharedMesh, vertices[0], vertices[7], vertices[10] );
        // terrainFaces[4] = new TerrainFace ( meshFilters[4].sharedMesh, vertices[0], vertices[10], vertices[11] );

        // generated dynamically
        for (int i = 0; i < 5; i++)
        {
            terrainFaces[i] = new TerrainFace (
                meshFilters[i].sharedMesh,
                vertices[0],
                vertices[getCircularPointA(i)],
                vertices[getCircularPointA(i + 1)]
            );
        }

        // 5 faces adjacent to the faces around point 0
        // terrainFaces[5] = new TerrainFace ( meshFilters[5].sharedMesh, vertices[1], vertices[5], vertices[9] );
        // terrainFaces[6] = new TerrainFace ( meshFilters[6].sharedMesh, vertices[5], vertices[11], vertices[4] );
        // terrainFaces[7] = new TerrainFace ( meshFilters[7].sharedMesh, vertices[11], vertices[10], vertices[2] );
        // terrainFaces[8] = new TerrainFace ( meshFilters[8].sharedMesh, vertices[10], vertices[7], vertices[6] );
        // terrainFaces[9] = new TerrainFace ( meshFilters[9].sharedMesh, vertices[7], vertices[1], vertices[8] );

        // generated dynamically
        for (int i = 0; i < 5; i++)
        {
            terrainFaces[i + 5] = new TerrainFace (
                meshFilters[i + 5].sharedMesh,
                vertices[getCircularPointA(2 - i)],
                vertices[getCircularPointA(1 - i)],
                vertices[getCircularPointB(i)]
            );
        }

        // 5 faces around point 3
        // terrainFaces[10] = new TerrainFace ( meshFilters[10].sharedMesh, vertices[3], vertices[9], vertices[4] );
        // terrainFaces[11] = new TerrainFace ( meshFilters[11].sharedMesh, vertices[3], vertices[4], vertices[2] );
        // terrainFaces[12] = new TerrainFace ( meshFilters[12].sharedMesh, vertices[3], vertices[2], vertices[6] );
        // terrainFaces[13] = new TerrainFace ( meshFilters[13].sharedMesh, vertices[3], vertices[6], vertices[8] );
        // terrainFaces[14] = new TerrainFace ( meshFilters[14].sharedMesh, vertices[3], vertices[8], vertices[9] );

        // generated dynamically
        for (int i = 0; i < 5; i++)
        {
            terrainFaces[i + 10] = new TerrainFace (
                meshFilters[i + 10].sharedMesh,
                vertices[3],
                vertices[getCircularPointB(i)],
                vertices[getCircularPointB(i + 1)]
            );
        }

        // 5 faces adjacent to the faces around point 3
        // terrainFaces[15] = new TerrainFace ( meshFilters[15].sharedMesh, vertices[4], vertices[9], vertices[5] );
        // terrainFaces[16] = new TerrainFace ( meshFilters[16].sharedMesh, vertices[2], vertices[4], vertices[11] );
        // terrainFaces[17] = new TerrainFace ( meshFilters[17].sharedMesh, vertices[6], vertices[2], vertices[10] );
        // terrainFaces[18] = new TerrainFace ( meshFilters[18].sharedMesh, vertices[8], vertices[6], vertices[7] );
        // terrainFaces[19] = new TerrainFace ( meshFilters[19].sharedMesh, vertices[9], vertices[8], vertices[1] );

        // generated dynamically
        for (int i = 0; i < 5; i++)
        {
            terrainFaces[i + 15] = new TerrainFace (
                meshFilters[i + 15].sharedMesh,
                vertices[getCircularPointB(i + 1)],
                vertices[getCircularPointB(i)],
                vertices[getCircularPointA(1 - i)]
            );
        }
    }

    // mathematically selected list of neighbours to point 0
    int getCircularPointA (int index)
    {

        int[] circularPoints = new int[]
        {
            11, 5, 1, 7, 10
        };

        return circularPoints[getCircularPoint(index)];

    }

    // mathematically selected list of neighbours to point 3
    int getCircularPointB (int index)
    {

        int[] circularPoints = new int[]
        {
            9, 4, 2, 6, 8
        };

        return circularPoints[getCircularPoint(index)];

    }

    // ensure the index is within 0-4 (since each point on an icososphere has 5 neighbours)
    int getCircularPoint (int index)
    {
        return (index + 10) % 5;
    }

    void GenerateMesh ()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh (radius, resolution);
        }
    }
}
