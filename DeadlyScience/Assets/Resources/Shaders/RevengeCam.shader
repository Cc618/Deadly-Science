Shader "DeadlyScience/RevengeCam"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            float animRatio = 0;

#define strength .65f
#define minDist .6f
#define maxDist 1.f

            float map(float dist)
            {
                float range = maxDist - minDist;

                dist -= minDist;
                dist /= range;

                return clamp(dist, 0, 1);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Normalized position to center
                float2 position = (i.uv - .5) * 2;
                // Normalized distance to center
                float dist = sqrt(position.x * position.x + position.y * position.y) / 1.41f;
             
                // Map to color
                col.r += map(dist) * strength + animRatio;

                return col;
            }
            ENDCG
        }
    }
}
