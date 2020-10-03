using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class WaterMesh : MonoBehaviour
{

    Mesh mesh;

    public float scaleMultiplier = 1;
    public int gridCount = 16;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    void Start() {
       mesh = new Mesh(); 
       GetComponent<MeshFilter>().mesh = mesh;
       GenerateWater();
       transform.localScale *= scaleMultiplier; 
    }

    void GenerateWater() {

        for (int x = -(gridCount/2); x < gridCount/2 + 1; x++) {
            for (int y = -(gridCount/2); y < gridCount/2 + 1; y++) {
                vertices.Add(new Vector3(x,0,y));
            }
        }
        
        int triangleIndex = 0;
        int rowIndex = 0;
        for (int x = 0; x < gridCount; x++) {
            for (int y = 0; y < gridCount; y++) {

                triangles.Add(rowIndex+0);
                triangles.Add(rowIndex+1);
                triangles.Add(rowIndex+gridCount+1);

                triangles.Add(rowIndex+1);
                triangles.Add(rowIndex+gridCount+2);
                triangles.Add(rowIndex+gridCount+1);

                rowIndex++;
                triangleIndex += 6;
            }
            rowIndex++;
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

    }

}
