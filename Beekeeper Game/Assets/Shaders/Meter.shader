Shader "Unlit/Meter"
{
    Properties
    {
        _BackTex ("BackTexture", 2D) = "white" {}
        _FrontTex ("FrontTexture", 2D) = "white" {}
        _Level ("Level", Range(0,1)) = 0
        _Color ("Bar Color", Color) = (0, 0, 0, 0.5)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uvBack : TEXCOORD0;
                float2 uvFront : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _BackTex;
            float4 _BackTex_ST;
            sampler2D _FrontTex;
            float4 _FrontTex_ST;
            float _Level;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uvBack = TRANSFORM_TEX(v.uv, _BackTex);
                o.uvFront = TRANSFORM_TEX(v.uv, _FrontTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                fixed4 back = tex2D(_BackTex, i.uvBack) * 0.5;
                fixed4 bar = (i.uvBack.y < _Level) * (back.a != 0) * _Color;
                back = back + bar;
                fixed4 front = tex2D(_FrontTex, i.uvFront);

                fixed4 col = lerp(back, front, front.a);
                return col;
            }
            ENDCG
        }
    }
}
