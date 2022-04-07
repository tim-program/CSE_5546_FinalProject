Shader "Unlit/HeatmapShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color0("Color 0",Color) = (0,0,0,1)
        _Color1("Color 1", Color) = (0, .9, .2, 1)
        _Color2("Color 2", Color) = (.9, 1,.3, 1)
        _Color3("Color 3", Color) = (.9,.7 , .1, 1)
        _Color4("Color 4", Color) = (1, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }



            float4 colors[5];
            float pointRanges[5];

            //3 for x coord, y coord, intensity
            //3 * number of allowed points
            float _Hits[3 * 32];
            int _HitCount = 0;

            float4 _Color0;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;

            void init() {
                colors[0] = _Color0;// float4(0, 0, 0, 0);
                colors[1] = _Color1;//float4(0, .9, .2, 1);
                colors[2] = _Color2;//float4(.9, 1, .3, 1);
                colors[3] = _Color3;//float4(.9, .7, .1, 1);
                colors[4] = _Color4;//float4(1, 0, 0, 1);

                pointRanges[0] = 0;
                pointRanges[1] = 0.25;
                pointRanges[2] = 0.5;
                pointRanges[3] = 0.75;
                pointRanges[4] = 1;


            }


            float distsq(float2 a, float2 b) {
                float area_of_effect_size = 0.5;
                float d = max(0.0,1.0 - distance(a, b) / area_of_effect_size);
                d = pow(d, 2);
                return d;
            }

            float3 getHeatForPixel(float weight) {
                if (weight <= pointRanges[0]) {
                    return colors[0];
                }

                if (weight >= pointRanges[4]) {
                    return colors[4];
                }

                for (int i = 1; i < 5; i++) {
                    if (weight < pointRanges[i]) {
                        float dist_from_lower_point = weight - pointRanges[i - 1];
                        float size_of_point_range = pointRanges[i] - pointRanges[i - 1];
                        
                        float ratio = dist_from_lower_point / size_of_point_range;
                        
                        float3 color_range = colors[i] - colors[i - 1];
                        float3 color_contribution = color_range * ratio;
                        
                        float3 new_color = colors[i - 1] + color_contribution;
                        return new_color;
                    }
                }
                return colors[0];
            }

            fixed4 frag(v2f i) : SV_Target
            {
                init();
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 uv = i.uv;
                uv = uv * 4.0 - float2(2.0, 2.0); // change uv coord range 50 -2,2
                
                float totalWeight = 0;
                for (int i = 0; i < _HitCount; i++) {
                    float2 work_pt = float2(_Hits[i*3],_Hits[i*3+1]);
                    float pt_intensity = _Hits[i * 3 + 2];

                    totalWeight += 0.5 * distsq(uv, work_pt) * pt_intensity;
                }

                float3 heat = getHeatForPixel(totalWeight);

                return col + float4(heat,0.5);
            }
            ENDCG
        }
    }
}
