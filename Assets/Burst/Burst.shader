Shader "Hidden/Burst Line"
{
    Properties
    {
        _Color ("-", Color) = (1,1,1,1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    struct appdata
    {
        float4 position : POSITION;
        float2 texcoord : TEXCOORD0;
    };

    struct v2f
    {
        float4 position : SV_POSITION;
        half4 color : COLOR;
    };

    half4 _Color;
    float _Throttle;
    float _Radius;

    float _Cutoff;
    float _WTime;
    float3 _WParams1;
    float3 _WParams2;
    float3 _WParams3;

    float nrand(float2 uv, float salt)
    {
        uv += float2(salt, 0);
        return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
    }

    float3 get_random_dir(float uv)
    {
        float u1 = nrand(uv, 0) * 2 - 1;
        float u2 = sqrt(1 - u1 * u1);
        float theta = nrand(uv, 1) * 3.14 * 2;
        return float3(u2 * cos(theta), u2 * sin(theta), u1);
    }

    float wave_alpha(float3 p)
    {
        float a =
            sin(p.x * _WParams1.x * sin(_WTime * _WParams2.x)  * _WParams3.x +
            sin(p.y * _WParams1.y * sin(_WTime * _WParams2.y)) * _WParams3.y +
            sin(p.z * _WParams1.z * sin(_WTime * _WParams2.z)) * _WParams3.z + _WTime);
        return (a + 1) / 2;
    }

    v2f vert(appdata v)
    {
        v2f o;

        float tip = v.position.x;
        float3 dir = get_random_dir(v.texcoord);

        float sw = wave_alpha(dir * tip) < _Cutoff ? _Radius : 1.0;

        float3 pos = dir * tip * nrand(v.texcoord, 2) * sw;

        o.position = mul(UNITY_MATRIX_MVP, float4(pos, 1));

        o.color = _Color;
        o.color.a *= (1.0 - tip);

        return o;
    }

    half4 frag(v2f i) : SV_Target
    {
        return i.color;
    }

    ENDCG

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            //Blend SrcAlpha OneMinusSrcAlpha
            Blend SrcAlpha One
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    } 
}
