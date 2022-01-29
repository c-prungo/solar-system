using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    Mesh mesh;
    Vector3 pointA;
    Vector3 pointB;
    Vector3 pointC;

    public TerrainFace(
        Mesh mesh,
        Vector3 pointA,
        Vector3 pointB,
        Vector3 pointC
    )
    {
        this.mesh = mesh;
        this.pointA = pointA;
        this.pointB = pointB;
        this.pointC = pointC;
    }

    public void ConstructMesh (int radius, int resolution)
    {
        Vector3[] vertices = CalculateVertices (resolution);

        // normalize the vertices and increase the size by radius
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = vertices[i].normalized * (float)radius;
        }

        int[] triangles = CalculateTriangles (vertices, resolution);

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    Vector3[] CalculateVertices (int resolution)
    {

        // length of the vertices list is equal to the triangular number sequence for resolution = n
        // Dots in triangle = n(n+1)/2 starting at 2 (3 points)
        Vector3[] vertices = new Vector3 [(resolution + 1) * (resolution + 2) / 2];

        // add the very top vertex to the vertices list
        vertices[0] = pointA;

        // for each layer of the triangle (resolution) find the triangle corners
        int vertexIndex = 1;
        // Debug.Log("START" + "\npointA=" + pointA + "\npointB=" + pointB + "\npointC=" + pointC);
        for (int i = 0; i < resolution; i++)
        {
            // i points need to be added to the centre of this row
            float resCalc = ((float)i + 1f) / (float)resolution;

            
            // find leftmost point in the row
            Vector3 rowLeft = VectorUtil.LerpByDistance(pointA, pointB, resCalc);

            // find rightmost point in the row
            Vector3 rowRight = VectorUtil.LerpByDistance(pointA, pointC, resCalc);

            // Debug.Log("i=" + i + "\nrowLeft=" + rowLeft + "\nrowRight=" + rowRight + "\nresCalc=" + resCalc);

            // // add rowLeft
            // vertices[vertexIndex] = rowLeft;
            // vertexIndex++;

            // add points to the line between rowLeft and rowRight (excluding rowLeft and rowRight)
            Vector3[] lineVertices = VectorUtil.FindPointsInLine(rowLeft, rowRight, i+2);
            for (int j = 0; j < lineVertices.Length; j++)
            {
                vertices[vertexIndex] = lineVertices[j];
                vertexIndex++;
            }

            // // add rowRight
            // vertices[vertexIndex] = rowRight;
            // vertexIndex++;
        }

        return vertices;
    }

    static int[] CalculateTriangles (Vector3[] vertices, int resolution)
    {
        int[] triangles = new int[resolution * resolution * 3];
        int triangleIndex = 0;
        int triangleOffset = 1;
        int triangularSequence = 1;
        for (int i = 0; i < (vertices.Length - resolution - 1); i++)
        {

            triangles[triangleIndex] = i;
            triangles[triangleIndex + 1] = i + triangleOffset;
            triangles[triangleIndex + 2] = i + triangleOffset + 1;

            // only generate an adjacent triangle (to the right of the point) if it doesn't line up with the triangular number sequence
            // this is because the triangular number sequence indicates the end of one row
            if ( (i + 1) != triangularSequence )
            {
                triangles[triangleIndex + 3] = i;
                triangles[triangleIndex + 4] = i + triangleOffset + 1;
                triangles[triangleIndex + 5] = i + 1;
                triangleIndex += 3;
            }
            else
            {
                triangleOffset++;
                triangularSequence = (triangleOffset) * (triangleOffset + 1) / 2;
            }

            triangleIndex += 3;
        }

        return triangles;
    }
}
