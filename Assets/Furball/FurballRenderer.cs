//
// Geometric Furball Like Thing
//
using UnityEngine;
using Emgen;

namespace Furball
{
    [ExecuteInEditMode]
    public class FurballRenderer : MonoBehaviour
    {
        #region Public Properties

        [SerializeField, Range(0, 4)]
        int _subdivision = 2;

        [SerializeField]
        float _noiseFrequency = 1;

        [SerializeField]
        float _noiseAmplitude = 1;

        [SerializeField]
        float _noisePowerScale = 1;

        [SerializeField]
        float _noiseSpeed = 0.1f;

        [SerializeField]
        Material _material;

        #endregion

        #region Private Members

        Mesh _mesh;
        Vector3 _noisePosition;
        bool _needsReset = true;

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

            var noiseDir = new Vector3(1, 0.3f, -0.5f).normalized;
            _noisePosition += noiseDir * (_noiseSpeed * Time.deltaTime);

            var props = new MaterialPropertyBlock();
            props.SetVector("_NoiseOffset", _noisePosition);
            props.SetFloat("_NoiseFrequency", _noiseFrequency);
            props.SetFloat("_NoiseAmplitude", _noiseAmplitude);
            props.SetFloat("_NoisePower", _noisePowerScale);

            Graphics.DrawMesh(_mesh, transform.position, transform.rotation, _material, 0, null, 0, props);
        }

        #endregion

        #region Mesh Builder

        void BuildMesh()
        {
            // Make an icosphere.
            IcosphereBuilder ib = new IcosphereBuilder();
            for (var i = 0; i < _subdivision; i++) ib.Subdivide();

            // Vertex array.
            var vc = ib.vertexCache;

            var vertices = new Vector3[vc.triangles.Count * 12];
            var colors = new Color[vc.triangles.Count * 12];

            // Make a funnel shaped surface for each original triangle.
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
                for (var i = 0; i < 12; i++)
                    colors[offs + i] = c;

                // Make each face.
                vertices[offs++] = v1;
                vertices[offs++] = v2;
                vertices[offs++] = v3;

                vertices[offs++] = Vector3.zero;
                vertices[offs++] = v2;
                vertices[offs++] = v1;

                vertices[offs++] = Vector3.zero;
                vertices[offs++] = v3;
                vertices[offs++] = v2;

                vertices[offs++] = Vector3.zero;
                vertices[offs++] = v1;
                vertices[offs++] = v3;
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
        }

        #endregion
    }
}
