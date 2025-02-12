Shader "GamePlay/UnitPreview"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PreviewColor ("Preview Color", Color) = (1,1,1,1)
        _PreviewAmount ("Preview Amount", Range(0,1)) = 0
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
        float4 _PreviewColor;
        float _PreviewAmount;
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
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = lerp(col.rgb, _PreviewColor.rgb, _PreviewAmount);     
                col.a = _PreviewColor.a * _PreviewAmount;           
                return col;
            }
            ENDCG
        }
    }
}
