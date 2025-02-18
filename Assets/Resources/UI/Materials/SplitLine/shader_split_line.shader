Shader "GamePlay/UI/SplitLine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LineWidth ("线条宽度", Float) = 1.0
        _LineColor ("线条颜色", Color) = (1,1,1,1)
        _LineCount ("线条数量", Float) = 1.0
    }
    SubShader
    {
        Tags {
            "Queue" = "Transparent" 
        }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _LineWidth;
            float4 _LineColor;
            float _LineCount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);
                float split = 1.0 / _LineCount;
                float _LineWidth_Half = _LineWidth / 2;
                for (float j = 0; j < _LineCount; j++)
                {
                    float line_uvx = (j + 1) * split;
                    bool isOverlapX = abs(i.uv.x - line_uvx) < _LineWidth_Half;
                    if (isOverlapX)
                    {
                        // 线条颜色自上而下颜色变浅
                        float strength = i.uv.y;
                        float4 finalColor = lerp(texColor, _LineColor, strength);
                        return finalColor;
                    }
                }

                // 给血条的上方边缘增加一个反光效果
                float heightv = 0.5;
                if (i.uv.y > heightv && texColor.a > 0.1)
                {
                    float4 lightColor = float4(0.8, 0.8, 0.8, 1);
                    float lightStrength = (i.uv.y - heightv) * 2;
                    float4 finalColor = lerp(texColor, lightColor, lightStrength);
                    return finalColor;
                }

                return texColor;
            }
            ENDCG
        }
    }
}