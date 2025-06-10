Shader "Custom/TextureCombine"
{
    Properties
    {
        _RedTexture("Red Texture", 2D) = "white" {}
        _GreenTexture("Green Texture", 2D) = "white" {}
        _BlueTexture("Blue Texture", 2D) = "white" {}
        _AlphaTexture("Alpha Texture", 2D) = "white" {}
        _RedChannel("Red Channel", int) = 0.0
        _GreenChannel("Green Channel", int) = 0.0
        _BlueChannel("Blue Channel", int) = 0.0
        _AlphaChannel("Alpha Channel", int) = 0.0
    }

        SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Input properties
            sampler2D _RedTexture;
            sampler2D _GreenTexture;
            sampler2D _BlueTexture;
            sampler2D _AlphaTexture;
            int _RedChannel;
            int _GreenChannel;
            int _BlueChannel;
            int _AlphaChannel;
            // Output texture
            half4 _OutputColor;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float SampleChannel(float4 tex, int channel)
            {
                if(channel == 0) return tex.r;
                else if(channel == 1) return tex.g;
                else if(channel == 2) return tex.b;
                else if(channel == 3) return tex.a;
                else if(channel == 4) return 0;
                else if(channel == 5) return 1;
                else if(channel == 6) return (1-tex.r);
                else if(channel == 7) return (1-tex.g);
                else if(channel == 8) return (1-tex.b);
                else if(channel == 9) return (1-tex.a);
                else return -1;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 red = tex2D(_RedTexture, i.uv);
                fixed4 green = tex2D(_GreenTexture, i.uv);
                fixed4 blue = tex2D(_BlueTexture, i.uv);
                fixed4 alpha = tex2D(_AlphaTexture, i.uv);

                float r = SampleChannel(red, _RedChannel);
                float g = SampleChannel(green, _GreenChannel);
                float b = SampleChannel(blue, _BlueChannel);
                float a = SampleChannel(alpha, _AlphaChannel);
                _OutputColor = fixed4(r, g, b, a);
                return _OutputColor;
            }
            ENDCG
        }
    }
}
