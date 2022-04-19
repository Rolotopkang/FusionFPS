Shader "Unlit/X-ray"
{
    Properties
    {
        _Color ("_Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        // 先绘制被遮挡绘制 X-ray
        Pass
        {
            Tags {"RenderType"="Opaque" }
            Blend One One
            ZWrite off
            ZTest greater

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
            };

            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.viewDir = ObjSpaceViewDir(v.vertex);
                o.normal = v.normal;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 normal = normalize(i.normal);
                fixed3 viewDir = normalize(i.viewDir);

                float rim = 1 - dot(normal, viewDir);

                return _Color * rim * 0.5;
            }
            ENDCG
        }

        // 再绘制物体
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 worldNormal = normalize(i.worldNormal);
                fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

                fixed hLambert = saturate(dot(worldNormal, worldLightDir));
                hLambert = (hLambert * 0.5 + 0.5);

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                col = col * hLambert;
                return col;
            }
            ENDCG
        }

    }
    Fallback "Specular"
}