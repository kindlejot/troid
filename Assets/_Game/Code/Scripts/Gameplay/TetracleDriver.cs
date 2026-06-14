using UnityEngine;
using System.Collections.Generic;

public class TetracleDriver : MonoBehaviour
{
    [Header("Animation Settings")]
    [Range(0.1f, 10f)] public float waveSpeed = 2.5f;
    [Range(1f, 45f)]   public float waveIntensity = 12f;
    [Range(0.1f, 2f)]  public float waveOffset = .4f;


    private List<Transform> _bones = new List<Transform>();
    private List<Quaternion> _restRotations = new List<Quaternion>();

    private bool _isInitialized = false;

    public void SetupBones(List<Transform> bones)
    {
        _bones.Clear();
        _restRotations.Clear();

        foreach (Transform bone in bones)
        {
            if (bone != null && bone.name.Contains("Joint_"))
            {
                _bones.Add(bone);
                _restRotations.Add(bone.localRotation);
            }
        }

        _isInitialized = _bones.Count > 0;
    }

    private void Update()
    {
        if (!_isInitialized)
            return;

        ApplyWaveMotion();
    }

    private void ApplyWaveMotion()
    {
        for (int i=0; i<_bones.Count; i++)
        {
            float time = Time.time * waveSpeed;
            float phase = i * waveOffset;
            float angle = Mathf.Sin(time + i * phase) * waveIntensity;

            _bones[i].localRotation = _restRotations[i] * Quaternion.Euler(0, 0, angle);
        }
    }
}
