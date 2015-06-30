using UnityEngine;
using System.Collections;

namespace Furball
{
    public class FurballRenderer : MonoBehaviour
    {
        Mesh _mesh;

        [SerializeField]
        Material _material;

        Mesh BuildMesh(VertexCache vc)
        {
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

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.colors = colors;
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);
            mesh.RecalculateNormals();

            return mesh;
        }

        void Start()
        {
            IcosphereBuilder ib = new IcosphereBuilder();
            ib.Subdivide();
            ib.Subdivide();
            _mesh = BuildMesh(ib.vertexCache);
        }

        void Update()
        {
            Graphics.DrawMesh(_mesh, transform.position, transform.rotation, _material, 0);
        }
    }
}
