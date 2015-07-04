using UnityEngine;
using UnityEngine.Rendering;
using Emgen;

[ExecuteInEditMode]
public class ShellRenderer : MonoBehaviour
{
    #region Public Properties

    [SerializeField, Range(0, 4)]
    int _subdivision = 2;

    [SerializeField]
    float _waveSpeed = 8;

    [SerializeField]
    Vector3 _alpha = new Vector3(3.3f, 1.3f, 1);

    [SerializeField]
    Vector3 _beta = new Vector3(0.028f, 0.047f, 0.032f);

    [SerializeField]
    Vector3 _gamma = new Vector3(1, 5.789f, 5.2f);

    [SerializeField, Range(0, 1)]
    float _cutoff = 0.5f;

    [SerializeField]
    float _noiseSpeed = 3;

    [SerializeField]
    float _noiseFrequency = 3;

    [SerializeField]
    float _noiseAmplitude = 5;

    [SerializeField]
    float _noiseExponent = 2;

    [SerializeField]
    Material _material;

    [SerializeField]
    bool _receiveShadows;

    [SerializeField]
    ShadowCastingMode _shadowCastingMode;

    #endregion

    #region Private Members

    Mesh _mesh;
    bool _needsReset = true;

    float _waveTime;
    Vector3 _noiseOffset;

    public void NotifyConfigChange()
    {
        _needsReset = true;
    }

    void ResetResources()
    {
        if (_mesh) DestroyImmediate(_mesh);
        BuildMesh();
        _needsReset = false;
    }

    #endregion

    #region MonoBehaviour Functions

    void Update()
    {
        if (_needsReset) ResetResources();

        var noiseDir = new Vector3(1, 0.5f, 0.2f).normalized;

        _waveTime += Time.deltaTime * _waveSpeed;
        _noiseOffset += noiseDir * (Time.deltaTime * _noiseSpeed);

        var props = new MaterialPropertyBlock();

        props.SetFloat("_Cutoff", _cutoff);

        props.SetFloat("_WTime", _waveTime);
        props.SetVector("_WParams1", _alpha);
        props.SetVector("_WParams2", _beta);
        props.SetVector("_WParams3", _gamma);

        props.SetVector("_NOffset", _noiseOffset);

        var np = new Vector3(_noiseFrequency, _noiseAmplitude, _noiseExponent);
        props.SetVector("_NParams", np);

        Graphics.DrawMesh(
            _mesh, transform.position, transform.rotation,
            _material, 0, null, 0, props,
            _shadowCastingMode, _receiveShadows);
    }

    #endregion

    #region Mesh Builder

    void BuildMesh()
    {
        // The Shell vertex shader needs positions of three vertices in a triangle
        // to calculate the normal vector. To provide these information, it uses
        // not only the position attribute but also the normal and tangent attributes
        // to store the 2nd and 3rd vertex position.

        IcosphereBuilder ib = new IcosphereBuilder();

        for (var i = 0; i < _subdivision; i++) ib.Subdivide();

        var vc = ib.vertexCache;

        var vcount = 3 * vc.triangles.Count;
        var va1 = new Vector3[vcount];
        var va2 = new Vector3[vcount];
        var va3 = new Vector4[vcount];

        var vi = 0;
        foreach (var t in vc.triangles)
        {
            var v1 = vc.vertices[t.i1];
            var v2 = vc.vertices[t.i2];
            var v3 = vc.vertices[t.i3];

            va1[vi + 0] = v1;
            va2[vi + 0] = v2;
            va3[vi + 0] = v3;

            va1[vi + 1] = v2;
            va2[vi + 1] = v3;
            va3[vi + 1] = v1;

            va1[vi + 2] = v3;
            va2[vi + 2] = v1;
            va3[vi + 2] = v2;

            vi += 3;
        }

        _mesh = new Mesh();
        _mesh.hideFlags = HideFlags.DontSave;
        _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10);

        _mesh.vertices = va1;
        _mesh.normals  = va2;
        _mesh.tangents = va3;

        _mesh.SetIndices(vc.MakeIndexArrayForFlatMesh(), MeshTopology.Triangles, 0);
    }

    #endregion
}
