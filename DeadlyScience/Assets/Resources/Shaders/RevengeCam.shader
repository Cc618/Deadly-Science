// Fx strength
#define STRENGTH .65f
// Minimum distance to center (between 0 and 1) to have a red overlay
#define MIN_DIST .6f
// Maximum distance to center (between 0 and 1) to have a red overlay
#define MAX_DIST 1.f
// Offset of the heart anim effect
#define HEART_POS .05f
// Strength of the animation
#define HEART_STRENGTH .075f

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

            // Ratio between 0 and 1 animated in PlayerCam
            float _AnimRatio = 0;

            float map(float dist)
            {
                float range = MAX_DIST - MIN_DIST;

                dist -= MIN_DIST;
                dist /= range;

                return clamp(dist, 0, 1);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Normalized position to center
                float2 position = (i.uv - .5) * 2;
                // Normalized distance to center
                float dist = (sqrt(position.x * position.x + position.y * position.y) / 1.41f - _AnimRatio * HEART_POS) * (1.f - _AnimRatio * HEART_STRENGTH);
             
                // Map to color
                col.r += map(dist) * STRENGTH;

                return col;
            }
            ENDCG
        }
    }
}
