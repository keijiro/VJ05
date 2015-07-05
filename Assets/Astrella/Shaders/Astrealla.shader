Shader "Custom/Astrealla"
{
    Properties
    {
        _MainTex ("Albedo, Metallic", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _BumpMap ("Normal", 2D) = "bump"{}
        _MetallicScale ("Metallic Scale", Float) = 1
        _SmoothnessScale ("Smoothness Scale", Float) = 1
        _Desaturation ("Desaturation", Range(0,1)) = 0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        
        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
        half4 _Color;
        half _MetallicScale;
        half _SmoothnessScale;
        half _Desaturation;

        struct Input {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = lerp(c.rgb, Luminance(c.rgb), _Desaturation) * _Color.rgb;
            o.Metallic = c.a * _MetallicScale;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
            o.Smoothness = c.a * _SmoothnessScale;
        }

        ENDCG
    } 
    FallBack "Diffuse"
}
