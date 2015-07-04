//
// Burst line
//
using UnityEngine;

[ExecuteInEditMode]
public class BurstRenderer : MonoBehaviour
{
    #region Public Properties

    [SerializeField]
    float _throttle = 1.0f;

    [SerializeField]
    float _radius = 1.0f;

    [ColorUsage(true, true, 0, 8, 0.125f, 3)]
    [SerializeField] Color _color = Color.white;

    #endregion

    #region Private Members

    const int _maxBeams = 32767;

    [SerializeField]
    Shader _lineShader;

    Mesh _mesh;
    Material _material;
    float _waveTime;

    #endregion

    #region MonoBehaviour Functions

    void Update()
    {
        if (_mesh == null) BuildMesh();

        _waveTime += Time.deltaTime * 8;

        _material.SetColor("_Color", _color);
        _material.SetFloat("_Throttle", _throttle);
        _material.SetFloat("_Radius", _radius);

        _material.SetFloat("_Cutoff", 0.4f);

        Vector3 wparam1 = new Vector3(3.1f, 2.3f, 6.3f);
        Vector3 wparam2 = new Vector3(0.031f, 0.022f, 0.039f);
        Vector3 wparam3 = new Vector3(1.21f, 0.93f, 1.73f);

        _material.SetFloat("_WTime", _waveTime);
        _material.SetVector("_WParams1", wparam1);
        _material.SetVector("_WParams2", wparam2);
        _material.SetVector("_WParams3", wparam3);

        Graphics.DrawMesh(
            _mesh, transform.position, transform.rotation,
            _material, 0, null);
    }

    #endregion

    #region Mesh Builder

    void BuildMesh()
    {
        if (_mesh) DestroyImmediate(_mesh);

        var vertices = new Vector3[_maxBeams * 2];
        var texcoords = new Vector2[_maxBeams * 2];
        var indices = new int[_maxBeams * 2];

        for (var i = 0; i < _maxBeams * 2; i += 2)
        {
            vertices[i] = Vector3.zero;
            vertices[i + 1] = Vector3.one;

            texcoords[i] = Vector2.zero;
            texcoords[i + 1] = new Vector2(1.0f * i / 256, 1.0f * i / 65536);

            indices[i] = i;
            indices[i + 1] = i + 1;
        }

        _mesh = new Mesh();
        _mesh.hideFlags = HideFlags.DontSave;
        _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10);
        _mesh.vertices = vertices;
        _mesh.uv = texcoords;
        _mesh.SetIndices(indices, MeshTopology.Lines, 0);

        _material = new Material(_lineShader);
    }

    #endregion
}
