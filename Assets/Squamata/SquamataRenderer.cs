//
// Geometric scale ball thing
//
using UnityEngine;
using UnityEngine.Rendering;
using Emgen;

[ExecuteInEditMode]
public class SquamataRenderer : MonoBehaviour
{
    #region Public Properties

    // Subdivision level
    [SerializeField, Range(0, 4)]
    int _subdivision = 2;

    public int subdivision {
        get { return _subdivision; }
        set { _subdivision = Mathf.Clamp(value, 0, 4); }
    }

    // Scale animation
    [SerializeField]
    float _scaleAnimation = 0.1f;

    public float scaleAnimation {
        get { return _scaleAnimation; }
        set { _scaleAnimation = value; }
    }

    // Opacity
    [SerializeField, Range(0, 1)]
    float _opacity = 0.5f;

    public float opacity {
        get { return _opacity; }
        set { _opacity = value; }
    }

    // Noise amplitude
    [SerializeField, Header("Noise Parameters")]
    float _noiseAmplitude = 3;

    public float noiseAmplitude {
        get { return _noiseAmplitude; }
        set { _noiseAmplitude = value; }
    }

    // Noise frequency
    [SerializeField]
    float _noiseFrequency = 2;

    public float noiseFrequency {
        get { return _noiseFrequency; }
        set { _noiseFrequency = value; }
    }

    // Noise speed
    [SerializeField]
    float _noiseSpeed = 3;

    public float noiseSpeed {
        get { return _noiseSpeed; }
        set { _noiseSpeed = value; }
    }

    // Rendering settings
    [SerializeField, Header("Rendering")]
    Material _material;
    bool _owningMaterial; // whether owning the material

    public Material sharedMaterial {
        get { return _material; }
        set { _material = value; }
    }

    public Material material {
        get {
            if (!_owningMaterial) {
                _material = Instantiate<Material>(_material);
                _owningMaterial = true;
            }
            return _material;
        }
        set {
            if (_owningMaterial) Destroy(_material, 0.1f);
            _material = value;
            _owningMaterial = false;
        }
    }

    [SerializeField]
    bool _receiveShadows;

    [SerializeField]
    ShadowCastingMode _shadowCastingMode;

    #endregion

    #region Private Members

    Mesh _mesh;
    int _subdivided = -1;
    Vector3 _noiseOffset;

    #endregion

    #region MonoBehaviour Functions

    void Update()
    {
        if (_subdivided != _subdivision) RebuildMesh();

        var noiseDir = new Vector3(0.1f, 0.3f, -0.5f).normalized;
        _noiseOffset += noiseDir * (Time.deltaTime * _noiseSpeed);

        var radius = 1.0f + Reaktion.Perlin.Noise(Time.time, 23.5f) * _scaleAnimation;

        var props = new MaterialPropertyBlock();
        props.SetVector("_NoiseOffset", _noiseOffset);
        props.SetFloat("_NoiseFrequency", _noiseFrequency);
        props.SetFloat("_NoiseAmplitude", _noiseAmplitude);
        props.SetFloat("_Opacity", _opacity);
        props.SetFloat("_Radius", radius);

        Graphics.DrawMesh(
            _mesh, transform.localToWorldMatrix,
            _material, 0, null, 0, props,
            _shadowCastingMode, _receiveShadows);
    }

    #endregion

    #region Mesh Builder

    void RebuildMesh()
    {
        if (_mesh) DestroyImmediate(_mesh);

        // Make an icosphere.
        IcosphereBuilder ib = new IcosphereBuilder();
        for (var i = 0; i < _subdivision; i++) ib.Subdivide();

        // Vertex array.
        var vc = ib.vertexCache;

        var vertices = new Vector3[vc.triangles.Count * 6];
        var colors = new Color[vc.triangles.Count * 6];

        // Make triangle scales.
        var offs = 0;
        foreach (var t in vc.triangles)
        {
            // Vertices on the original triangle.
            var v1 = vc.vertices[t.i1];
            var v2 = vc.vertices[t.i2];
            var v3 = vc.vertices[t.i3];

            // Get the center of mass and encode it to a color.
            var cc = (v1 + v2 + v3) * 0.3333333f;
            var c = new Color(cc.x, cc.y, cc.z, 1);

            // Fill the color.
            for (var i = 0; i < 6; i++)
                colors[offs + i] = c;

            // Make each face.
            vertices[offs++] = v1;
            vertices[offs++] = v2;
            vertices[offs++] = v3;

            vertices[offs++] = v3;
            vertices[offs++] = v2;
            vertices[offs++] = v1;
        }

        // Index array. Simply enumerates the vertices.
        var indices = new int[vertices.Length];
        for (var i = 0; i < indices.Length; i++)
            indices[i] = i;

        // Build a mesh.
        _mesh = new Mesh();
        _mesh.hideFlags = HideFlags.DontSave;
        _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10);
        _mesh.vertices = vertices;
        _mesh.colors = colors;
        _mesh.SetIndices(indices, MeshTopology.Triangles, 0);
        _mesh.RecalculateNormals();

        _subdivided = _subdivision;
    }

    #endregion
}
