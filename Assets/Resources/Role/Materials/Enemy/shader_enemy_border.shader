Shader "GamePlay/EnemyBorder"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BorderColor ("Border Color", Color) = (1,1,1,1)
    }
    SubShader
    {
       Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }
        LOD 100

        CGINCLUDE
        #include "UnityCG.cginc"
        struct appdata_t
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            float color : COLOR; // 读取 SpriteRenderer 传入的颜色
        };

        struct v2f
        {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
            float4 color : COLOR; // 传递颜色信息
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;
        float4 _BorderColor;
        ENDCG

        Pass
        {
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color; // 传递 SpriteRenderer.color
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 计算九宫格alpha通道的差值，超过一定值的部分就是边框
                float offsetx = 1.0 / _ScreenParams.x;
                float offsety = 1.0 / _ScreenParams.y;
                float4 e1 = tex2D(_MainTex, i.uv + float2(-offsetx, offsety));
                float4 e2 = tex2D(_MainTex, i.uv + float2(0, offsety));
                float4 e3 = tex2D(_MainTex, i.uv + float2(offsetx, offsety));
                float4 e4 = tex2D(_MainTex, i.uv + float2(-offsetx, 0));
                float4 e5 = tex2D(_MainTex, i.uv);
                float4 e6 = tex2D(_MainTex, i.uv + float2(offsetx, 0));
                float4 e7 = tex2D(_MainTex, i.uv + float2(-offsetx, -offsety));
                float4 e8 = tex2D(_MainTex, i.uv + float2(0, -offsety));
                float4 e9 = tex2D(_MainTex, i.uv + float2(offsetx, -offsety));
                float avg =  (e1.a + e2.a + e3.a + e4.a + e5.a + e6.a + e7.a + e8.a + e9.a) / 9;
                float param = 0.01;
                bool isBorder = abs(e5.a - avg) > param;
                if (isBorder) return _BorderColor;
                return e5;
            }
            ENDCG
        }
    }
}
