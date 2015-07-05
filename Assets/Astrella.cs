using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Astrella : MonoBehaviour
{
    #region Public Settings

    // General settings.
    public Transform hipNode;

    // Skeletal particle settings.
    public ParticleSystem emitter;
    [Range(0, 1)] public float particleEmission = 0;

    #endregion

    #region Private Variables

    // Constants.
    const float emissionPointDensity = 60;
    const float maxEmissionFrequency = 1000;

    // Particle emision point array.
    struct EmissionPoint
    {
        public Transform transform;
        public Vector3 position;
        public EmissionPoint(Transform t, Vector3 p)
        {
            this.transform = t;
            this.position = p;
        }
    }
    List<EmissionPoint> emissionPoints;

    // Timer used for particle emission.
    float particleTimer = 0;

    #endregion

    #region Private Methods

    // Traverse the hierarchy and pick up emission points.
    void ScanEmissionPointRecursive(Transform node)
    {
        foreach (Transform t in node)
        {
            if (t.name.EndsWith("Point")) return;

            var intvl = 1.0f / (t.localPosition.magnitude * emissionPointDensity);
            for (var p = 0.0f; p < 1.0f; p += intvl)
                emissionPoints.Add(new EmissionPoint(node, t.localPosition * p));

            if (!t.name.EndsWith("Hand")) ScanEmissionPointRecursive(t);
        }
    }

    // Emit a given number of particles on randomly chosen emission points.
    void EmitParticle(int count)
    {
        var p = new ParticleSystem.Particle();

        // Shared settings.
        p.startLifetime = p.lifetime = Random.Range(0.2f, 0.5f);
        p.angularVelocity = 100;
        p.color = Color.white;

        for (var i = 0; i < count; i++)
        {
            // Pick up an emission point from the array.
            var ep = emissionPoints[Random.Range(0, emissionPoints.Count)];

            // Set the position.
            p.position = ep.transform.TransformPoint(ep.position);
            p.position += Random.insideUnitSphere * (2.0f / emissionPointDensity);

            // Random settings.
            p.velocity = Random.insideUnitSphere * 0.8f;
            p.size = Random.Range(0.015f, 0.08f);
            p.axisOfRotation = Random.onUnitSphere;
            p.rotation = Random.Range(-180.0f, 180.0f);

            // Emit one particle.
            emitter.Emit(p);
        }
    }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        // Initialize the emission point list.
        emissionPoints = new List<EmissionPoint>();
        ScanEmissionPointRecursive(hipNode);
    }

    void Update()
    {
        // Particle emission.
        particleTimer += Time.deltaTime * maxEmissionFrequency * particleEmission;
        var emission = Mathf.FloorToInt(particleTimer);
        EmitParticle(emission);
        particleTimer -= emission;
    }

    #endregion
}
