Shader "Custom/Shell"
{
    Properties
    {
        _Color      ("Base Color", Color)      = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic   ("Metallic",   Range(0,1)) = 0.0

        [HDR] _Emission ("Emission", Color) = (1,1,1)

        _Cutoff ("Cutoff", Range(0,1)) = 0.5

        _Speed ("Speed", float)  = 8
        _Alpha ("Alpha", Vector) = (10, 8, 4, 0)
        _Beta  ("Beta",  Vector) = (0.028, 0.047, 0.032, 0)
        _Gamma ("Gamma", Vector) = (1, 5.789, 5.2, 0)
    }

    CGINCLUDE

    float _Speed;
    float3 _Alpha;
    float3 _Beta;
    float3 _Gamma;

    float wave(float3 p)
    {
        float t = _Time.y * _Speed;
        float a = sin(p.x * _Alpha.x * sin(t * _Beta.x)  * _Gamma.x +
                  sin(p.y * _Alpha.y * sin(t * _Beta.y)) * _Gamma.y +
                  sin(p.z * _Alpha.z * sin(t * _Beta.z)) * _Gamma.z + t);
        return (a + 1) / 2;
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }

        // front-face

        Cull Back
        
        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows addshadow alphatest:_Cutoff
        #pragma target 3.0

        struct Input {
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        half4 _Color;
        half4 _Emission;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Emission = _Emission;
            o.Alpha = wave(IN.worldPos);
        }
        ENDCG

        // back-face

        Cull Front
        
        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows addshadow alphatest:_Cutoff
        #pragma target 3.0

        struct Input {
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        half4 _Color;
        half4 _Emission;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Normal = float3(0, 0, -1);
            o.Albedo = _Color.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = wave(IN.worldPos);
        }
        ENDCG
    }
}
