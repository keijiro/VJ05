Shader "Custom/Furball"
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

        #pragma surface surf Standard vertex:vert
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
        float _NoisePower;

        void vert(inout appdata_full v)
        {
            float n = cnoise(mul(_Object2World, v.color) * _NoiseFrequency + _NoiseOffset);
            v.vertex.xyz *= 1 + pow(abs(n), _NoisePower) * _NoiseAmplitude;
            //v.vertex.xyz *= 1 + n * _NoiseAmplitude;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
