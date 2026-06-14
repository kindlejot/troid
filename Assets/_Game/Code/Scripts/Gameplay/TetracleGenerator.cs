using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(SkinnedMeshRenderer), typeof(TetracleDriver))]
public class TetracleGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private int segmentsPerArm = 6;
    [SerializeField] private float bodyEdgeDistance = .55f;
    [SerializeField] private float segmentLength = .7f;
    [SerializeField] private float segmentWidth = .5f;
    [SerializeField] private float tapering = .75f;

    private List<Vector3> _vertices = new List<Vector3>();
    private List<int> _triangles = new List<int>();
    private List<Vector2> _uvs = new List<Vector2>();

    private List<Transform> _bones = new List<Transform> ();
    private List<Matrix4x4> _bindPoses = new List<Matrix4x4> ();
    private List<BoneWeight> _boneWeights = new List<BoneWeight> ();

    private void OnEnable()
    {
        GenerateTetracle();
    }

    [ContextMenu("Generate")]
    private void GenerateTetracle()
    {
        _vertices.Clear();
        _triangles.Clear();
        _uvs.Clear();

        _bones.Clear();
        _bindPoses.Clear();
        _boneWeights.Clear();

        for (int i=transform.childCount - 1; i>=0; i--)
        {
            if (Application.isPlaying)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            else
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        Transform bodyRoot = CreateBone("BodyRoot", transform, Vector3.zero);

        Vector3 top = new Vector3(0.0f, 0.0f, -0.5f * bodyEdgeDistance);

        float thirdOfCircle = Mathf.PI * 2 / 3;
        float angle = 0;
        Vector3 a = new Vector3(Mathf.Sin(angle) * bodyEdgeDistance, Mathf.Cos(angle) * bodyEdgeDistance, 0.0f);
        angle += thirdOfCircle;
        Vector3 b = new Vector3(Mathf.Sin(angle) * bodyEdgeDistance, Mathf.Cos(angle) * bodyEdgeDistance, 0.0f);
        angle += thirdOfCircle;
        Vector3 c = new Vector3(Mathf.Sin(angle) * bodyEdgeDistance, Mathf.Cos(angle) * bodyEdgeDistance, 0.0f);

        CreateRiggedTetrahedron(a, b, c, top, 0);

        Vector3[] roots = new[] { (a + b) / 2, (b + c) / 2, (c + a) / 2 };

        float armAngle = thirdOfCircle / 2;

        for (int i=0; i<3; i++)
        {
            Vector3 root = roots[i];
            Vector3 apex = new Vector3(Mathf.Sin(armAngle), Mathf.Cos(armAngle), 0.0f);
            apex *= segmentLength;
            float baseWidth = segmentWidth;

            Transform previousBone = bodyRoot;
            int armStartBoneIndex = _bones.Count;

            for (int j=0; j<segmentsPerArm; j++)
            {
                Transform currentBone = CreateBone($"Arm_{i}_Joint_{j}", previousBone, root);

                int currentBoneIndex = armStartBoneIndex + j;
                AddRiggedSegment(root, root + apex, baseWidth, currentBoneIndex);
                root += apex;
                apex *= tapering;
                baseWidth *= tapering;
                previousBone = currentBone;
            }
            armAngle += thirdOfCircle;
        }

        Mesh mesh = new Mesh { name = "Tetracle" };
        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _triangles.ToArray();
        mesh.uv = _uvs.ToArray();
        mesh.boneWeights = _boneWeights.ToArray();
        mesh.bindposes = _bindPoses.ToArray();
        mesh.RecalculateNormals();

        SkinnedMeshRenderer smr = GetComponent<SkinnedMeshRenderer>();
        smr.sharedMesh = mesh;
        smr.bones = _bones.ToArray();
        smr.rootBone = bodyRoot;

        GetComponent<MeshFilter>().mesh = mesh;

        GetComponent<TetracleDriver>().SetupBones(_bones);
    }

    private Transform CreateBone(string name, Transform parent, Vector3 position)
    {
        GameObject boneObj = new GameObject(name);
        boneObj.transform.parent = parent;
        boneObj.transform.position = transform.TransformPoint(position);

        Vector3 direction = boneObj.transform.position - parent.position;
        if (direction.magnitude > 0.001f)
        {
            boneObj.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
        _bones.Add(boneObj.transform);

        _bindPoses.Add(boneObj.transform.worldToLocalMatrix * transform.localToWorldMatrix);

        return boneObj.transform;
    }

    private void AddRiggedSegment(Vector3 root, Vector3 apex, float baseWidth, int boneIndex)
    {
        Vector3 diagonal = apex - root;
        Vector3 perpendicular = Vector3.Cross(diagonal, Vector3.forward).normalized;

        Vector3 a = apex;
        Vector3 b = root + perpendicular * baseWidth / 2;
        Vector3 c = root - perpendicular * baseWidth / 2;

        Vector3 top = (a + b + c) / 3 + Vector3.back * diagonal.magnitude / 2;

        CreateRiggedTetrahedron(a, b, c, top, boneIndex);
    }

    private void CreateRiggedTetrahedron(Vector3 a, Vector3 b, Vector3 c, Vector3 t, int boneIndex)
    {
        int offset = _vertices.Count;

        BoneWeight weight = new BoneWeight
        {
            boneIndex0 = boneIndex,
            weight0 = 1
        };

        _vertices.AddRange(new Vector3[] { t, a, b });
        _triangles.AddRange(new int[] { offset, offset + 1, offset + 2 });
        _uvs.AddRange(new Vector2[] { new(.5f, .5f), new(0.0f, 1.0f), new(1.0f, 1.0f) });
        _boneWeights.AddRange(new BoneWeight[] { weight, weight, weight });
        offset += 3;

        _vertices.AddRange(new Vector3[] { t, b, c });
        _triangles.AddRange(new int[] { offset, offset + 1, offset + 2 });
        _uvs.AddRange(new Vector2[] { new(.5f, .5f), new(1.0f, 1.0f), new(1.0f, 0.0f) });
        _boneWeights.AddRange(new BoneWeight[] { weight, weight, weight });
        offset += 3;

        _vertices.AddRange(new Vector3[] { t, c, a });
        _triangles.AddRange(new int[] { offset, offset + 1, offset + 2 });
        _uvs.AddRange(new Vector2[] { new(.5f, .5f), new(1.0f, 0.0f), new(0.0f, 0.0f) });
        _boneWeights.AddRange(new BoneWeight[] { weight, weight, weight });
        offset += 3;

        _vertices.AddRange(new Vector3[] { a, c, b });
        _triangles.AddRange(new int[] { offset, offset + 1, offset + 2 });
        _uvs.AddRange(new Vector2[] { new(.5f, .5f), new(0.0f, 0.0f), new(0.0f, 1.0f) });
        _boneWeights.AddRange(new BoneWeight[] { weight, weight, weight });
    }
}
