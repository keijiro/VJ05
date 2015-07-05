using UnityEngine;
using System.Collections;

public class JitterJoint : MonoBehaviour
{
    public static float freq = 0.7f;

    public Vector3 limitAngle;
    public JitterJoint linkedTo;

    public Quaternion CurrentRotation {
        get { return currentRotation; }
    }

    Quaternion originalRotation;
    Quaternion currentRotation;
    Vector3 noiseSeed;
    float time;

    void Awake()
    {
        originalRotation = transform.localRotation;
        noiseSeed = new Vector3(Random.value, Random.value, Random.value) * Mathf.PI * 10;
        currentRotation = Quaternion.identity;
    }

    void Update()
    {
        if (linkedTo)
        {
            currentRotation = linkedTo.CurrentRotation;
        }
        else
        {
            time += Time.deltaTime * freq;
            var rx = limitAngle.x * Reaktion.Perlin.Noise(time, noiseSeed.x) * 2;
            var ry = limitAngle.y * Reaktion.Perlin.Noise(time, noiseSeed.y) * 2;
            var rz = limitAngle.z * Reaktion.Perlin.Noise(time, noiseSeed.z) * 2;
            currentRotation = Quaternion.Euler(rx, ry, rz);
        }
        transform.localRotation = currentRotation * originalRotation;
    }
}
