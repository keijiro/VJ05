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

        #include "ClassicNoise3D.cginc"

        #pragma surface surf Standard vertex:vert fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float dummy;
        };

        half4 _Color;
        half _Smoothness;
        half _Metallic;

        void vert(inout appdata_full v)
        {
            v.vertex.xyz *= 1 + pow(cnoise(v.color + float3(_Time.y, 0, _Time.x)),3) * 2;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = _Color.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
            o.Alpha = _Color.a;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
