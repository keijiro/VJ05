using UnityEngine;
using System.Collections;
using Emgen;

namespace Furball
{
    [ExecuteInEditMode]
    public class FurballRenderer : MonoBehaviour
    {
        [SerializeField, Range(0, 4)]
        int _subdivision = 2;

        [SerializeField]
        Material _material;

        Mesh _mesh;
        bool _needsReset = true;

        void ResetResources()
        {
            if (_mesh) DestroyImmediate(_mesh);
            BuildMesh();
            _needsReset = false;
        }

        void Update()
        {
            if (_needsReset) ResetResources();
            Graphics.DrawMesh(_mesh, transform.position, transform.rotation, _material, 0);
        }

        public void NotifyConfigChange()
        {
            _needsReset = true;
        }

        #region Mesh Builder

        void BuildMesh()
        {
            IcosphereBuilder ib = new IcosphereBuilder();

            for (var i = 0; i < _subdivision; i++) ib.Subdivide();

            var vc = ib.vertexCache;

            var vertices = new Vector3[vc.triangles.Count * 12];
            var colors = new Color[vc.triangles.Count * 12];
            var offs = 0;
            foreach (var t in vc.triangles)
            {
                var v1 = vc.vertices[t.i1];
                var v2 = vc.vertices[t.i2];
                var v3 = vc.vertices[t.i3];

                var c = Vector3.Cross(v2 - v1, v3 - v1).normalized;
                var center = new Color(c.x, c.y, c.z);

                colors[offs] = center;
                vertices[offs++] = v1;
                colors[offs] = center;
                vertices[offs++] = v2;
                colors[offs] = center;
                vertices[offs++] = v3;

                colors[offs] = center;
                vertices[offs++] = Vector3.zero;
                colors[offs] = center;
                vertices[offs++] = v2;
                colors[offs] = center;
                vertices[offs++] = v1;

                colors[offs] = center;
                vertices[offs++] = Vector3.zero;
                colors[offs] = center;
                vertices[offs++] = v3;
                colors[offs] = center;
                vertices[offs++] = v2;

                colors[offs] = center;
                vertices[offs++] = Vector3.zero;
                colors[offs] = center;
                vertices[offs++] = v1;
                colors[offs] = center;
                vertices[offs++] = v3;
            }

            var indices = new int[vertices.Length];
            for (var i = 0; i < indices.Length; i++)
                indices[i] = i;

            if (_mesh) Destroy(_mesh);

            _mesh = new Mesh();
            _mesh.hideFlags = HideFlags.DontSave;
            _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 100);
            _mesh.vertices = vertices;
            _mesh.colors = colors;
            _mesh.SetIndices(indices, MeshTopology.Triangles, 0);
            _mesh.RecalculateNormals();
        }

        #endregion
    }
}
