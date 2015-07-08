Shader "Custom/Squamata"
{
    Properties
    {
        _Color      ("Color",      Color) = (1,1,1,1)
        _Metallic   ("Metallic",   Range(0,1)) = 0.5
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        CGPROGRAM

        #pragma surface surf Standard vertex:vert nolightmap addshadow
        #pragma target 3.0

        #include "ClassicNoise3D.cginc"

        struct Input
        {
            float dummy;
        };

        half4 _Color;
        half _Smoothness;
        half _Metallic;

        float3 _NoiseOffset;
        float _NoiseFrequency;
        float _NoiseAmplitude;
        float _Opacity;
        float _Radius;

        void vert(inout appdata_full v)
        {
            float n = cnoise(mul(_Object2World, v.color) * _NoiseFrequency + _NoiseOffset);
            n = saturate((n + _Opacity * 2 - 1) * _NoiseAmplitude);
            v.vertex.xyz = lerp(v.color.xyz, v.vertex.xyz, n) * _Radius;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
        }

        ENDCG
    }
}
