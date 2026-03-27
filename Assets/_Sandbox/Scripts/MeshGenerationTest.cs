using UnityEngine;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteInEditMode]
public class MeshGenerationTest : MonoBehaviour
{
    private void GenerateMesh()
    {
        Mesh mesh = new Mesh { name = "GeneratedMesh" };
        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = new Vector3[4]
        {
            new Vector3(-1, 1),
            new Vector3(1, 1),
            new Vector3(1, -1),
            new Vector3(-1, -1)
        };

        mesh.triangles = new int[6] { 0, 1, 2,  0, 2, 3 };

        mesh.uv = new Vector2[4] {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0) };

        mesh.RecalculateNormals();


    }

    private void OnEnable()
    {
        GenerateMesh();
    }
}
