using UnityEngine;

public class AnalogGlitchController : MonoBehaviour
{
    public Kino.AnalogGlitch target;

    public AnimationCurve kickCurve1;
    public AnimationCurve kickCurve2;

    public float ScanLineJitter { get; set; }
    public float ColorDrift { get; set; }
    public float VerticalJump { get; set; }
    public float HorizontalShake { get; set; }

    float curveTime = 100;
    float kickIntensity = 1;

    public void Kick(float intensity)
    {
        kickIntensity = intensity;
        curveTime = 0;
    }

    void Update()
    {
        var v1 = kickCurve1.Evaluate(curveTime) * kickIntensity;
        var v2 = kickCurve2.Evaluate(curveTime) * kickIntensity;

        var sj = Mathf.Clamp01(v1 + ScanLineJitter);
        var cd = Mathf.Clamp01(v1 + ColorDrift);
        var vj = Mathf.Clamp01(v2 + VerticalJump);
        var hs = Mathf.Clamp01(v2 + HorizontalShake);

        if (sj > 0 || cd > 0 || vj > 0 || hs > 0)
        {
            target.enabled = true;
            target.scanLineJitter = sj;
            target.colorDrift = cd;
            target.verticalJump = vj;
            target.horizontalShake = hs;
        }
        else
        {
            target.enabled = false;
        }

        curveTime += Time.deltaTime;
    }
}
