using UnityEngine;
using Emgen;

[ExecuteInEditMode]
public class ShellRenderer : MonoBehaviour
{
    #region Public Properties

    [SerializeField, Range(0, 4)]
    int _subdivision = 2;

    [SerializeField]
    Material _material;

    #endregion

    #region Private Members

    Mesh _mesh;
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

        Graphics.DrawMesh(_mesh, transform.position, transform.rotation, _material, 0, null, 0);
    }

    #endregion

    #region Mesh Builder

    void BuildMesh()
    {
        IcosphereBuilder ib = new IcosphereBuilder();
        for (var i = 0; i < _subdivision; i++) ib.Subdivide();
        _mesh = ib.vertexCache.BuildFlatMesh();
    }

    #endregion
}
